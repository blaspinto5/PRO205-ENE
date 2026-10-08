using Codice.Domain;

namespace Codice.Application;

public sealed record EvaluationReport(Guid AssessmentId, string Name, DateOnly Date, decimal? Value);
public sealed record EnrollmentReport(Guid StudentId, string StudentCode, string FirstName, string LastName,
    Guid CourseId, string SubjectName, string Section, string Period, IReadOnlyList<EvaluationReport> Evaluations,
    decimal? Average, bool IsProvisional, bool? IsAtOrAboveReference, AttendanceCounts Attendance);
public sealed record StudentReport(Guid StudentId, string StudentCode, string FirstName, string LastName,
    IReadOnlyList<EnrollmentReport> Courses);
public sealed record CourseReport(Guid CourseId, string SubjectName, string Section, string Period,
    IReadOnlyList<EnrollmentReport> Students);

public sealed class ReportService(ICourseRepository courses, IStudentRepository students, AcademicTime time)
{
    public async Task<StudentReport> GetStudentReportAsync(Guid studentId, DateTime cutoff, Guid? courseId = null,
        CancellationToken cancellationToken = default)
    {
        var student = (await students.GetAllAsync(cancellationToken)).SingleOrDefault(s => s.Id == studentId)
            ?? throw new DomainException("Student does not exist.");
        var allCourses = await courses.GetAllAsync(cancellationToken);
        if (courseId.HasValue && allCourses.All(c => c.Id != courseId)) throw new DomainException("Course does not exist.");
        var effectiveCutoff = GetCutoff(cutoff);
        var rows = allCourses.Where(c => !courseId.HasValue || c.Id == courseId)
            .SelectMany(c => c.Enrollments).Where(e => e.Student.Id == studentId)
            .Select(e => CreateRow(e, effectiveCutoff)).ToArray();
        return new StudentReport(student.Id, student.StudentCode, student.FirstName, student.LastName, rows);
    }

    public async Task<CourseReport> GetCourseReportAsync(Guid courseId, DateTime cutoff,
        CancellationToken cancellationToken = default)
    {
        var course = (await courses.GetAllAsync(cancellationToken)).SingleOrDefault(c => c.Id == courseId)
            ?? throw new DomainException("Course does not exist.");
        var effectiveCutoff = GetCutoff(cutoff);
        return new CourseReport(course.Id, course.Subject.Name, course.SectionCode, course.Period,
            course.Enrollments.OrderBy(e => e.Student.StudentCode).Select(e => CreateRow(e, effectiveCutoff)).ToArray());
    }

    private DateTime GetCutoff(DateTime cutoff)
    {
        if (!time.IsValidLocalTime(cutoff)) throw new DomainException("Report cutoff is invalid or ambiguous.");
        return cutoff < time.LocalNow ? cutoff : time.LocalNow;
    }

    private static EnrollmentReport CreateRow(Enrollment enrollment, DateTime cutoff)
    {
        var evaluations = enrollment.Course.Assessments.Where(a => a.Date >= enrollment.EnrolledOn)
            .OrderBy(a => a.Date).ThenBy(a => a.Name)
            .Select(a => new EvaluationReport(a.Id, a.Name, a.Date,
                enrollment.Grades.SingleOrDefault(g => g.Assessment.Id == a.Id)?.Value)).ToArray();
        var average = enrollment.CalculateAverage();
        return new EnrollmentReport(enrollment.Student.Id, enrollment.Student.StudentCode,
            enrollment.Student.FirstName, enrollment.Student.LastName, enrollment.Course.Id,
            enrollment.Course.Subject.Name, enrollment.Course.SectionCode, enrollment.Course.Period,
            Array.AsReadOnly(evaluations), average, evaluations.Any(e => !e.Value.HasValue),
            average.HasValue ? average >= 4m : null, enrollment.GetAttendanceCounts(cutoff));
    }
}
