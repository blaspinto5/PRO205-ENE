namespace Codice.Domain;

public sealed record AttendanceCounts(int Present, int Absent, int Unrecorded);

public sealed class Enrollment
{
    private readonly List<Grade> grades = [];
    private readonly List<AttendanceRecord> attendanceRecords = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Student Student { get; private set; } = null!;
    public Course Course { get; private set; } = null!;
    public DateOnly EnrolledOn { get; private set; }
    public IReadOnlyList<Grade> Grades => grades.AsReadOnly();
    public IReadOnlyList<AttendanceRecord> AttendanceRecords => attendanceRecords.AsReadOnly();

    private Enrollment() { }

    internal Enrollment(Student student, Course course, DateOnly enrolledOn) =>
        (Student, Course, EnrolledOn) = (student, course, enrolledOn);

    public Grade RegisterGrade(Assessment assessment, decimal value, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(assessment);
        DomainRules.Require(assessment.Course.Id == Course.Id, "Assessment belongs to another course.");
        DomainRules.Require(assessment.Date >= EnrolledOn, "Assessment precedes enrollment.");
        DomainRules.Require(grades.All(g => g.Assessment.Id != assessment.Id), "Grade already exists for this assessment.");
        var grade = new Grade(this, assessment, value, timestamp);
        grades.Add(grade);
        return grade;
    }

    public AttendanceRecord RecordAttendance(ClassSession session, AttendanceStatus status,
        DateTime localNow, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(session);
        DomainRules.Require(localNow.Kind == DateTimeKind.Unspecified, "Reference time must be local academic time.");
        DomainRules.Require(session.Course.Id == Course.Id, "Session belongs to another course.");
        DomainRules.Require(session.GetDate() >= EnrolledOn, "Session precedes enrollment.");
        DomainRules.Require(session.StartsAt <= localNow, "Cannot record attendance for a future session.");
        DomainRules.Require(attendanceRecords.All(a => a.Session.Id != session.Id), "Attendance already exists for this session.");
        var record = new AttendanceRecord(this, session, status, timestamp);
        attendanceRecords.Add(record);
        return record;
    }

    public decimal? CalculateAverage() => grades.Count == 0 ? null :
        decimal.Round(grades.Average(g => g.Value), 1, MidpointRounding.AwayFromZero);

    public AttendanceCounts GetAttendanceCounts(DateTime cutoff)
    {
        DomainRules.Require(cutoff.Kind == DateTimeKind.Unspecified, "Cutoff must be local academic time.");
        var eligibleIds = Course.Sessions.Where(s => s.GetDate() >= EnrolledOn && s.StartsAt <= cutoff)
            .Select(s => s.Id).ToHashSet();
        var records = attendanceRecords.Where(a => eligibleIds.Contains(a.Session.Id)).ToArray();
        return new AttendanceCounts(records.Count(a => a.Status == AttendanceStatus.Present),
            records.Count(a => a.Status == AttendanceStatus.Absent), eligibleIds.Count - records.Length);
    }
}
