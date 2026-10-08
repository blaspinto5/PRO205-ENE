namespace Codice.Domain;

public enum AttendanceStatus { Present, Absent }

public sealed class AttendanceRecord : AcademicRecord
{
    public ClassSession Session { get; private set; } = null!;
    public AttendanceStatus Status { get; private set; }

    private AttendanceRecord() { }

    internal AttendanceRecord(Enrollment enrollment, ClassSession session, AttendanceStatus status, DateTimeOffset timestamp)
        : base(enrollment, timestamp)
    {
        ValidateStatus(status);
        Session = session;
        Status = status;
    }

    public void ChangeStatus(AttendanceStatus status, DateTimeOffset timestamp)
    {
        ValidateStatus(status);
        MarkUpdated(timestamp);
        Status = status;
    }

    private static void ValidateStatus(AttendanceStatus status) =>
        DomainRules.Require(Enum.IsDefined(status), "Attendance status is invalid.");

    public override string GetSummary() => Status.ToString();
}
