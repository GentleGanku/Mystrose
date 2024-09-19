using Application = System.Windows.Application;

namespace Mystrose;

public partial class App : Application
{

    protected override async void OnStartup(StartupEventArgs e)
    {
        //MUserAgent.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36");
        MUserAgent.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ArtixGameLauncher/2.1.2 Chrome/80.0.3987.163 Electron/8.5.5 Safari/537.36");

        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        base.OnStartup(e);
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception ex = (Exception)e.ExceptionObject;
        // WIP
    }

}
