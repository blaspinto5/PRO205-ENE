namespace Codice.Domain;

public sealed class Assessment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Course Course { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public DateOnly Date { get; private set; }

    private Assessment() { }

    internal Assessment(Course course, string name, DateOnly date) =>
        (Course, Name, Date) = (course, name, date);

    public string GetSummary() => Name;
}
