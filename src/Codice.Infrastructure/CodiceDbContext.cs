using Codice.Domain;
using Microsoft.EntityFrameworkCore;

namespace Codice.Infrastructure;

public sealed class CodiceDbContext(DbContextOptions<CodiceDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder model)
    {
        var student = model.Entity<Student>();
        student.HasKey(x => x.Id);
        student.Property(x => x.Id).ValueGeneratedNever();
        student.HasIndex(x => x.StudentCode).IsUnique();
        student.Property(x => x.StudentCode).IsRequired();
        student.Property(x => x.FirstName).IsRequired();
        student.Property(x => x.LastName).IsRequired();

        var subject = model.Entity<Subject>();
        subject.HasKey(x => x.Id);
        subject.Property(x => x.Id).ValueGeneratedNever();
        subject.HasIndex(x => x.Code).IsUnique();
        subject.Property(x => x.Code).IsRequired();
        subject.Property(x => x.Name).IsRequired();

        var course = model.Entity<Course>();
        course.HasKey(x => x.Id);
        course.Property(x => x.Id).ValueGeneratedNever();
        course.HasOne(x => x.Subject).WithMany().HasForeignKey("SubjectId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        course.HasIndex("SubjectId", nameof(Course.Period), nameof(Course.SectionCode)).IsUnique();
        course.HasMany(x => x.Enrollments).WithOne(x => x.Course).HasForeignKey("CourseId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        course.HasMany(x => x.Assessments).WithOne(x => x.Course).HasForeignKey("CourseId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        course.HasMany(x => x.Sessions).WithOne(x => x.Course).HasForeignKey("CourseId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        course.Navigation(x => x.Enrollments).HasField("enrollments").UsePropertyAccessMode(PropertyAccessMode.Field);
        course.Navigation(x => x.Assessments).HasField("assessments").UsePropertyAccessMode(PropertyAccessMode.Field);
        course.Navigation(x => x.Sessions).HasField("sessions").UsePropertyAccessMode(PropertyAccessMode.Field);
        course.ToTable("Courses", t => t.HasCheckConstraint("CK_Course_Dates", "StartDate <= EndDate"));

        var enrollment = model.Entity<Enrollment>();
        enrollment.HasKey(x => x.Id);
        enrollment.Property(x => x.Id).ValueGeneratedNever();
        enrollment.HasOne(x => x.Student).WithMany().HasForeignKey("StudentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        enrollment.HasIndex("CourseId", "StudentId").IsUnique();
        enrollment.HasMany(x => x.Grades).WithOne(x => x.Enrollment).HasForeignKey("EnrollmentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        enrollment.HasMany(x => x.AttendanceRecords).WithOne(x => x.Enrollment).HasForeignKey("EnrollmentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        enrollment.Navigation(x => x.Grades).HasField("grades").UsePropertyAccessMode(PropertyAccessMode.Field);
        enrollment.Navigation(x => x.AttendanceRecords).HasField("attendanceRecords").UsePropertyAccessMode(PropertyAccessMode.Field);

        var assessment = model.Entity<Assessment>();
        assessment.HasKey(x => x.Id);
        assessment.Property(x => x.Id).ValueGeneratedNever();
        assessment.Property<string>("NormalizedName").IsRequired();
        assessment.HasIndex("CourseId", "NormalizedName").IsUnique();
        var session = model.Entity<ClassSession>();
        session.HasKey(x => x.Id);
        session.Property(x => x.Id).ValueGeneratedNever();
        session.HasIndex("CourseId", nameof(ClassSession.StartsAt)).IsUnique();

        var record = model.Entity<AcademicRecord>();
        record.UseTpcMappingStrategy();
        record.HasKey(x => x.Id);
        record.Property(x => x.Id).ValueGeneratedNever();
        var grade = model.Entity<Grade>();
        grade.HasOne(x => x.Assessment).WithMany().HasForeignKey("AssessmentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        grade.HasIndex("EnrollmentId", "AssessmentId").IsUnique();
        grade.Property(x => x.Value).HasColumnName("ValueTenths").HasConversion(x => (int)(x * 10m), x => x / 10m);
        grade.ToTable("Grades", t => t.HasCheckConstraint("CK_Grade_Value", "ValueTenths BETWEEN 10 AND 70"));
        var attendance = model.Entity<AttendanceRecord>();
        attendance.HasOne(x => x.Session).WithMany().HasForeignKey("SessionId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        attendance.HasIndex("EnrollmentId", "SessionId").IsUnique();
        attendance.ToTable("Attendance", t => t.HasCheckConstraint("CK_Attendance_Status", "Status IN (0, 1)"));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Assessment>().Where(e => e.State is EntityState.Added or EntityState.Modified))
            entry.Property("NormalizedName").CurrentValue = entry.Entity.Name.ToUpperInvariant();
        return base.SaveChangesAsync(cancellationToken);
    }
}
