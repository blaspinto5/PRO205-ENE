using Codice.Application;
using Codice.Domain;
using Codice.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Codice.IntegrationTests;

public sealed class SqlitePersistenceTests
{
    [Fact]
    public async Task SaveAndReopen_WhenAcademicGraphExists_PreservesIdentityLinksAndReports()
    {
        using var fixture = new DatabaseFixture();
        var course = await fixture.CreateGraphAsync();
        var original = course.Enrollments.Single();
        var loaded = (await fixture.Courses.GetAllAsync()).Single();
        var enrollment = loaded.Enrollments.Single();
        Assert.Equal(course.Id, loaded.Id);
        Assert.Equal(original.Id, enrollment.Id);
        Assert.Equal(original.Grades.Single().Id, enrollment.Grades.Single().Id);
        Assert.Same(loaded, enrollment.Course);
        Assert.Same(loaded.Assessments.Single(), enrollment.Grades.Single().Assessment);
        Assert.Equal(5.4m, enrollment.Grades.Single().Value);
        Assert.Equal(original.Grades.Single().RecordedAt, enrollment.Grades.Single().RecordedAt);
        Assert.Equal(original.Grades.Single().UpdatedAt, enrollment.Grades.Single().UpdatedAt);
        var report = await new ReportService(fixture.Courses, fixture.Students, fixture.Time)
            .GetCourseReportAsync(loaded.Id, fixture.Time.LocalNow);
        Assert.Equal(5.4m, report.Students.Single().Average);
        Assert.Equal(new AttendanceCounts(1, 0, 0), report.Students.Single().Attendance);
        var catalog = await new CatalogService(fixture.Students, fixture.Repositories, fixture.Courses).GetSummaryAsync();
        Assert.Equal(1, catalog.Students);
        Assert.Equal(1, catalog.Subjects);
        Assert.Equal(course.Id, catalog.Courses.Single().Id);
    }

    [Fact]
    public async Task CorrectRecords_WhenReloaded_PersistsChangesWithoutDuplicates()
    {
        using var fixture = new DatabaseFixture();
        var course = await fixture.CreateGraphAsync();
        var loaded = (await fixture.Courses.GetAllAsync()).Single();
        var enrollment = loaded.Enrollments.Single();
        var timestamp = fixture.Time.UtcNow.AddMinutes(1);
        enrollment.Grades.Single().ChangeValue(5.5m, timestamp);
        enrollment.AttendanceRecords.Single().ChangeStatus(AttendanceStatus.Absent, timestamp);
        await fixture.Courses.SaveAsync(loaded);
        var reloaded = (await fixture.Courses.GetAllAsync()).Single().Enrollments.Single();
        Assert.Single(reloaded.Grades);
        Assert.Single(reloaded.AttendanceRecords);
        Assert.Equal(5.5m, reloaded.Grades.Single().Value);
        Assert.Equal(timestamp, reloaded.Grades.Single().UpdatedAt);
        Assert.Equal(AttendanceStatus.Absent, reloaded.AttendanceRecords.Single().Status);
        Assert.Equal(course.Id, reloaded.Course.Id);
    }

    [Fact]
    public async Task SaveCourse_WhenStudentWasEditedSeparately_DoesNotOverwriteName()
    {
        using var fixture = new DatabaseFixture();
        await fixture.CreateGraphAsync();
        var staleCourse = (await fixture.Courses.GetAllAsync()).Single();
        var student = (await fixture.Students.GetAllAsync()).Single();
        student.UpdateName("Nombre actualizado", "Apellido");
        await fixture.Students.SaveAsync(student);
        staleCourse.AddAssessment("Nueva evaluación", DateOnly.FromDateTime(fixture.Time.LocalNow));
        await fixture.Courses.SaveAsync(staleCourse);
        Assert.Equal("Nombre actualizado", (await fixture.Students.GetAllAsync()).Single().FirstName);
    }

    [Fact]
    public async Task Save_WhenCodeIsDuplicated_DatabaseRejectsWithoutPartialWrites()
    {
        using var fixture = new DatabaseFixture();
        fixture.Storage.PrepareDirectory();
        await fixture.Storage.InitializeAsync();
        await fixture.Students.SaveAsync(new Student("S001", "Ana", "Pérez"));
        await Assert.ThrowsAsync<DbUpdateException>(() => fixture.Students.SaveAsync(new Student(" s001 ", "Otro", "Apellido")));
        Assert.Single(await fixture.Students.GetAllAsync());
    }

    [Fact]
    public async Task SaveCourse_WhenForeignKeyIsMissing_RollsBackEntireGraph()
    {
        using var fixture = new DatabaseFixture();
        fixture.Storage.PrepareDirectory();
        await fixture.Storage.InitializeAsync();
        var day = DateOnly.FromDateTime(fixture.Time.LocalNow);
        var orphan = new Course(new Subject("NO", "No guardada"), "A", "2026", day, day);
        orphan.AddAssessment("Evaluación", day);
        await Assert.ThrowsAsync<DbUpdateException>(() => fixture.Courses.SaveAsync(orphan));
        Assert.Empty(await fixture.Courses.GetAllAsync());
        await using var db = fixture.Storage.CreateContext();
        Assert.Empty(await db.Set<Assessment>().ToListAsync());
    }

    [Fact]
    public async Task RawWrite_WhenGradeOrAttendanceIsInvalid_CheckConstraintRejects()
    {
        using var fixture = new DatabaseFixture();
        await fixture.CreateGraphAsync();
        await using var db = fixture.Storage.CreateContext();
        await Assert.ThrowsAsync<SqliteException>(() => db.Database.ExecuteSqlRawAsync("UPDATE Grades SET ValueTenths = 71"));
        await Assert.ThrowsAsync<SqliteException>(() => db.Database.ExecuteSqlRawAsync("UPDATE Attendance SET Status = 9"));
        Assert.Equal(5.4m, (await fixture.Courses.GetAllAsync()).Single().Enrollments.Single().Grades.Single().Value);
    }

    [Fact]
    public async Task CopyClosedData_WhenMovedToAccentedPath_ReportsRemainAvailable()
    {
        using var source = new DatabaseFixture();
        using var destination = new DatabaseFixture();
        var course = await source.CreateGraphAsync();
        destination.Storage.PrepareDirectory();
        foreach (var path in Directory.EnumerateFiles(source.Storage.DataDirectory))
            File.Copy(path, Path.Combine(destination.Storage.DataDirectory, Path.GetFileName(path)));
        await destination.Storage.InitializeAsync();
        await destination.Storage.InitializeAsync();
        Assert.Equal(course.Id, (await destination.Courses.GetAllAsync()).Single().Id);
        await using var db = destination.Storage.CreateContext();
        Assert.Equal("ok", await ReadIntegrityAsync(db));
        Assert.Single(await db.Database.GetAppliedMigrationsAsync());
    }

    [Fact]
    public void AcquireLease_WhenAnotherInstanceOwnsFolder_RejectsUntilReleased()
    {
        using var fixture = new DatabaseFixture();
        fixture.Storage.PrepareDirectory();
        using (var first = StorageLease.Acquire(fixture.Storage))
            Assert.Throws<IOException>(() => StorageLease.Acquire(fixture.Storage));
        using var second = StorageLease.Acquire(fixture.Storage);
        Assert.NotNull(second);
    }

    private static async Task<string?> ReadIntegrityAsync(CodiceDbContext db)
    {
        await db.Database.OpenConnectionAsync();
        await using var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = "PRAGMA integrity_check";
        return (string?)await command.ExecuteScalarAsync();
    }

    private sealed class DatabaseFixture : IDisposable
    {
        public PortableStorage Storage { get; } = new(Path.Combine(Path.GetTempPath(), "Códice pruebas con espacios", Guid.NewGuid().ToString("N")));
        public AcademicTime Time { get; } = new(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago"));
        public SqliteRepositories Repositories { get; }
        public ICourseRepository Courses => Repositories;
        public IStudentRepository Students => Repositories;
        public DatabaseFixture() => Repositories = new SqliteRepositories(Storage);

        public async Task<Course> CreateGraphAsync()
        {
            Storage.PrepareDirectory();
            await Storage.InitializeAsync();
            var day = DateOnly.FromDateTime(Time.LocalNow);
            var student = new Student("S001", "Ana", "Pérez");
            var subject = new Subject("PRO205", "Programación");
            await Repositories.SaveAsync(student);
            await Repositories.SaveAsync(subject);
            var course = new Course(subject, "A", "2026", day.AddDays(-60), day.AddDays(60));
            var enrollment = course.Enroll(student, day.AddDays(-30));
            var assessment = course.AddAssessment("Evaluación", day.AddDays(-1));
            var session = course.AddSession(Time.LocalNow.AddDays(-1));
            enrollment.RegisterGrade(assessment, 5.4m, Time.UtcNow);
            enrollment.RecordAttendance(session, AttendanceStatus.Present, Time.LocalNow, Time.UtcNow);
            await Repositories.SaveAsync(course);
            return course;
        }

        public void Dispose()
        {
            if (Directory.Exists(Storage.Root)) Directory.Delete(Storage.Root, true);
        }
    }
}
