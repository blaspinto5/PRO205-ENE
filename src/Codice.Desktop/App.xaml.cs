using System.IO;
using System.Windows;
using Codice.Infrastructure;
using Codice.Desktop.ViewModels;
using System.Globalization;

namespace Codice.Desktop;

public partial class App : System.Windows.Application
{
    private StorageLease? lease;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("es-CL");
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        var verification = e.Args.Contains("--verify-startup", StringComparer.Ordinal);
        var previewIndex = Array.IndexOf(e.Args, "--capture-preview");
        try
        {
            var storage = new PortableStorage(AppContext.BaseDirectory);
            storage.PrepareDirectory();
            lease = StorageLease.Acquire(storage);
            await storage.InitializeAsync();
            var repositories = new SqliteRepositories(storage);
            var workspace = new Workspace(repositories);
            await workspace.RefreshAsync();
            var viewModel = new NavigationViewModel(workspace);
            if (verification) { Shutdown(0); return; }
            MainWindow = new MainWindow { DataContext = viewModel };
            if (previewIndex >= 0)
            {
                if (previewIndex + 1 >= e.Args.Length) throw new ArgumentException("Preview output folder is required.");
                await UiPreview.CreateAsync((MainWindow)MainWindow, viewModel, workspace, e.Args[previewIndex + 1]);
                Shutdown(0); return;
            }
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
        }
        catch (Exception exception)
        {
            if (previewIndex >= 0 && previewIndex + 1 < e.Args.Length)
            {
                var output = e.Args[previewIndex + 1];
                Directory.CreateDirectory(output);
                await File.WriteAllTextAsync(Path.Combine(output, "preview-error.txt"), exception.ToString());
            }
            var message = exception switch
            {
                UnauthorizedAccessException => "No hay permiso para guardar datos. Mueve la carpeta completa a una ubicación con permiso de escritura.",
                IOException => "No se pudo abrir la carpeta de datos. Comprueba que Códice no esté abierto y que la ubicación esté disponible.",
                _ => "No se pudo iniciar la base de datos. Conserva la carpeta datos y solicita revisión antes de volver a intentar."
            };
            if (!verification && previewIndex < 0) MessageBox.Show(message, "Códice", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(3);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        lease?.Dispose();
        base.OnExit(e);
    }
}
