using Codice.Domain;

namespace Codice.Application;

public sealed record StudentItem(Guid Id, string Code, string FirstName, string LastName)
{
    public string Display => $"{Code} · {FirstName} {LastName}";
}
public sealed record SubjectItem(Guid Id, string Code, string Name)
{
    public string Display => $"{Code} · {Name}";
}
public sealed record AssessmentItem(Guid Id, string Name, DateOnly Date);
public sealed record SessionItem(Guid Id, DateTime StartsAt)
{
    public string Display => StartsAt.ToString("dd/MM/yyyy HH:mm");
}
public sealed record GradeItem(Guid Id, Guid AssessmentId, decimal Value);
public sealed record AttendanceItem(Guid Id, Guid SessionId, AttendanceStatus Status);
public sealed record EnrollmentItem(Guid Id, Guid StudentId, string Student,
    IReadOnlyList<GradeItem> Grades, IReadOnlyList<AttendanceItem> Attendance);
public sealed record CourseItem(Guid Id, Guid SubjectId, string Subject, string Section, string Period,
    DateOnly Start, DateOnly End, IReadOnlyList<EnrollmentItem> Enrollments,
    IReadOnlyList<AssessmentItem> Assessments, IReadOnlyList<SessionItem> Sessions)
{
    public string Display => $"{Subject} · {Section} · {Period}";
    public int Students => Enrollments.Count;
}
public sealed record WorkspaceSnapshot(IReadOnlyList<StudentItem> Students, IReadOnlyList<SubjectItem> Subjects,
    IReadOnlyList<CourseItem> Courses);

public sealed class WorkspaceQueryService(IStudentRepository students, ISubjectRepository subjects, ICourseRepository courses)
{
    public async Task<WorkspaceSnapshot> GetAsync(CancellationToken cancellationToken = default)
    {
        var studentList = await students.GetAllAsync(cancellationToken);
        var subjectList = await subjects.GetAllAsync(cancellationToken);
        var courseList = await courses.GetAllAsync(cancellationToken);
        return new WorkspaceSnapshot(
            studentList.Select(s => new StudentItem(s.Id, s.StudentCode, s.FirstName, s.LastName)).ToArray(),
            subjectList.Select(s => new SubjectItem(s.Id, s.Code, s.Name)).ToArray(),
            courseList.Select(c => new CourseItem(c.Id, c.Subject.Id, c.Subject.Name, c.SectionCode, c.Period, c.StartDate, c.EndDate,
                c.Enrollments.OrderBy(e => e.Student.StudentCode).Select(e => new EnrollmentItem(e.Id, e.Student.Id,
                    $"{e.Student.StudentCode} · {e.Student.FirstName} {e.Student.LastName}",
                    e.Grades.Select(g => new GradeItem(g.Id, g.Assessment.Id, g.Value)).ToArray(),
                    e.AttendanceRecords.Select(a => new AttendanceItem(a.Id, a.Session.Id, a.Status)).ToArray())).ToArray(),
                c.Assessments.Select(a => new AssessmentItem(a.Id, a.Name, a.Date)).OrderBy(a => a.Date).ToArray(),
                c.Sessions.Select(s => new SessionItem(s.Id, s.StartsAt)).OrderBy(s => s.StartsAt).ToArray())).ToArray());
    }
}
