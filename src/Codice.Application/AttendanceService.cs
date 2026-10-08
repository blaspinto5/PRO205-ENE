using Codice.Domain;

namespace Codice.Application;

public sealed class AttendanceService(ICourseRepository repository, AcademicTime time)
{
    public async Task<AttendanceRecord> RecordAttendanceAsync(Guid courseId, Guid enrollmentId,
        Guid sessionId, AttendanceStatus status, CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var enrollment = course.Enrollments.SingleOrDefault(e => e.Id == enrollmentId)
            ?? throw new DomainException("Enrollment does not exist in this course.");
        var session = course.Sessions.SingleOrDefault(s => s.Id == sessionId)
            ?? throw new DomainException("Session does not exist in this course.");
        var record = enrollment.RecordAttendance(session, status, time.LocalNow, time.UtcNow);
        await repository.SaveAsync(course, cancellationToken);
        return record;
    }

    public async Task<AttendanceRecord> UpdateAttendanceAsync(Guid courseId, Guid recordId,
        AttendanceStatus status, CancellationToken cancellationToken = default)
    {
        var course = await GetCourseAsync(courseId, cancellationToken);
        var record = course.Enrollments.SelectMany(e => e.AttendanceRecords).SingleOrDefault(a => a.Id == recordId)
            ?? throw new DomainException("Attendance does not exist in this course.");
        record.ChangeStatus(status, time.UtcNow);
        await repository.SaveAsync(course, cancellationToken);
        return record;
    }

    private async Task<Course> GetCourseAsync(Guid id, CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(cancellationToken)).SingleOrDefault(c => c.Id == id)
        ?? throw new DomainException("Course does not exist.");
}
