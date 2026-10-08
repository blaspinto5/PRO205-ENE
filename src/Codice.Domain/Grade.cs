using System.Globalization;

namespace Codice.Domain;

public sealed class Grade : AcademicRecord
{
    public Assessment Assessment { get; private set; } = null!;
    public decimal Value { get; private set; }

    private Grade() { }

    internal Grade(Enrollment enrollment, Assessment assessment, decimal value, DateTimeOffset timestamp)
        : base(enrollment, timestamp)
    {
        ValidateValue(value);
        Assessment = assessment;
        Value = value;
    }

    public void ChangeValue(decimal value, DateTimeOffset timestamp)
    {
        ValidateValue(value);
        MarkUpdated(timestamp);
        Value = value;
    }

    private static void ValidateValue(decimal value)
    {
        DomainRules.Require(value >= 1m && value <= 7m, "Grade must be between 1.0 and 7.0.");
        DomainRules.Require(decimal.Truncate(value * 10m) == value * 10m, "Grade accepts at most one decimal place.");
    }

    public override string GetSummary() => Value.ToString("0.0", CultureInfo.InvariantCulture);
}
