using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed record ReportRow(string Student, string Course, string Average, string State, string Reference, int Present, int Absent, int Unrecorded,
    IReadOnlyList<EvaluationRow> Evaluations);
public sealed record EvaluationRow(string Name, string Date, string Value);

public sealed class ReportsViewModel : ScreenViewModel
{
    private string kind = "Por curso";
    private CourseItem? course;
    private StudentItem? student;
    private DateTime? cutoff;
    private IReadOnlyList<ReportRow> rows = [];
    private ReportRow? selected;
    private string caption = "Selecciona el alcance y genera el informe.";
    public override string Title => "Informes";
    public override string Description => "Revisa notas y asistencia por estudiante o curso, sin convertir pendientes en calificaciones.";
    public IReadOnlyList<string> Kinds { get; } = ["Por curso", "Por estudiante"];
    public IReadOnlyList<CourseItem> Courses => Workspace.Snapshot.Courses;
    public IReadOnlyList<StudentItem> Students => Workspace.Snapshot.Students;
    public string Kind { get => kind; set { if (Set(ref kind, value)) { Course = null; Student = null; } Notify(nameof(IsCourse)); Notify(nameof(IsStudent)); Notify(nameof(CourseLabel)); Clear(); } }
    public bool IsCourse => Kind == "Por curso";
    public bool IsStudent => !IsCourse;
    public string CourseLabel => IsCourse ? "Curso *" : "Curso opcional · vacío = todos";
    public CourseItem? Course { get => course; set { Set(ref course, value); Clear(); } }
    public StudentItem? Student { get => student; set { Set(ref student, value); Clear(); } }
    public DateTime? Cutoff { get => cutoff; set { Set(ref cutoff, value); Clear(); } }
    public IReadOnlyList<ReportRow> Rows { get => rows; private set => Set(ref rows, value); }
    public ReportRow? Selected { get => selected; set { Set(ref selected, value); Notify(nameof(Evaluations)); } }
    public IReadOnlyList<EvaluationRow> Evaluations => Selected?.Evaluations ?? [];
    public string Caption { get => caption; private set => Set(ref caption, value); }
    public ICommand GenerateCommand { get; }
    public ICommand AllCoursesCommand => new ActionCommand(() => Course = null);

    public ReportsViewModel(Workspace workspace) : base(workspace)
    {
        cutoff = workspace.Time.LocalNow.Date;
        GenerateCommand = new AsyncCommand(() => ExecuteAsync(async () =>
        {
            var cut = Date(Cutoff, "la fecha de corte").ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Unspecified);
            IReadOnlyList<EnrollmentReport> results;
            if (IsCourse)
            {
                var report = await Workspace.Reports.GetCourseReportAsync(Required(Course, "un curso").Id, cut);
                results = report.Students;
            }
            else
            {
                var report = await Workspace.Reports.GetStudentReportAsync(Required(Student, "un estudiante").Id, cut, Course?.Id);
                results = report.Courses;
            }
            Rows = results.Select(r => new ReportRow($"{r.StudentCode} · {r.FirstName} {r.LastName}",
                $"{r.SubjectName} · {r.Section} · {r.Period}", r.Average?.ToString("0.0") ?? "Sin notas",
                r.IsProvisional ? "Provisional" : r.Average.HasValue ? "Completo" : "Sin notas",
                r.IsAtOrAboveReference.HasValue ? r.IsAtOrAboveReference.Value ? "Promedio ≥ 4,0" : "Promedio < 4,0" : "Sin promedio disponible",
                r.Attendance.Present, r.Attendance.Absent, r.Attendance.Unrecorded,
                r.Evaluations.Select(e => new EvaluationRow(e.Name, e.Date.ToString("dd/MM/yyyy"), e.Value?.ToString("0.0") ?? "Pendiente")).ToArray())).ToArray();
            Selected = Rows.FirstOrDefault();
            Caption = Rows.Count == 0 ? "No hay matrículas para este alcance." : $"{Rows.Count} registros académicos · corte de asistencia: {Cutoff:dd/MM/yyyy}";
        }, "Informe actualizado. El promedio es informativo; no determina aprobación institucional."));
    }
    private void Clear() { Rows = []; Selected = null; Caption = "Selección modificada. Genera el informe para actualizar los resultados."; }
    public override async Task ActivateAsync() { Clear(); await base.ActivateAsync(); }
    public override void Load()
    {
        var courseId = Course?.Id; var studentId = Student?.Id;
        var savedRows = Rows; var savedCaption = Caption; var savedSelected = Selected;
        Notify(nameof(Courses)); Notify(nameof(Students));
        Course = Courses.SingleOrDefault(x => x.Id == courseId);
        Student = Students.SingleOrDefault(x => x.Id == studentId);
        Rows = savedRows; Selected = savedSelected; Caption = savedCaption;
    }
}
