using Codice.Domain;

namespace Codice.Application;

public sealed class CourseService(ICourseRepository courses, ISubjectRepository subjects,
    IStudentRepository students, AcademicTime time)
{
    public async Task<Course> CreateCourseAsync(Guid subjectId, string section, string period,
        DateOnly start, DateOnly end, CancellationToken cancellationToken = default)
    {
        var subject = (await subjects.GetAllAsync(cancellationToken)).SingleOrDefault(s => s.Id == subjectId)
            ?? throw new DomainException("Subject does not exist.");
        var course = new Course(subject, section, period, start, end);
        if ((await courses.GetAllAsync(cancellationToken)).Any(c => c.Subject.Id == subjectId &&
            c.SectionCode == course.SectionCode && c.Period == course.Period))
            throw new DomainException("Course already exists for this subject, period and section.");
        await courses.SaveAsync(course, cancellationToken);
        return course;
    }

    public async Task<Enrollment> EnrollStudentAsync(Guid courseId, Guid studentId, DateOnly date,
        CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var student = (await students.GetAllAsync(cancellationToken)).SingleOrDefault(s => s.Id == studentId)
            ?? throw new DomainException("Student does not exist.");
        var enrollment = course.Enroll(student, date);
        await courses.SaveAsync(course, cancellationToken);
        return enrollment;
    }

    public async Task<Assessment> CreateAssessmentAsync(Guid courseId, string name, DateOnly date,
        CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var assessment = course.AddAssessment(name, date);
        await courses.SaveAsync(course, cancellationToken);
        return assessment;
    }

    public async Task<ClassSession> CreateSessionAsync(Guid courseId, DateTime start,
        CancellationToken cancellationToken = default)
    {
        if (!time.IsValidLocalTime(start)) throw new DomainException("Session time is invalid or ambiguous in the academic time zone.");
        var course = await GetCourseAsync(courseId, cancellationToken);
        var session = course.AddSession(start);
        await courses.SaveAsync(course, cancellationToken);
        return session;
    }

    private async Task<Course> GetCourseAsync(Guid id, CancellationToken cancellationToken) =>
        (await courses.GetAllAsync(cancellationToken)).SingleOrDefault(c => c.Id == id)
        ?? throw new DomainException("Course does not exist.");
}
