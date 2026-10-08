namespace Codice.Domain;

public sealed class Course
{
    private readonly List<Enrollment> enrollments = [];
    private readonly List<Assessment> assessments = [];
    private readonly List<ClassSession> sessions = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Subject Subject { get; private set; } = null!;
    public string SectionCode { get; private set; } = null!;
    public string Period { get; private set; } = null!;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public IReadOnlyList<Enrollment> Enrollments => enrollments.AsReadOnly();
    public IReadOnlyList<Assessment> Assessments => assessments.AsReadOnly();
    public IReadOnlyList<ClassSession> Sessions => sessions.AsReadOnly();

    private Course() { }

    public Course(Subject subject, string sectionCode, string period, DateOnly startDate, DateOnly endDate)
    {
        ArgumentNullException.ThrowIfNull(subject);
        DomainRules.Require(startDate <= endDate, "Course dates are reversed.");
        Subject = subject;
        SectionCode = DomainRules.NormalizeCode(sectionCode, nameof(SectionCode));
        Period = DomainRules.NormalizeCode(period, nameof(Period));
        StartDate = startDate;
        EndDate = endDate;
    }

    public Enrollment Enroll(Student student, DateOnly date)
    {
        ArgumentNullException.ThrowIfNull(student);
        RequireDate(date);
        DomainRules.Require(enrollments.All(e => e.Student.Id != student.Id), "Student is already enrolled.");
        var enrollment = new Enrollment(student, this, date);
        enrollments.Add(enrollment);
        return enrollment;
    }

    public Assessment AddAssessment(string name, DateOnly date)
    {
        RequireDate(date);
        var validName = DomainRules.RequiredText(name, nameof(name));
        DomainRules.Require(assessments.All(a => !string.Equals(a.Name, validName, StringComparison.OrdinalIgnoreCase)),
            "Assessment name already exists in this course.");
        var assessment = new Assessment(this, validName, date);
        assessments.Add(assessment);
        return assessment;
    }

    public ClassSession AddSession(DateTime start)
    {
        DomainRules.Require(start.Kind == DateTimeKind.Unspecified, "Session time must be a local academic time.");
        RequireDate(DateOnly.FromDateTime(start));
        DomainRules.Require(sessions.All(s => s.StartsAt != start), "Session already exists.");
        var session = new ClassSession(this, start);
        sessions.Add(session);
        return session;
    }

    private void RequireDate(DateOnly date) =>
        DomainRules.Require(date >= StartDate && date <= EndDate, "Date must be inside the course period.");
}
