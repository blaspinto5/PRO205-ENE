using Codice.Domain;

namespace Codice.Application;

public sealed class GradeService(ICourseRepository repository, AcademicTime time)
{
    public async Task<Grade> RegisterGradeAsync(Guid courseId, Guid enrollmentId, Guid assessmentId, decimal value,
        CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var enrollment = course.Enrollments.SingleOrDefault(e => e.Id == enrollmentId)
            ?? throw new DomainException("Enrollment does not exist in this course.");
        var assessment = course.Assessments.SingleOrDefault(a => a.Id == assessmentId)
            ?? throw new DomainException("Assessment does not exist in this course.");
        var grade = enrollment.RegisterGrade(assessment, value, time.UtcNow);
        await repository.SaveAsync(course, cancellationToken);
        return grade;
    }

    public async Task<Grade> UpdateGradeAsync(Guid courseId, Guid gradeId, decimal value,
        CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var grade = course.Enrollments.SelectMany(e => e.Grades).SingleOrDefault(g => g.Id == gradeId)
            ?? throw new DomainException("Grade does not exist in this course.");
        grade.ChangeValue(value, time.UtcNow);
        await repository.SaveAsync(course, cancellationToken);
        return grade;
    }

    private async Task<Course> GetCourseAsync(Guid id, CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(cancellationToken)).SingleOrDefault(c => c.Id == id)
        ?? throw new DomainException("Course does not exist.");
}
