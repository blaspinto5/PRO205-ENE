namespace Codice.Domain;

internal static class DomainRules
{
    public static string RequiredText(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{field} is required.");
        return value.Trim();
    }

    public static string NormalizeCode(string value, string field) =>
        RequiredText(value, field).ToUpperInvariant();

    public static void Require(bool condition, string message)
    {
        if (!condition) throw new DomainException(message);
    }
}
