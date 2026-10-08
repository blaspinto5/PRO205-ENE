using Codice.Application;
using Codice.Domain;
using Codice.UnitTests.Support;

namespace Codice.UnitTests.Application;

public sealed class AcademicServicesTests
{
    [Fact]
    public async Task RegisterStudent_WhenDataIsValid_PersistsStudent()
    {
        // Arrange
        var store = new MemoryRepositories();
        var service = new StudentService(store);
        // Act
        var student = await service.RegisterStudentAsync(" s001 ", " Ana ", " Pérez ");
        // Assert
        Assert.Same(student, Assert.Single(store.Students));
        Assert.Equal("S001", student.StudentCode);
        Assert.Equal("Ana", student.FirstName);
        Assert.Equal(1, store.Saves);
    }

    [Fact]
    public async Task RegisterStudent_WhenNormalizedCodeExists_RejectsDuplicate()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var service = new StudentService(fixture.Store);
        // Act
        await Assert.ThrowsAsync<DomainException>(() => service.RegisterStudentAsync(" s001 ", "Otra", "Persona"));
        // Assert
        Assert.Single(fixture.Store.Students);
        Assert.Equal(0, fixture.Store.Saves);
    }

    [Fact]
    public async Task UpdateStudentAndSubject_WhenNamesChange_PreservesAcademicLinks()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var studentCode = fixture.Student.StudentCode;
        var subjectCode = fixture.Course.Subject.Code;
        // Act
        await new StudentService(fixture.Store).UpdateStudentAsync(fixture.Student.Id, "María", "Soto");
        await new SubjectService(fixture.Store).UpdateSubjectAsync(fixture.Course.Subject.Id, "Taller de Programación");
        // Assert
        Assert.Same(fixture.Student, fixture.Enrollment.Student);
        Assert.Equal("María", fixture.Enrollment.Student.FirstName);
        Assert.Equal("Taller de Programación", fixture.Enrollment.Course.Subject.Name);
        Assert.Equal(studentCode, fixture.Student.StudentCode);
        Assert.Equal(subjectCode, fixture.Course.Subject.Code);
    }

    [Fact]
    public async Task CreateCourseAndEvents_WhenDataIsValid_PersistsAcademicContext()
    {
        // Arrange
        var fixture = new AcademicFixture();
        // Act
        var course = await fixture.Courses.CreateCourseAsync(fixture.Course.Subject.Id, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate);
        var enrollment = await fixture.Courses.EnrollStudentAsync(course.Id, fixture.Student.Id, new DateOnly(2026, 9, 1));
        var assessment = await fixture.Courses.CreateAssessmentAsync(course.Id, "Eval", new DateOnly(2026, 10, 1));
        var session = await fixture.Courses.CreateSessionAsync(course.Id, new DateTime(2026, 10, 5, 9, 0, 0));
        // Assert
        Assert.Same(enrollment, Assert.Single(course.Enrollments));
        Assert.Same(assessment, Assert.Single(course.Assessments));
        Assert.Same(session, Assert.Single(course.Sessions));
        Assert.Equal(4, fixture.Store.Saves);
    }

    [Fact]
    public async Task RegisterSubjectAndCourse_WhenNormalizedKeysExist_RejectsDuplicates()
    {
        // Arrange
        var fixture = new AcademicFixture();
        // Act
        await Assert.ThrowsAsync<DomainException>(() => new SubjectService(fixture.Store).RegisterSubjectAsync(" pro205 ", "Other"));
        await Assert.ThrowsAsync<DomainException>(() => fixture.Courses.CreateCourseAsync(fixture.Course.Subject.Id, " a ", " 2026-2 ", fixture.Course.StartDate, fixture.Course.EndDate));
        // Assert
        Assert.Single(fixture.Store.Subjects);
        Assert.Single(fixture.Store.Courses);
        Assert.Equal(0, fixture.Store.Saves);
    }

    [Fact]
    public async Task RecordAttendance_WhenRecordExists_RejectsDuplicateAndAllowsExplicitCorrection()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var record = await fixture.Attendance.RecordAttendanceAsync(fixture.Course.Id, fixture.Enrollment.Id, fixture.Session.Id, AttendanceStatus.Present);
        fixture.Clock.Timestamp = fixture.Clock.Timestamp.AddMinutes(2);
        // Act
        await Assert.ThrowsAsync<DomainException>(() => fixture.Attendance.RecordAttendanceAsync(fixture.Course.Id, fixture.Enrollment.Id, fixture.Session.Id, AttendanceStatus.Absent));
        await fixture.Attendance.UpdateAttendanceAsync(fixture.Course.Id, record.Id, AttendanceStatus.Absent);
        // Assert
        Assert.Single(fixture.Enrollment.AttendanceRecords);
        Assert.Equal(AttendanceStatus.Absent, record.Status);
        Assert.Equal(fixture.Clock.Timestamp, record.UpdatedAt);
        Assert.Equal(2, fixture.Store.Saves);
    }

    [Fact]
    public async Task UpdateGrade_WhenValueChanges_UpdatesExistingRecord()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var grade = await fixture.Grades.RegisterGradeAsync(fixture.Course.Id, fixture.Enrollment.Id, fixture.Assessment.Id, 5m);
        fixture.Clock.Timestamp = fixture.Clock.Timestamp.AddMinutes(1);
        // Act
        await fixture.Grades.UpdateGradeAsync(fixture.Course.Id, grade.Id, 6m);
        // Assert
        Assert.Same(grade, Assert.Single(fixture.Enrollment.Grades));
        Assert.Equal(6m, grade.Value);
        Assert.Equal(fixture.Clock.Timestamp, grade.UpdatedAt);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetStudentReport_WhenGradesAreMissing_PreservesPendingState(bool hasGrade)
    {
        // Arrange
        var fixture = new AcademicFixture();
        fixture.Course.AddAssessment("Prueba 2", new DateOnly(2026, 10, 2));
        if (hasGrade) await fixture.Grades.RegisterGradeAsync(fixture.Course.Id, fixture.Enrollment.Id, fixture.Assessment.Id, 6m);
        // Act
        var report = await fixture.Reports.GetStudentReportAsync(fixture.Student.Id, fixture.Time.LocalNow);
        // Assert
        var row = Assert.Single(report.Courses);
        Assert.Equal(hasGrade ? 6m : (decimal?)null, row.Average);
        Assert.True(row.IsProvisional);
        Assert.Equal(hasGrade ? 1 : 2, row.Evaluations.Count(e => !e.Value.HasValue));
        Assert.Equal(hasGrade ? true : (bool?)null, row.IsAtOrAboveReference);
    }

    [Fact]
    public async Task GetCourseReport_WhenCutoffIsApplied_ExcludesOtherCoursesAndFutureSessions()
    {
        // Arrange
        var fixture = new AcademicFixture();
        fixture.Course.AddSession(new DateTime(2026, 8, 20, 9, 0, 0));
        fixture.Course.AddSession(new DateTime(2026, 10, 4, 9, 0, 0));
        fixture.Course.AddSession(new DateTime(2026, 10, 6, 16, 0, 0));
        var absentSession = fixture.Course.AddSession(new DateTime(2026, 10, 3, 9, 0, 0));
        await fixture.Attendance.RecordAttendanceAsync(fixture.Course.Id, fixture.Enrollment.Id, fixture.Session.Id, AttendanceStatus.Present);
        await fixture.Attendance.RecordAttendanceAsync(fixture.Course.Id, fixture.Enrollment.Id, absentSession.Id, AttendanceStatus.Absent);
        var other = new Course(fixture.Course.Subject, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate);
        other.Enroll(new Student("S2", "Luis", "Díaz"), fixture.Enrollment.EnrolledOn);
        fixture.Store.Courses.Add(other);
        // Act
        var report = await fixture.Reports.GetCourseReportAsync(fixture.Course.Id, new DateTime(2026, 12, 1, 0, 0, 0));
        // Assert
        var row = Assert.Single(report.Students);
        Assert.Equal(fixture.Student.Id, row.StudentId);
        Assert.Equal(new AttendanceCounts(1, 1, 1), row.Attendance);
        Assert.Equal(2, fixture.Store.Saves);
    }

    [Fact]
    public async Task GetReports_WhenNoEnrollmentsExist_ReturnsEmptyWithoutMutatingStore()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var otherStudent = new Student("S2", "Luis", "Díaz");
        fixture.Store.Students.Add(otherStudent);
        var emptyCourse = new Course(fixture.Course.Subject, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate);
        fixture.Store.Courses.Add(emptyCourse);
        // Act
        var studentReport = await fixture.Reports.GetStudentReportAsync(otherStudent.Id, fixture.Time.LocalNow);
        var courseReport = await fixture.Reports.GetCourseReportAsync(emptyCourse.Id, fixture.Time.LocalNow);
        // Assert
        Assert.Empty(studentReport.Courses);
        Assert.Empty(courseReport.Students);
        Assert.Equal(0, fixture.Store.Saves);
    }

    [Fact]
    public async Task RegisterGrade_WhenAssessmentIsFromAnotherCourse_RejectsWithoutSaving()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var other = new Course(fixture.Course.Subject, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate);
        var assessment = other.AddAssessment("Other", fixture.Assessment.Date);
        fixture.Store.Courses.Add(other);
        // Act
        await Assert.ThrowsAsync<DomainException>(() => fixture.Grades.RegisterGradeAsync(fixture.Course.Id, fixture.Enrollment.Id, assessment.Id, 5m));
        // Assert
        Assert.Empty(fixture.Enrollment.Grades);
        Assert.Equal(0, fixture.Store.Saves);
    }

    [Fact]
    public async Task CreateSession_WhenLocalTimeIsSkippedByDaylightSaving_RejectsWithoutSaving()
    {
        // Arrange
        var zone = TimeZoneInfo.CreateCustomTimeZone("TestDST", TimeSpan.Zero, "TestDST", "Standard", "Daylight",
            [TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), TimeSpan.FromHours(1),
                TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1, 2, 0, 0), 9, 1),
                TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1, 2, 0, 0), 11, 1))]);
        var fixture = new AcademicFixture();
        var service = new CourseService(fixture.Store, fixture.Store, fixture.Store, new AcademicTime(fixture.Clock, zone));
        // Act
        await Assert.ThrowsAsync<DomainException>(() => service.CreateSessionAsync(fixture.Course.Id, new DateTime(2026, 9, 1, 2, 30, 0)));
        // Assert
        Assert.Single(fixture.Course.Sessions);
        Assert.Equal(0, fixture.Store.Saves);
    }
}
