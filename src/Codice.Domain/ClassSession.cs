namespace Codice.Domain;

public sealed class ClassSession
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Course Course { get; private set; } = null!;
    public DateTime StartsAt { get; private set; }

    private ClassSession() { }

    internal ClassSession(Course course, DateTime startsAt) => (Course, StartsAt) = (course, startsAt);

    public DateOnly GetDate() => DateOnly.FromDateTime(StartsAt);
}
