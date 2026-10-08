namespace Codice.Application;

public sealed class AcademicTime(TimeProvider provider, TimeZoneInfo timeZone)
{
    public DateTimeOffset UtcNow => provider.GetUtcNow();
    public DateTime LocalNow => TimeZoneInfo.ConvertTime(UtcNow, timeZone).DateTime;

    public bool IsValidLocalTime(DateTime value) =>
        value.Kind == DateTimeKind.Unspecified && !timeZone.IsInvalidTime(value) && !timeZone.IsAmbiguousTime(value);
}
