using System.Globalization;
using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed class CoursesViewModel : ScreenViewModel
{
    private CourseItem? selected;
    private SubjectItem? subject;
    private StudentItem? student;
    private string section = "A", period, assessmentName = string.Empty, sessionTime = "09:00";
    private DateTime? start, end, enrollmentDate, assessmentDate, sessionDate;
    public override string Title => "Cursos";
    public override string Description => "Crea una sección y prepara sus matrículas, evaluaciones y sesiones.";
    public IReadOnlyList<CourseItem> Items => Workspace.Snapshot.Courses;
    public IReadOnlyList<SubjectItem> Subjects => Workspace.Snapshot.Subjects;
    public IReadOnlyList<StudentItem> Students => Workspace.Snapshot.Students;
    public IReadOnlyList<EnrollmentItem> Enrollments => Selected?.Enrollments ?? [];
    public IReadOnlyList<AssessmentItem> Assessments => Selected?.Assessments ?? [];
    public IReadOnlyList<SessionItem> Sessions => Selected?.Sessions ?? [];
    public CourseItem? Selected { get => selected; set { Set(ref selected, value); Notify(nameof(Enrollments)); Notify(nameof(Assessments)); Notify(nameof(Sessions)); Notify(nameof(SelectedLabel)); } }
    public string SelectedLabel => Selected?.Display ?? "Selecciona un curso de la lista para preparar su actividad.";
    public SubjectItem? Subject { get => subject; set => Set(ref subject, value); }
    public StudentItem? Student { get => student; set => Set(ref student, value); }
    public string Section { get => section; set => Set(ref section, value); }
    public string Period { get => period; set => Set(ref period, value); }
    public string AssessmentName { get => assessmentName; set => Set(ref assessmentName, value); }
    public string SessionTime { get => sessionTime; set => Set(ref sessionTime, value); }
    public DateTime? Start { get => start; set => Set(ref start, value); }
    public DateTime? End { get => end; set => Set(ref end, value); }
    public DateTime? EnrollmentDate { get => enrollmentDate; set => Set(ref enrollmentDate, value); }
    public DateTime? AssessmentDate { get => assessmentDate; set => Set(ref assessmentDate, value); }
    public DateTime? SessionDate { get => sessionDate; set => Set(ref sessionDate, value); }
    public ICommand CreateCommand { get; }
    public ICommand EnrollCommand { get; }
    public ICommand AssessmentCommand { get; }
    public ICommand SessionCommand { get; }

    public CoursesViewModel(Workspace workspace) : base(workspace)
    {
        var today = workspace.Time.LocalNow.Date;
        period = $"{today.Year}-{(today.Month <= 6 ? 1 : 2)}";
        start = enrollmentDate = assessmentDate = sessionDate = today;
        end = today.AddMonths(3);
        CreateCommand = new AsyncCommand(() => ExecuteAsync(async () =>
        {
            var created = await Workspace.Courses.CreateCourseAsync(Required(Subject, "una asignatura").Id,
                Section, Period, Date(Start, "la fecha de inicio"), Date(End, "la fecha de término"));
            selected = new CourseItem(created.Id, created.Subject.Id, created.Subject.Name, created.SectionCode,
                created.Period, created.StartDate, created.EndDate, [], [], []);
        }, "Curso creado. Ya puedes preparar sus matrículas y actividades."));
        EnrollCommand = new AsyncCommand(() => ExecuteAsync(async () =>
            await Workspace.Courses.EnrollStudentAsync(Required(Selected, "un curso").Id,
                Required(Student, "un estudiante").Id, Date(EnrollmentDate, "la fecha de matrícula")), "Matrícula registrada."));
        AssessmentCommand = new AsyncCommand(() => ExecuteAsync(async () =>
            await Workspace.Courses.CreateAssessmentAsync(Required(Selected, "un curso").Id,
                AssessmentName, Date(AssessmentDate, "la fecha de evaluación")), "Evaluación creada."));
        SessionCommand = new AsyncCommand(() => ExecuteAsync(async () =>
        {
            if (!TimeOnly.TryParseExact(SessionTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                throw new InputException("Escribe la hora en formato HH:mm; por ejemplo, 09:00.");
            await Workspace.Courses.CreateSessionAsync(Required(Selected, "un curso").Id,
                Date(SessionDate, "la fecha de sesión").ToDateTime(time, DateTimeKind.Unspecified));
        }, "Sesión creada con horario de Chile continental."));
    }

    public override void Load()
    {
        var id = Selected?.Id;
        var subjectId = Subject?.Id; var studentId = Student?.Id;
        Notify(nameof(Items)); Notify(nameof(Subjects)); Notify(nameof(Students));
        Selected = Items.SingleOrDefault(x => x.Id == id);
        Subject = Subjects.SingleOrDefault(x => x.Id == subjectId);
        Student = Students.SingleOrDefault(x => x.Id == studentId);
    }
}
