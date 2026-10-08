using System.Windows.Input;
using Codice.Application;
using Codice.Domain;

namespace Codice.Desktop.ViewModels;

public sealed record AttendanceRow(EnrollmentItem Enrollment, string Student, string Status);

public sealed class AttendanceViewModel : ScreenViewModel
{
    private CourseItem? course;
    private SessionItem? session;
    private EnrollmentItem? enrollment;
    private AttendanceRow? selectedRow;
    private string status = "Presente";
    public override string Title => "Asistencia";
    public override string Description => "Consulta cada sesión y distingue presentes, ausentes y registros pendientes.";
    public IReadOnlyList<CourseItem> Courses => Workspace.Snapshot.Courses;
    public IReadOnlyList<SessionItem> Sessions => Course?.Sessions ?? [];
    public IReadOnlyList<EnrollmentItem> Enrollments => Course?.Enrollments ?? [];
    public IReadOnlyList<string> Statuses { get; } = ["Presente", "Ausente"];
    public CourseItem? Course { get => course; set { if (Set(ref course, value)) { Session = null; Enrollment = null; } Notify(nameof(Sessions)); Notify(nameof(Enrollments)); Notify(nameof(Rows)); } }
    public SessionItem? Session { get => session; set { Set(ref session, value); SyncStatus(); Notify(nameof(Rows)); } }
    public EnrollmentItem? Enrollment { get => enrollment; set { Set(ref enrollment, value); SyncStatus(); } }
    public AttendanceRow? SelectedRow { get => selectedRow; set { if (Set(ref selectedRow, value) && value is not null) Enrollment = value.Enrollment; } }
    public string Status { get => status; set => Set(ref status, value); }
    public IReadOnlyList<AttendanceRow> Rows => Session is null ? [] : Enrollments.Select(e =>
        new AttendanceRow(e, e.Student, e.Attendance.SingleOrDefault(a => a.SessionId == Session.Id) is { } record
            ? record.Status == AttendanceStatus.Present ? "Presente" : "Ausente" : "Sin registrar")).ToArray();
    public ICommand RegisterCommand { get; }
    public ICommand CorrectCommand { get; }

    public AttendanceViewModel(Workspace workspace) : base(workspace)
    {
        RegisterCommand = new AsyncCommand(() => ExecuteAsync(async () =>
            await Workspace.Attendance.RecordAttendanceAsync(Required(Course, "un curso").Id,
                Required(Enrollment, "un estudiante matriculado").Id, Required(Session, "una sesión").Id,
                SelectedStatus()), "Asistencia registrada correctamente."));
        CorrectCommand = new AsyncCommand(() => ExecuteAsync(async () =>
        {
            var sessionId = Required(Session, "una sesión").Id;
            var record = Required(Enrollment, "un estudiante matriculado").Attendance.SingleOrDefault(x => x.SessionId == sessionId)
                ?? throw new InputException("No existe asistencia para corregir. Usa Registrar asistencia.");
            await Workspace.Attendance.UpdateAttendanceAsync(Required(Course, "un curso").Id, record.Id, SelectedStatus());
        }, "Asistencia corregida sin duplicar el registro."));
    }
    private AttendanceStatus SelectedStatus() => Status switch { "Presente" => AttendanceStatus.Present, "Ausente" => AttendanceStatus.Absent, _ => throw new InputException("Selecciona Presente o Ausente.") };
    private void SyncStatus() => Status = Enrollment?.Attendance.SingleOrDefault(x => x.SessionId == Session?.Id)?.Status == AttendanceStatus.Absent ? "Ausente" : "Presente";
    public override void Load()
    {
        var courseId = Course?.Id; var sessionId = Session?.Id; var enrollmentId = Enrollment?.Id;
        Notify(nameof(Courses)); Course = Courses.SingleOrDefault(x => x.Id == courseId);
        Session = Sessions.SingleOrDefault(x => x.Id == sessionId);
        Enrollment = Enrollments.SingleOrDefault(x => x.Id == enrollmentId);
        Notify(nameof(Rows));
    }
}
