using System.Globalization;
using System.IO;
using Codice.Desktop;
using Codice.Desktop.ViewModels;
using Codice.Domain;
using Codice.Infrastructure;

namespace Codice.DesktopTests;

public sealed class DesktopWorkflowTests
{
    [Fact]
    public async Task StudentAndSubjectForms_WhenCreatedAndEdited_PreserveCodesAndIdentity()
    {
        using var fixture = new Fixture();
        await fixture.InitializeAsync();
        var students = new StudentsViewModel(fixture.Workspace) { Code = "EST-1", FirstName = "Ana", LastName = "Pérez" };
        await Run(students.SaveCommand);
        Assert.False(students.IsError);
        var id = students.Selected!.Id;
        students.FirstName = "Ana María";
        await Run(students.SaveCommand);
        Assert.Equal(id, students.Selected!.Id);
        Assert.Equal("EST-1", students.Selected.Code);
        Assert.Equal("Ana María", students.Selected.FirstName);
        var subjects = new SubjectsViewModel(fixture.Workspace) { Code = "PRO205", Name = "Programación" };
        await Run(subjects.SaveCommand);
        var subjectId = subjects.Selected!.Id;
        subjects.Name = "Programación orientada a objetos";
        await Run(subjects.SaveCommand);
        Assert.Equal(subjectId, subjects.Selected!.Id);
        Assert.Equal("PRO205", subjects.Selected.Code);
        Assert.Single(fixture.Workspace.Snapshot.Students);
        Assert.Single(fixture.Workspace.Snapshot.Subjects);
    }

    [Theory]
    [InlineData("", "Ana", "Pérez")]
    [InlineData("EST-1", "", "Pérez")]
    public async Task StudentForm_WhenRequiredFieldIsMissing_ShowsSpanishErrorWithoutSaving(string code, string first, string last)
    {
        using var fixture = new Fixture();
        await fixture.InitializeAsync();
        var form = new StudentsViewModel(fixture.Workspace) { Code = code, FirstName = first, LastName = last };
        await Run(form.SaveCommand);
        Assert.True(form.IsError);
        Assert.False(string.IsNullOrWhiteSpace(form.Message));
        Assert.Empty(fixture.Workspace.Snapshot.Students);
        await fixture.Workspace.RefreshAsync();
        Assert.Empty(fixture.Workspace.Snapshot.Students);
    }

    [Fact]
    public async Task AcademicForms_WhenEntireFlowIsPerformed_PersistCorrectionsAndBothReportScopes()
    {
        using var fixture = new Fixture();
        await fixture.InitializeAsync();
        var student = new StudentsViewModel(fixture.Workspace) { Code = "EST-1", FirstName = "Ana", LastName = "Pérez" };
        await Run(student.SaveCommand);
        var subject = new SubjectsViewModel(fixture.Workspace) { Code = "PRO205", Name = "Programación" };
        await Run(subject.SaveCommand);
        var today = fixture.Workspace.Time.LocalNow.Date;
        var course = new CoursesViewModel(fixture.Workspace)
        {
            Subject = fixture.Workspace.Snapshot.Subjects.Single(), Start = today.AddDays(-30), End = today.AddDays(30),
            Section = "A", Period = "2026-2", Student = fixture.Workspace.Snapshot.Students.Single(), EnrollmentDate = today.AddDays(-20)
        };
        await Run(course.CreateCommand);
        Assert.False(course.IsError);
        await Run(course.EnrollCommand);
        Assert.Single(course.Selected!.Enrollments);
        course.AssessmentName = "Prueba 1"; course.AssessmentDate = today.AddDays(-2);
        await Run(course.AssessmentCommand);
        course.AssessmentName = "Prueba 2";
        await Run(course.AssessmentCommand);
        course.SessionDate = today.AddDays(-1); course.SessionTime = "09:00";
        await Run(course.SessionCommand);
        Assert.False(course.IsError);

        var grades = new GradesViewModel(fixture.Workspace);
        await grades.ActivateAsync();
        grades.Course = grades.Courses.Single(); grades.Assessment = grades.Assessments.First();
        grades.Enrollment = grades.Enrollments.Single(); grades.Value = "5,4";
        await Run(grades.RegisterCommand);
        Assert.False(grades.IsError);
        var gradeId = grades.Enrollment!.Grades.Single().Id;
        grades.Value = "5.5";
        await Run(grades.CorrectCommand);
        Assert.Equal(gradeId, grades.Enrollment!.Grades.Single().Id);
        Assert.Equal(5.5m, grades.Enrollment.Grades.Single().Value);
        grades.Value = "7,1";
        await Run(grades.CorrectCommand);
        Assert.True(grades.IsError);
        Assert.Contains("1,0 y 7,0", grades.Message);
        await fixture.Workspace.RefreshAsync();
        Assert.Equal(5.5m, fixture.Workspace.Snapshot.Courses.Single().Enrollments.Single().Grades.Single().Value);

        var attendance = new AttendanceViewModel(fixture.Workspace);
        await attendance.ActivateAsync();
        attendance.Course = attendance.Courses.Single(); attendance.Session = attendance.Sessions.Single();
        attendance.Enrollment = attendance.Enrollments.Single(); attendance.Status = "Presente";
        await Run(attendance.RegisterCommand);
        Assert.False(attendance.IsError);
        var recordId = attendance.Enrollment!.Attendance.Single().Id;
        attendance.Status = "Ausente";
        await Run(attendance.CorrectCommand);
        Assert.Equal(recordId, attendance.Enrollment!.Attendance.Single().Id);
        Assert.Equal(AttendanceStatus.Absent, attendance.Enrollment.Attendance.Single().Status);

        var reports = new ReportsViewModel(fixture.Workspace) { Course = fixture.Workspace.Snapshot.Courses.Single() };
        await Run(reports.GenerateCommand);
        Assert.False(reports.IsError);
        Assert.Single(reports.Rows);
        Assert.Equal("Provisional", reports.Rows.Single().State);
        Assert.Equal("Pendiente", reports.Evaluations.Last().Value);
        Assert.Equal(1, reports.Rows.Single().Absent);
        reports.Kind = "Por estudiante"; reports.Student = fixture.Workspace.Snapshot.Students.Single();
        await Run(reports.GenerateCommand);
        Assert.Single(reports.Rows);
        Assert.Equal(2, reports.Evaluations.Count);
        Assert.Equal("Promedio ≥ 4,0", reports.Rows.Single().Reference);
        reports.Course = fixture.Workspace.Snapshot.Courses.Single();
        await Run(reports.GenerateCommand);
        Assert.Single(reports.Rows);
        var fresh = new Workspace(new SqliteRepositories(fixture.Storage));
        await fresh.RefreshAsync();
        Assert.Equal(gradeId, fresh.Snapshot.Courses.Single().Enrollments.Single().Grades.Single().Id);
        Assert.Equal(recordId, fresh.Snapshot.Courses.Single().Enrollments.Single().Attendance.Single().Id);
    }

    [Fact]
    public async Task AttendanceForm_WhenSessionIsFuture_ShowsErrorAndLeavesRecordPending()
    {
        using var fixture = new Fixture();
        await fixture.InitializeAsync();
        var workspace = fixture.Workspace;
        var student = await workspace.Students.RegisterStudentAsync("S1", "Ana", "Pérez");
        var subject = await workspace.Subjects.RegisterSubjectAsync("P1", "Programación");
        var day = DateOnly.FromDateTime(workspace.Time.LocalNow);
        var course = await workspace.Courses.CreateCourseAsync(subject.Id, "A", "2026", day, day.AddDays(2));
        await workspace.Courses.EnrollStudentAsync(course.Id, student.Id, day);
        await workspace.Courses.CreateSessionAsync(course.Id, day.AddDays(1).ToDateTime(new TimeOnly(9, 0)));
        await workspace.RefreshAsync();
        var form = new AttendanceViewModel(workspace);
        await form.ActivateAsync();
        form.Course = form.Courses.Single(); form.Session = form.Sessions.Single(); form.Enrollment = form.Enrollments.Single();
        await Run(form.RegisterCommand);
        Assert.True(form.IsError);
        Assert.Contains("futura", form.Message);
        Assert.Equal("Sin registrar", form.Rows.Single().Status);
        Assert.Empty(form.Enrollment.Attendance);
    }

    [Fact]
    public async Task CoursesForm_WhenSessionTimeIsMalformed_DoesNotCreateSession()
    {
        using var fixture = new Fixture();
        await fixture.InitializeAsync();
        var subject = await fixture.Workspace.Subjects.RegisterSubjectAsync("PRO", "Programación");
        var day = DateOnly.FromDateTime(fixture.Workspace.Time.LocalNow);
        await fixture.Workspace.Courses.CreateCourseAsync(subject.Id, "A", "2026", day, day.AddDays(5));
        await fixture.Workspace.RefreshAsync();
        var form = new CoursesViewModel(fixture.Workspace) { Selected = fixture.Workspace.Snapshot.Courses.Single(), SessionTime = "hora inválida" };
        await Run(form.SessionCommand);
        Assert.True(form.IsError);
        Assert.Contains("HH:mm", form.Message);
        Assert.Empty(form.Selected!.Sessions);
    }

    private static Task Run(System.Windows.Input.ICommand command) => ((AsyncCommand)command).ExecuteAsync();

    private sealed class Fixture : IDisposable
    {
        public PortableStorage Storage { get; } = new(Path.Combine(Path.GetTempPath(), "Codice-desktop-tests", Guid.NewGuid().ToString("N")));
        public Workspace Workspace { get; }
        public Fixture()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("es-CL");
            Workspace = new Workspace(new SqliteRepositories(Storage));
        }
        public async Task InitializeAsync() { Storage.PrepareDirectory(); await Storage.InitializeAsync(); await Workspace.RefreshAsync(); }
        public void Dispose() { if (Directory.Exists(Storage.Root)) Directory.Delete(Storage.Root, true); }
    }
}
