using Codice.Domain;

namespace Codice.Application;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Student student, CancellationToken cancellationToken = default);
}

public interface ISubjectRepository
{
    Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Subject subject, CancellationToken cancellationToken = default);
}

public interface ICourseRepository
{
    Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Course course, CancellationToken cancellationToken = default);
}
