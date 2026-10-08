using Codice.Application;
using Codice.Infrastructure;

namespace Codice.Desktop;

public sealed class Workspace
{
    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly WorkspaceQueryService query;
    public WorkspaceSnapshot Snapshot { get; private set; } = new([], [], []);
    public StudentService Students { get; }
    public SubjectService Subjects { get; }
    public CourseService Courses { get; }
    public GradeService Grades { get; }
    public AttendanceService Attendance { get; }
    public ReportService Reports { get; }
    public AcademicTime Time { get; }

    public Workspace(SqliteRepositories repositories)
    {
        Time = new AcademicTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago"));
        query = new WorkspaceQueryService(repositories, repositories, repositories);
        Students = new StudentService(repositories);
        Subjects = new SubjectService(repositories);
        Courses = new CourseService(repositories, repositories, repositories, Time);
        Grades = new GradeService(repositories, Time);
        Attendance = new AttendanceService(repositories, Time);
        Reports = new ReportService(repositories, repositories, Time);
    }

    public async Task RefreshAsync() => await RunAsync(() => Task.CompletedTask);

    public async Task RunAsync(Func<Task> action)
    {
        await gate.WaitAsync();
        try { await action(); Snapshot = await query.GetAsync(); }
        finally { gate.Release(); }
    }
}
