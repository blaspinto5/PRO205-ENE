namespace Codice.Application;

public sealed record CourseSummary(Guid Id, string Subject, string Section, string Period, int Students);
public sealed record CatalogSummary(int Students, int Subjects, IReadOnlyList<CourseSummary> Courses);

public sealed class CatalogService(IStudentRepository students, ISubjectRepository subjects, ICourseRepository courses)
{
    public async Task<CatalogSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var studentList = await students.GetAllAsync(cancellationToken);
        var subjectList = await subjects.GetAllAsync(cancellationToken);
        var courseList = await courses.GetAllAsync(cancellationToken);
        var rows = courseList.Select(x => new CourseSummary(x.Id, x.Subject.Name, x.SectionCode, x.Period, x.Enrollments.Count)).ToArray();
        return new CatalogSummary(studentList.Count, subjectList.Count, Array.AsReadOnly(rows));
    }
}
