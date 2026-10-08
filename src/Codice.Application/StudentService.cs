using Codice.Domain;

namespace Codice.Application;

public sealed class StudentService(IStudentRepository repository)
{
    public async Task<Student> RegisterStudentAsync(string code, string firstName, string lastName,
        CancellationToken cancellationToken = default)
    {
        var student = new Student(code, firstName, lastName);
        var students = await repository.GetAllAsync(cancellationToken);
        if (students.Any(s => s.StudentCode == student.StudentCode))
            throw new DomainException("Student code already exists.");
        await repository.SaveAsync(student, cancellationToken);
        return student;
    }

    public async Task<Student> UpdateStudentAsync(Guid id, string firstName, string lastName,
        CancellationToken cancellationToken = default)
    {
        var student = (await repository.GetAllAsync(cancellationToken)).SingleOrDefault(s => s.Id == id)
            ?? throw new DomainException("Student does not exist.");
        student.UpdateName(firstName, lastName);
        await repository.SaveAsync(student, cancellationToken);
        return student;
    }
}
