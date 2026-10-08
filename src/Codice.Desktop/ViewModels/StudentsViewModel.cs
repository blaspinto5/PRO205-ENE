using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed class StudentsViewModel(Workspace workspace) : ScreenViewModel(workspace)
{
    private StudentItem? selected;
    private string code = string.Empty, firstName = string.Empty, lastName = string.Empty;
    private string filter = string.Empty;
    public override string Title => "Estudiantes";
    public override string Description => "Registra estudiantes y actualiza sus nombres conservando su historia académica.";
    public IReadOnlyList<StudentItem> Items => Workspace.Snapshot.Students.Where(x => x.Display.Contains(Filter, StringComparison.OrdinalIgnoreCase)).ToArray();
    public string Filter { get => filter; set { Set(ref filter, value); Notify(nameof(Items)); } }
    public StudentItem? Selected { get => selected; set { if (Set(ref selected, value) && value is not null) { Code = value.Code; FirstName = value.FirstName; LastName = value.LastName; } Notify(nameof(IsNew)); } }
    public bool IsNew => Selected is null;
    public string Code { get => code; set => Set(ref code, value); }
    public string FirstName { get => firstName; set => Set(ref firstName, value); }
    public string LastName { get => lastName; set => Set(ref lastName, value); }
    public ICommand NewCommand => new ActionCommand(New);
    public ICommand SaveCommand => new AsyncCommand(() => ExecuteAsync(SaveAsync, "Estudiante guardado correctamente."));
    public override void Load() { var id = Selected?.Id; Notify(nameof(Items)); Selected = Workspace.Snapshot.Students.SingleOrDefault(x => x.Id == id); }
    public void New() { Selected = null; Code = FirstName = LastName = string.Empty; Message = string.Empty; }
    private async Task SaveAsync()
    {
        if (Selected is null) { var created = await Workspace.Students.RegisterStudentAsync(Code, FirstName, LastName); selected = new StudentItem(created.Id, created.StudentCode, created.FirstName, created.LastName); }
        else await Workspace.Students.UpdateStudentAsync(Selected.Id, FirstName, LastName);
    }
}
