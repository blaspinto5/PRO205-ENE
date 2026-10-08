namespace Codice.Domain;

public sealed class Student
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string StudentCode { get; private set; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private Student() { }

    public Student(string studentCode, string firstName, string lastName)
    {
        StudentCode = DomainRules.NormalizeCode(studentCode, nameof(StudentCode));
        UpdateName(firstName, lastName);
    }

    public void UpdateName(string firstName, string lastName)
    {
        var validFirstName = DomainRules.RequiredText(firstName, nameof(FirstName));
        var validLastName = DomainRules.RequiredText(lastName, nameof(LastName));
        FirstName = validFirstName;
        LastName = validLastName;
    }
}
