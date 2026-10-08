using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed class SubjectsViewModel(Workspace workspace) : ScreenViewModel(workspace)
{
    private SubjectItem? selected;
    private string code = string.Empty, name = string.Empty;
    public override string Title => "Asignaturas";
    public override string Description => "Organiza el catálogo de asignaturas. Los códigos permanecen estables al editar.";
    public IReadOnlyList<SubjectItem> Items => Workspace.Snapshot.Subjects;
    public SubjectItem? Selected { get => selected; set { if (Set(ref selected, value) && value is not null) { Code = value.Code; Name = value.Name; } Notify(nameof(IsNew)); } }
    public bool IsNew => Selected is null;
    public string Code { get => code; set => Set(ref code, value); }
    public string Name { get => name; set => Set(ref name, value); }
    public ICommand NewCommand => new ActionCommand(New);
    public ICommand SaveCommand => new AsyncCommand(() => ExecuteAsync(SaveAsync, "Asignatura guardada correctamente."));
    public override void Load() { var id = Selected?.Id; Notify(nameof(Items)); Selected = Items.SingleOrDefault(x => x.Id == id); }
    public void New() { Selected = null; Code = Name = string.Empty; Message = string.Empty; }
    private async Task SaveAsync()
    {
        if (Selected is null) { var created = await Workspace.Subjects.RegisterSubjectAsync(Code, Name); selected = new SubjectItem(created.Id, created.Code, created.Name); }
        else await Workspace.Subjects.UpdateSubjectAsync(Selected.Id, Name);
    }
}
