using Codice.Domain;
using Codice.UnitTests.Support;

namespace Codice.UnitTests.Domain;

public sealed class AcademicRulesTests
{
    [Theory]
    [InlineData("", "Pérez")]
    [InlineData("Ana", " ")]
    public void CreateStudent_WhenRequiredNameIsEmpty_ThrowsException(string first, string last)
    {
        // Arrange
        var code = "S1";
        // Act
        var error = Assert.Throws<DomainException>(() => new Student(code, first, last));
        // Assert
        Assert.Contains("required", error.Message);
    }

    [Fact]
    public void UpdateName_WhenLastNameIsInvalid_PreservesBothNames()
    {
        // Arrange
        var student = new Student("S1", "Ana", "Pérez");
        // Act
        Assert.Throws<DomainException>(() => student.UpdateName("María", ""));
        // Assert
        Assert.Equal("Ana", student.FirstName);
        Assert.Equal("Pérez", student.LastName);
    }

    [Theory]
    [InlineData("0.9")]
    [InlineData("7.1")]
    [InlineData("5.45")]
    public void ChangeValue_WhenValueIsInvalid_PreservesValueAndTimestamp(string input)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var grade = fixture.Enrollment.RegisterGrade(fixture.Assessment, 5m, fixture.Clock.Timestamp);
        var value = decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
        // Act
        Assert.Throws<DomainException>(() => grade.ChangeValue(value, fixture.Clock.Timestamp.AddMinutes(1)));
        // Assert
        Assert.Equal(5m, grade.Value);
        Assert.Equal(grade.RecordedAt, grade.UpdatedAt);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    public void ChangeValue_WhenValueIsValid_UpdatesValueAndTimestamp(int value)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var grade = fixture.Enrollment.RegisterGrade(fixture.Assessment, 5m, fixture.Clock.Timestamp);
        var changedAt = fixture.Clock.Timestamp.AddMinutes(1);
        // Act
        grade.ChangeValue(value, changedAt);
        // Assert
        Assert.Equal(value, grade.Value);
        Assert.Equal(changedAt, grade.UpdatedAt);
        Assert.Equal(fixture.Clock.Timestamp, grade.RecordedAt);
    }

    [Theory]
    [InlineData("5.4", "5.5", "5.5")]
    [InlineData("5.4", "5.4", "5.4")]
    [InlineData("3.9", "4.0", "4.0")]
    public void CalculateAverage_WhenGradesExist_ReturnsRoundedMean(string first, string second, string expected)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var another = fixture.Course.AddAssessment("Prueba 2", new DateOnly(2026, 10, 2));
        decimal Parse(string value) => decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        fixture.Enrollment.RegisterGrade(fixture.Assessment, Parse(first), fixture.Clock.Timestamp);
        fixture.Enrollment.RegisterGrade(another, Parse(second), fixture.Clock.Timestamp);
        // Act
        var average = fixture.Enrollment.CalculateAverage();
        // Assert
        Assert.Equal(Parse(expected), average);
    }

    [Theory]
    [InlineData("another-course")]
    [InlineData("before-enrollment")]
    [InlineData("duplicate")]
    public void RegisterGrade_WhenContextIsInvalid_RejectsWithoutAddingRecord(string scenario)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var assessment = scenario == "another-course"
            ? new Course(fixture.Course.Subject, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate)
                .AddAssessment("Other", new DateOnly(2026, 10, 1))
            : scenario == "before-enrollment"
                ? fixture.Course.AddAssessment("Early", new DateOnly(2026, 8, 15)) : fixture.Assessment;
        if (scenario == "duplicate") fixture.Enrollment.RegisterGrade(assessment, 5m, fixture.Clock.Timestamp);
        var count = fixture.Enrollment.Grades.Count;
        // Act
        Assert.Throws<DomainException>(() => fixture.Enrollment.RegisterGrade(assessment, 6m, fixture.Clock.Timestamp));
        // Assert
        Assert.Equal(count, fixture.Enrollment.Grades.Count);
    }

    [Theory]
    [InlineData("another-course")]
    [InlineData("before-enrollment")]
    [InlineData("future")]
    [InlineData("invalid-status")]
    public void RecordAttendance_WhenContextIsInvalid_RejectsWithoutAddingRecord(string scenario)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var session = scenario == "another-course"
            ? new Course(fixture.Course.Subject, "B", "2026-2", fixture.Course.StartDate, fixture.Course.EndDate)
                .AddSession(new DateTime(2026, 10, 5, 9, 0, 0))
            : scenario == "before-enrollment" ? fixture.Course.AddSession(new DateTime(2026, 8, 20, 9, 0, 0))
            : scenario == "future" ? fixture.Course.AddSession(new DateTime(2026, 10, 7, 9, 0, 0)) : fixture.Session;
        var status = scenario == "invalid-status" ? (AttendanceStatus)99 : AttendanceStatus.Present;
        // Act
        Assert.Throws<DomainException>(() => fixture.Enrollment.RecordAttendance(session, status, fixture.Time.LocalNow, fixture.Clock.Timestamp));
        // Assert
        Assert.Empty(fixture.Enrollment.AttendanceRecords);
    }

    [Theory]
    [InlineData("duplicate")]
    [InlineData("before-course")]
    [InlineData("after-course")]
    public void Enroll_WhenEnrollmentIsInvalid_PreservesEnrollmentCount(string scenario)
    {
        // Arrange
        var fixture = new AcademicFixture();
        var student = scenario == "duplicate" ? fixture.Student : new Student("S2", "Luis", "Díaz");
        var date = scenario == "before-course" ? new DateOnly(2026, 7, 31)
            : scenario == "after-course" ? new DateOnly(2026, 12, 21) : fixture.Enrollment.EnrolledOn;
        // Act
        Assert.Throws<DomainException>(() => fixture.Course.Enroll(student, date));
        // Assert
        Assert.Single(fixture.Course.Enrollments);
    }

    [Theory]
    [InlineData("assessment-name")]
    [InlineData("assessment-date")]
    [InlineData("session-time")]
    [InlineData("session-date")]
    [InlineData("session-utc")]
    public void AddCourseEvent_WhenInvalid_PreservesExistingEvents(string scenario)
    {
        // Arrange
        var fixture = new AcademicFixture();
        // Act
        Assert.Throws<DomainException>(() =>
        {
            switch (scenario)
            {
                case "assessment-name": fixture.Course.AddAssessment(" prueba 1 ", fixture.Assessment.Date); break;
                case "assessment-date": fixture.Course.AddAssessment("Other", new DateOnly(2027, 1, 1)); break;
                case "session-time": fixture.Course.AddSession(fixture.Session.StartsAt); break;
                case "session-date": fixture.Course.AddSession(new DateTime(2027, 1, 1, 9, 0, 0)); break;
                case "session-utc": fixture.Course.AddSession(DateTime.SpecifyKind(fixture.Session.StartsAt, DateTimeKind.Utc)); break;
            }
        });
        // Assert
        Assert.Single(fixture.Course.Assessments);
        Assert.Single(fixture.Course.Sessions);
    }

    [Fact]
    public void ChangeValue_WhenTimestampMovesBackwards_PreservesRecord()
    {
        // Arrange
        var fixture = new AcademicFixture();
        var grade = fixture.Enrollment.RegisterGrade(fixture.Assessment, 5m, fixture.Clock.Timestamp);
        // Act
        Assert.Throws<DomainException>(() => grade.ChangeValue(6m, fixture.Clock.Timestamp.AddMinutes(-1)));
        // Assert
        Assert.Equal(5m, grade.Value);
        Assert.Equal(fixture.Clock.Timestamp, grade.UpdatedAt);
    }

    [Fact]
    public void GetSummary_WhenRecordsAreViewedPolymorphically_PreservesSpecializedBehavior()
    {
        // Arrange
        var fixture = new AcademicFixture();
        AcademicRecord grade = fixture.Enrollment.RegisterGrade(fixture.Assessment, 5.4m, fixture.Clock.Timestamp);
        AcademicRecord attendance = fixture.Enrollment.RecordAttendance(fixture.Session, AttendanceStatus.Present, fixture.Time.LocalNow, fixture.Clock.Timestamp);
        // Act
        var summaries = new[] { grade.GetSummary(), attendance.GetSummary() };
        // Assert
        Assert.Equal(new[] { "5.4", "Present" }, summaries);
        Assert.Same(fixture.Enrollment, grade.Enrollment);
        Assert.Same(fixture.Enrollment, attendance.Enrollment);
    }
}
