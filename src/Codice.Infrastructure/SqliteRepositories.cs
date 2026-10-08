using Codice.Application;
using Codice.Domain;
using Microsoft.EntityFrameworkCore;

namespace Codice.Infrastructure;

public sealed class SqliteRepositories(PortableStorage storage) : IStudentRepository, ISubjectRepository, ICourseRepository
{
    async Task<IReadOnlyList<Student>> IStudentRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        await using var db = storage.CreateContext();
        return await db.Set<Student>().AsNoTracking().OrderBy(x => x.StudentCode).ToListAsync(cancellationToken);
    }

    async Task<IReadOnlyList<Subject>> ISubjectRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        await using var db = storage.CreateContext();
        return await db.Set<Subject>().AsNoTracking().OrderBy(x => x.Code).ToListAsync(cancellationToken);
    }

    async Task<IReadOnlyList<Course>> ICourseRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        await using var db = storage.CreateContext();
        return await db.Set<Course>().AsNoTrackingWithIdentityResolution().AsSplitQuery()
            .Include(x => x.Subject).Include(x => x.Assessments).Include(x => x.Sessions)
            .Include(x => x.Enrollments).ThenInclude(x => x.Student)
            .Include(x => x.Enrollments).ThenInclude(x => x.Grades).ThenInclude(x => x.Assessment)
            .Include(x => x.Enrollments).ThenInclude(x => x.AttendanceRecords).ThenInclude(x => x.Session)
            .OrderBy(x => x.Period).ThenBy(x => x.SectionCode).ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(Student student, CancellationToken cancellationToken = default)
    {
        await using var db = storage.CreateContext();
        db.Entry(student).State = await db.Set<Student>().AnyAsync(x => x.Id == student.Id, cancellationToken)
            ? EntityState.Modified : EntityState.Added;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        await using var db = storage.CreateContext();
        db.Entry(subject).State = await db.Set<Subject>().AnyAsync(x => x.Id == subject.Id, cancellationToken)
            ? EntityState.Modified : EntityState.Added;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(Course course, CancellationToken cancellationToken = default)
    {
        await using var db = storage.CreateContext();
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        var existing = new HashSet<Guid>();
        existing.UnionWith(await db.Set<Course>().Select(x => x.Id).ToListAsync(cancellationToken));
        existing.UnionWith(await db.Set<Enrollment>().Select(x => x.Id).ToListAsync(cancellationToken));
        existing.UnionWith(await db.Set<Assessment>().Select(x => x.Id).ToListAsync(cancellationToken));
        existing.UnionWith(await db.Set<ClassSession>().Select(x => x.Id).ToListAsync(cancellationToken));
        existing.UnionWith(await db.Set<AcademicRecord>().Select(x => x.Id).ToListAsync(cancellationToken));
        db.ChangeTracker.TrackGraph(course, node =>
        {
            var id = (Guid)node.Entry.Property("Id").CurrentValue!;
            node.Entry.State = node.Entry.Entity is Student or Subject ? EntityState.Unchanged :
                existing.Contains(id) ? EntityState.Modified : EntityState.Added;
        });
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
