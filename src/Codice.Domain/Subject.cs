namespace Codice.Domain;

public sealed class Subject
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Subject() { }

    public Subject(string code, string name)
    {
        Code = DomainRules.NormalizeCode(code, nameof(Code));
        Name = DomainRules.RequiredText(name, nameof(Name));
    }

    public void UpdateName(string name) => Name = DomainRules.RequiredText(name, nameof(Name));
}
