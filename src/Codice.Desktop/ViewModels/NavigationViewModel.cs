using System.Windows.Input;
using Codice.Application;

namespace Codice.Desktop.ViewModels;

public sealed class NavigationItem(string title, ScreenViewModel screen, Func<ScreenViewModel, Task> navigate) : ObservableObject
{
    private bool selected;
    public string Title { get; } = title;
    public ScreenViewModel Screen { get; } = screen;
    public bool IsSelected { get => selected; set => Set(ref selected, value); }
    public ICommand Command { get; } = new AsyncCommand(() => navigate(screen));
}

public sealed class NavigationViewModel : ObservableObject
{
    private ScreenViewModel current;
    public ScreenViewModel Current { get => current; private set => Set(ref current, value); }
    public IReadOnlyList<NavigationItem> Navigation { get; }
    public ICommand RefreshCommand { get; }
    public NavigationViewModel(Workspace workspace)
    {
        var screens = new ScreenViewModel[] { new HomeViewModel(workspace), new StudentsViewModel(workspace),
            new SubjectsViewModel(workspace), new CoursesViewModel(workspace), new GradesViewModel(workspace),
            new AttendanceViewModel(workspace), new ReportsViewModel(workspace) };
        current = screens[0];
        Navigation = screens.Select(s => new NavigationItem(s.Title, s, NavigateAsync)).ToArray();
        Navigation[0].IsSelected = true;
        current.Load();
        RefreshCommand = new AsyncCommand(() => Current.ActivateAsync());
    }

    public async Task NavigateAsync(ScreenViewModel screen)
    {
        Current = screen;
        foreach (var item in Navigation) item.IsSelected = item.Screen == screen;
        await screen.ActivateAsync();
    }
}

public sealed class HomeViewModel(Workspace workspace) : ScreenViewModel(workspace)
{
    public override string Title => "Inicio";
    public override string Description => "Una vista clara de tu actividad académica, en un solo lugar.";
    public int StudentCount => Workspace.Snapshot.Students.Count;
    public int SubjectCount => Workspace.Snapshot.Subjects.Count;
    public int CourseCount => Workspace.Snapshot.Courses.Count;
    public IReadOnlyList<CourseItem> Courses => Workspace.Snapshot.Courses;
    public override void Load() { Notify(nameof(StudentCount)); Notify(nameof(SubjectCount)); Notify(nameof(CourseCount)); Notify(nameof(Courses)); }
}
