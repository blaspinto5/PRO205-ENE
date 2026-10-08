using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed record GradeRow(EnrollmentItem Enrollment, string Student, string Value);

public sealed class GradesViewModel : ScreenViewModel
{
    private CourseItem? course;
    private AssessmentItem? assessment;
    private EnrollmentItem? enrollment;
    private GradeRow? selectedRow;
    private string value = string.Empty;
    public override string Title => "Calificaciones";
    public override string Description => "Registra una nota por evaluación y estudiante. Las correcciones se realizan de forma explícita.";
    public IReadOnlyList<CourseItem> Courses => Workspace.Snapshot.Courses;
    public IReadOnlyList<AssessmentItem> Assessments => Course?.Assessments ?? [];
    public IReadOnlyList<EnrollmentItem> Enrollments => Course?.Enrollments ?? [];
    public CourseItem? Course { get => course; set { if (Set(ref course, value)) { Assessment = null; Enrollment = null; } Notify(nameof(Assessments)); Notify(nameof(Enrollments)); Notify(nameof(Rows)); } }
    public AssessmentItem? Assessment { get => assessment; set { Set(ref assessment, value); SyncValue(); Notify(nameof(Rows)); } }
    public EnrollmentItem? Enrollment { get => enrollment; set { Set(ref enrollment, value); SyncValue(); } }
    public GradeRow? SelectedRow { get => selectedRow; set { if (Set(ref selectedRow, value) && value is not null) Enrollment = value.Enrollment; } }
    public string Value { get => value; set => Set(ref this.value, value); }
    public IReadOnlyList<GradeRow> Rows => Assessment is null ? [] : Enrollments.Select(e =>
        new GradeRow(e, e.Student, e.Grades.SingleOrDefault(g => g.AssessmentId == Assessment.Id)?.Value.ToString("0.0") ?? "Pendiente")).ToArray();
    public ICommand RegisterCommand { get; }
    public ICommand CorrectCommand { get; }

    public GradesViewModel(Workspace workspace) : base(workspace)
    {
        RegisterCommand = new AsyncCommand(() => ExecuteAsync(async () =>
            await Workspace.Grades.RegisterGradeAsync(Required(Course, "un curso").Id,
                Required(Enrollment, "un estudiante matriculado").Id, Required(Assessment, "una evaluación").Id,
                ParseGrade(Value.Trim())), "Nota registrada correctamente."));
        CorrectCommand = new AsyncCommand(() => ExecuteAsync(async () =>
        {
            var assessmentId = Required(Assessment, "una evaluación").Id;
            var grade = Required(Enrollment, "un estudiante matriculado").Grades.SingleOrDefault(x => x.AssessmentId == assessmentId)
                ?? throw new InputException("No existe una nota para corregir. Usa Registrar nota.");
            await Workspace.Grades.UpdateGradeAsync(Required(Course, "un curso").Id, grade.Id, ParseGrade(Value.Trim()));
        }, "Nota corregida. Se conservó la identidad del registro."));
    }

    public override void Load()
    {
        var courseId = Course?.Id; var assessmentId = Assessment?.Id; var enrollmentId = Enrollment?.Id;
        Notify(nameof(Courses)); Course = Courses.SingleOrDefault(x => x.Id == courseId);
        Assessment = Assessments.SingleOrDefault(x => x.Id == assessmentId);
        Enrollment = Enrollments.SingleOrDefault(x => x.Id == enrollmentId);
        Notify(nameof(Rows));
    }
    private void SyncValue() => Value = Enrollment?.Grades.SingleOrDefault(x => x.AssessmentId == Assessment?.Id)?.Value.ToString("0.0") ?? string.Empty;
}
