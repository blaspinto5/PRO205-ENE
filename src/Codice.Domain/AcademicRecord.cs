namespace Codice.Domain;

public abstract class AcademicRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Enrollment Enrollment { get; private set; } = null!;
    public DateTimeOffset RecordedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    protected AcademicRecord() { }

    protected AcademicRecord(Enrollment enrollment, DateTimeOffset recordedAt)
    {
        Enrollment = enrollment;
        RecordedAt = recordedAt.ToUniversalTime();
        UpdatedAt = RecordedAt;
    }

    protected void MarkUpdated(DateTimeOffset timestamp)
    {
        DomainRules.Require(timestamp >= UpdatedAt, "Modification cannot precede the last recorded change.");
        UpdatedAt = timestamp.ToUniversalTime();
    }

    public abstract string GetSummary();
}
