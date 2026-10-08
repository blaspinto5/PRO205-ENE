using Codice.Domain;

namespace Codice.Application;

public sealed class SubjectService(ISubjectRepository repository)
{
    public async Task<Subject> RegisterSubjectAsync(string code, string name, CancellationToken cancellationToken = default)
    {
        var subject = new Subject(code, name);
        if ((await repository.GetAllAsync(cancellationToken)).Any(s => s.Code == subject.Code))
            throw new DomainException("Subject code already exists.");
        await repository.SaveAsync(subject, cancellationToken);
        return subject;
    }

    public async Task<Subject> UpdateSubjectAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        var subject = (await repository.GetAllAsync(cancellationToken)).SingleOrDefault(s => s.Id == id)
            ?? throw new DomainException("Subject does not exist.");
        subject.UpdateName(name);
        await repository.SaveAsync(subject, cancellationToken);
        return subject;
    }
}
