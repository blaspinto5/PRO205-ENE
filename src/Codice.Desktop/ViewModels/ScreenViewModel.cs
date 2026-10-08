using System.Globalization;
using Codice.Domain;
using Microsoft.EntityFrameworkCore;

namespace Codice.Desktop.ViewModels;

public abstract class ScreenViewModel(Workspace workspace) : ObservableObject
{
    protected Workspace Workspace { get; } = workspace;
    private string message = string.Empty;
    private bool isError;
    private bool busy;
    public string Message { get => message; protected set => Set(ref message, value); }
    public bool IsError { get => isError; private set => Set(ref isError, value); }
    public bool IsBusy { get => busy; private set => Set(ref busy, value); }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract void Load();

    public virtual async Task ActivateAsync()
    {
        await ExecuteAsync(() => Task.CompletedTask, string.Empty);
    }

    protected async Task ExecuteAsync(Func<Task> action, string success)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            await Workspace.RunAsync(action);
            Load(); IsError = false; Message = success;
        }
        catch (Exception exception)
        {
            IsError = true; Message = FriendlyError(exception);
        }
        finally { IsBusy = false; }
    }

    protected static T Required<T>(T? value, string label) where T : class =>
        value ?? throw new InputException($"Selecciona {label}.");
    protected static DateOnly Date(DateTime? value, string label) => value.HasValue
        ? DateOnly.FromDateTime(value.Value) : throw new InputException($"Indica {label}.");
    protected static decimal ParseGrade(string value)
    {
        if (value.Contains(',') && value.Contains('.') || !decimal.TryParse(value.Replace(',', '.'),
            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number))
            throw new InputException("Escribe una nota numérica; por ejemplo, 5,4.");
        return number;
    }

    private static string FriendlyError(Exception exception) => exception switch
    {
        InputException => exception.Message,
        DomainException => Translate(exception.Message),
        ArgumentException => "Completa los campos obligatorios con datos válidos.",
        DbUpdateException => "No se pudo guardar: hay un registro duplicado o una relación inválida. Actualiza los datos y revisa la selección.",
        _ => "No se pudo completar la operación. Conserva tus datos y vuelve a intentar."
    };

    private static string Translate(string message) => message switch
    {
        "StudentCode is required." => "Completa el código del estudiante.",
        "FirstName is required." => "Completa los nombres del estudiante.",
        "LastName is required." => "Completa los apellidos del estudiante.",
        "Code is required." => "Completa el código de la asignatura.",
        "Name is required." => "Completa el nombre de la asignatura.",
        "SectionCode is required." => "Completa la sección del curso.",
        "Period is required." => "Completa el período del curso.",
        "name is required." => "Completa el nombre de la evaluación.",
        "Student code already exists." => "Ya existe un estudiante con ese código.",
        "Subject code already exists." => "Ya existe una asignatura con ese código.",
        "Course already exists for this subject, period and section." => "Ya existe ese curso para la asignatura, período y sección.",
        "Course dates are reversed." => "La fecha de término debe ser igual o posterior al inicio.",
        "Date must be inside the course period." => "La fecha debe estar dentro del período del curso.",
        "Student is already enrolled." => "El estudiante ya está matriculado en este curso.",
        "Assessment name already exists in this course." => "Ya existe una evaluación con ese nombre en el curso.",
        "Session already exists." => "Ya existe una sesión en esa fecha y hora.",
        "Grade must be between 1.0 and 7.0." => "La nota debe estar entre 1,0 y 7,0.",
        "Grade accepts at most one decimal place." => "La nota admite como máximo una cifra decimal.",
        "Grade already exists for this assessment." => "La nota ya está registrada. Usa Corregir nota para modificarla.",
        "Attendance already exists for this session." => "La asistencia ya está registrada. Usa Corregir asistencia para modificarla.",
        "Cannot record attendance for a future session." => "No se puede registrar asistencia en una sesión futura.",
        "Assessment precedes enrollment." => "La evaluación es anterior a la matrícula del estudiante.",
        "Session precedes enrollment." => "La sesión es anterior a la matrícula del estudiante.",
        "Session time is invalid or ambiguous in the academic time zone." => "La hora no es válida o es ambigua para Chile. Selecciona otra hora.",
        "Report cutoff is invalid or ambiguous." => "La fecha de corte no es válida para la zona horaria académica.",
        "Modification cannot precede the last recorded change." => "La hora del equipo es anterior a la última modificación. Revisa el reloj del sistema.",
        _ => "La operación no cumple las reglas académicas. Revisa los campos y el curso seleccionado."
    };
}

public sealed class InputException(string message) : Exception(message);
