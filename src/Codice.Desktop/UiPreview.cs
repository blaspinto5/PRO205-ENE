using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Controls;
using Codice.Domain;
using Codice.Desktop.ViewModels;

namespace Codice.Desktop;

internal static class UiPreview
{
    public static async Task CreateAsync(MainWindow window, NavigationViewModel navigation, Workspace workspace, string output)
    {
        Directory.CreateDirectory(output);
        var errors = new BindingErrorListener();
        PresentationTraceSources.DataBindingSource.Listeners.Add(errors);
        PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
        if (workspace.Snapshot.Students.Count != 0 || workspace.Snapshot.Subjects.Count != 0 || workspace.Snapshot.Courses.Count != 0)
            throw new InvalidOperationException("UI preview requires an empty, disposable data folder.");
        await SeedAsync(workspace);
        window.Show();
        var index = 0;
        foreach (var item in navigation.Navigation)
        {
            await navigation.NavigateAsync(item.Screen);
            Prepare(item.Screen, workspace);
            if (item.Screen is ReportsViewModel report) await ((AsyncCommand)report.GenerateCommand).ExecuteAsync();
            await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
            Save(window, Path.Combine(output, $"{++index:00}-{item.Screen.GetType().Name.Replace("ViewModel", string.Empty)}.png"));
        }
        var courseScreen = (CoursesViewModel)navigation.Navigation.Single(x => x.Screen is CoursesViewModel).Screen;
        await navigation.NavigateAsync(courseScreen);
        Prepare(courseScreen, workspace);
        await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
        var tabs = Find<TabControl>((DependencyObject)window.Content) ?? throw new InvalidOperationException("Course tabs are missing.");
        for (var tab = 1; tab <= 2; tab++)
        {
            tabs.SelectedIndex = tab;
            await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
            Save(window, Path.Combine(output, tab == 1 ? "10-CourseAssessments.png" : "11-CourseSessions.png"));
        }
        var studentReport = (ReportsViewModel)navigation.Navigation.Single(x => x.Screen is ReportsViewModel).Screen;
        await navigation.NavigateAsync(studentReport);
        studentReport.Kind = "Por estudiante"; studentReport.Student = studentReport.Students.First();
        await ((AsyncCommand)studentReport.GenerateCommand).ExecuteAsync();
        await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
        Save(window, Path.Combine(output, "12-StudentReport.png"));
        var grades = (GradesViewModel)navigation.Navigation.Single(x => x.Screen is GradesViewModel).Screen;
        await navigation.NavigateAsync(grades);
        Prepare(grades, workspace);
        grades.Value = "7,8";
        await ((AsyncCommand)grades.CorrectCommand).ExecuteAsync();
        await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
        Save(window, Path.Combine(output, "08-GradeValidation.png"));
        window.Width = window.MinWidth; window.Height = window.MinHeight;
        await window.Dispatcher.InvokeAsync(() => window.UpdateLayout(), DispatcherPriority.ContextIdle);
        Save(window, Path.Combine(output, "09-MinimumWindow.png"));
        await File.WriteAllLinesAsync(Path.Combine(output, "binding-errors.txt"), errors.Errors);
        PresentationTraceSources.DataBindingSource.Listeners.Remove(errors);
        if (errors.Errors.Count > 0) throw new InvalidOperationException("Binding errors were detected during UI preview.");
    }

    private static T? Find<T>(DependencyObject root) where T : DependencyObject
    {
        if (root is T match) return match;
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            if (Find<T>(VisualTreeHelper.GetChild(root, i)) is { } found) return found;
        return null;
    }

    private static void Save(Window window, string path)
    {
        var content = (FrameworkElement)window.Content;
        var image = new RenderTargetBitmap((int)Math.Ceiling(content.ActualWidth), (int)Math.Ceiling(content.ActualHeight), 96, 96, PixelFormats.Pbgra32);
        image.Render(content);
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(image));
        using var stream = File.Create(path);
        encoder.Save(stream);
    }

    private static void Prepare(ScreenViewModel screen, Workspace workspace)
    {
        switch (screen)
        {
            case StudentsViewModel students: students.Selected = students.Items.First(); break;
            case SubjectsViewModel subjects: subjects.Selected = subjects.Items.First(); break;
            case CoursesViewModel courses:
                courses.Selected = courses.Items.First(); courses.Subject = courses.Subjects.First();
                courses.Student = courses.Students.Last(); break;
            case GradesViewModel grades:
                grades.Course = grades.Courses.First(); grades.Assessment = grades.Assessments.First();
                grades.Enrollment = grades.Enrollments.First(); break;
            case AttendanceViewModel attendance:
                attendance.Course = attendance.Courses.First(); attendance.Session = attendance.Sessions.First();
                attendance.Enrollment = attendance.Enrollments.First(); break;
            case ReportsViewModel reports:
                reports.Course = reports.Courses.First(); reports.Cutoff = workspace.Time.LocalNow.Date; break;
        }
    }

    private static async Task SeedAsync(Workspace workspace)
    {
        var today = DateOnly.FromDateTime(workspace.Time.LocalNow);
        var names = new[] { ("Ana", "Pérez"), ("Benjamín", "Soto"), ("Camila", "Rojas"), ("Diego", "Muñoz"), ("Elena", "Vargas"), ("Felipe", "Contreras") };
        var students = new List<Student>();
        for (var i = 0; i < names.Length; i++) students.Add(await workspace.Students.RegisterStudentAsync($"EST-{i + 1:000}", names[i].Item1, names[i].Item2));
        var programming = await workspace.Subjects.RegisterSubjectAsync("PRO205", "Programación orientada a objetos");
        var databases = await workspace.Subjects.RegisterSubjectAsync("BD101", "Fundamentos de bases de datos");
        await workspace.Subjects.RegisterSubjectAsync("MAT110", "Matemática aplicada");
        var course = await workspace.Courses.CreateCourseAsync(programming.Id, "A", "2026-2", today.AddDays(-30), today.AddDays(60));
        await workspace.Courses.CreateCourseAsync(databases.Id, "B", "2026-2", today.AddDays(-30), today.AddDays(60));
        var assessment = await workspace.Courses.CreateAssessmentAsync(course.Id, "Evaluación práctica 1", today.AddDays(-5));
        await workspace.Courses.CreateAssessmentAsync(course.Id, "Proyecto de aplicación", today.AddDays(10));
        var session = await workspace.Courses.CreateSessionAsync(course.Id, today.AddDays(-2).ToDateTime(new TimeOnly(9, 0)));
        await workspace.Courses.CreateSessionAsync(course.Id, today.AddDays(-1).ToDateTime(new TimeOnly(9, 0)));
        var values = new[] { 5.4m, 6.1m, 5.5m, 5.9m, 6.6m, 4.8m };
        for (var i = 0; i < students.Count; i++)
        {
            var enrollment = await workspace.Courses.EnrollStudentAsync(course.Id, students[i].Id, today.AddDays(-20));
            await workspace.Grades.RegisterGradeAsync(course.Id, enrollment.Id, assessment.Id, values[i]);
            if (i < 5) await workspace.Attendance.RecordAttendanceAsync(course.Id, enrollment.Id, session.Id,
                i == 1 ? AttendanceStatus.Absent : AttendanceStatus.Present);
        }
        await workspace.RefreshAsync();
    }

    private sealed class BindingErrorListener : TraceListener
    {
        public List<string> Errors { get; } = [];
        public override void Write(string? message) { if (message is not null) Errors.Add(message); }
        public override void WriteLine(string? message) { if (message is not null) Errors.Add(message); }
    }
}
