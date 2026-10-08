using Codice.Application;
using Codice.Domain;

namespace Codice.UnitTests.Support;

internal sealed class FixedTimeProvider(DateTimeOffset timestamp) : TimeProvider
{
    public DateTimeOffset Timestamp { get; set; } = timestamp;
    public override DateTimeOffset GetUtcNow() => Timestamp;
}

internal sealed class MemoryRepositories : IStudentRepository, ISubjectRepository, ICourseRepository
{
    public List<Student> Students { get; } = [];
    public List<Subject> Subjects { get; } = [];
    public List<Course> Courses { get; } = [];
    public int Saves { get; private set; }

    Task<IReadOnlyList<Student>> IStudentRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Student>>(Students.AsReadOnly());
    Task<IReadOnlyList<Subject>> ISubjectRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Subject>>(Subjects.AsReadOnly());
    Task<IReadOnlyList<Course>> ICourseRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Course>>(Courses.AsReadOnly());

    public Task SaveAsync(Student student, CancellationToken cancellationToken)
    {
        if (Students.All(s => s.Id != student.Id)) Students.Add(student);
        Saves++;
        return Task.CompletedTask;
    }

    public Task SaveAsync(Subject subject, CancellationToken cancellationToken)
    {
        if (Subjects.All(s => s.Id != subject.Id)) Subjects.Add(subject);
        Saves++;
        return Task.CompletedTask;
    }

    public Task SaveAsync(Course course, CancellationToken cancellationToken)
    {
        if (Courses.All(c => c.Id != course.Id)) Courses.Add(course);
        Saves++;
        return Task.CompletedTask;
    }
}

internal sealed class AcademicFixture
{
    public MemoryRepositories Store { get; } = new();
    public FixedTimeProvider Clock { get; } = new(new DateTimeOffset(2026, 10, 6, 15, 0, 0, TimeSpan.Zero));
    public AcademicTime Time { get; }
    public Course Course { get; }
    public Student Student { get; } = new(" s001 ", "Ana", "Pérez");
    public Enrollment Enrollment { get; }
    public Assessment Assessment { get; }
    public ClassSession Session { get; }
    public GradeService Grades { get; }
    public AttendanceService Attendance { get; }
    public CourseService Courses { get; }
    public ReportService Reports { get; }

    public AcademicFixture()
    {
        Time = new AcademicTime(Clock, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago"));
        var subject = new Subject("PRO205", "Programación");
        Course = new Course(subject, "A", "2026-2", new DateOnly(2026, 8, 1), new DateOnly(2026, 12, 20));
        Store.Subjects.Add(subject);
        Store.Courses.Add(Course);
        Store.Students.Add(Student);
        Enrollment = Course.Enroll(Student, new DateOnly(2026, 9, 1));
        Assessment = Course.AddAssessment("Prueba 1", new DateOnly(2026, 10, 1));
        Session = Course.AddSession(new DateTime(2026, 10, 5, 9, 0, 0));
        Grades = new GradeService(Store, Time);
        Attendance = new AttendanceService(Store, Time);
        Courses = new CourseService(Store, Store, Store, Time);
        Reports = new ReportService(Store, Store, Time);
    }
}
