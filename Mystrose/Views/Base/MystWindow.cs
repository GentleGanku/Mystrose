using MessageBox = Wpf.Ui.Controls.MessageBox;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;

namespace Mystrose.Views.Base;

public class MystWindow : FluentWindow, IDestructible
{

    #region Constructor
    public MystWindow() : base()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Closed += OnClosed;

        SVCViewManager.Render(this);
    }
    #endregion

    #region Properties
    private ContentPresenter CPST_ContentDialog
    {
        get
        {
            Grid grid = (Grid)Content;
            return (ContentPresenter)grid.FindName("CPST_ContentDialog");
        }
    }

    private SnackbarPresenter SBPST_Base
    {
        get
        {
            Grid grid = (Grid)Content;
            return (SnackbarPresenter)grid.FindName("SBPST_Base");
        }
    }
    #endregion

    #region Methods: Utility
    public void OpenHyperlink(string url)
    {
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(url)
            {
                UseShellExecute = true
            };

            Process.Start(processStartInfo);

            SVCLogger.LogOnTrace($"Hyperlink opened: {url}");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException("(OpenHyperlink)" + ex.ToString());
        }
    }
    #endregion

    #region Methods: Invoker
    public Response<Action> Invoke(Action action)
    {
        bool isInvoked = false;

        try
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.Invoke(action);
            }

            isInvoked = true;
        }
        catch (Exception ex)
        {
            //SVCLogger.LogOnException($"({action.Method.Name}) " + ex.ToString());
            SVCLogger.LogOnConsole($"({action.Method.Name}) " + ex.ToString(), $"MystWindow-{Name}", "Invoke");
        }

        Response<Action> response = new(isInvoked,
            $"({action.Method.Name}) " + (isInvoked is true ? "Action invoked to the interface successfully." : "Action failed to invoke."),
            action);

        //SVCLogger.LogOnTrace(response.Message);
        SVCLogger.LogOnConsole(response.Message, $"MystWindow-{Name}", "Invoke");

        return response;
    }
    #endregion

    #region Methods: Notifier
    public void NotifyInfo(string title, string content)
    {
        Snackbar snackbar = new(SBPST_Base)
        {
            Title = title,
            Content = content,
            Timeout = TimeSpan.FromSeconds(2),

            Appearance = ControlAppearance.Info,
            Icon = new SymbolIcon(SymbolRegular.Info16, filled: true)
        };

        SBPST_Base.AddToQue(snackbar);
    }

    public void NotifySuccess(string title, string content)
    {
        Snackbar snackbar = new(SBPST_Base)
        {
            Title = title,
            Content = content,
            Timeout = TimeSpan.FromSeconds(2),

            Appearance = ControlAppearance.Success,
            Icon = new SymbolIcon(SymbolRegular.CheckmarkCircle16, filled: true)
        };

        SBPST_Base.AddToQue(snackbar);
    }

    public void NotifyFailure(string title, string content)
    {
        Snackbar snackbar = new(SBPST_Base)
        {
            Title = title,
            Content = content,
            Timeout = TimeSpan.FromSeconds(2),

            Appearance = ControlAppearance.Danger,
            Icon = new SymbolIcon(SymbolRegular.ErrorCircle16, filled: true)
        };

        SBPST_Base.AddToQue(snackbar);
    }

    public void NotifyException(string title, string content)
    {
        Snackbar snackbar = new(SBPST_Base)
        {
            Title = title,
            Content = content,
            Timeout = TimeSpan.FromSeconds(2),

            Appearance = ControlAppearance.Caution,
            Icon = new SymbolIcon(SymbolRegular.Triangle16, filled: true)
        };

        SBPST_Base.AddToQue(snackbar);
    }
    #endregion

    #region Methods: Messenger
    public void ShowDialog(string title, string content)
    {
        ContentDialog contentDialog = new(CPST_ContentDialog)
        {
            Title = title,
            Content = content,

            CloseButtonText = "Close"
        };

        contentDialog.ShowAsync();
    }

    public async void ShowActionDialog(string title, string content, string primaryButtonText, string secondaryButtonText, Action primaryAction, Action secondaryAction, string closeButtonText = "Close")
    {
        ContentDialog contentDialog = new(CPST_ContentDialog)
        {
            Title = title,
            Content = content,

            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            CloseButtonText = closeButtonText
        };

        ContentDialogResult result = await contentDialog.ShowAsync();

        if (result is ContentDialogResult.Primary)
        {
            primaryAction();
        }
        else if (result is ContentDialogResult.Secondary)
        {
            secondaryAction();
        }
    }
    #endregion

    #region Methods: Classic Messenger
    public void ShowMessageBox(string title, string content)
    {
        MessageBox messageBox = new()
        {
            Title = title,
            Content = content,

            CloseButtonText = "Close"
        };

        messageBox.ShowDialogAsync();
    }

    public async void ShowActionMessageBox(string title, string content, string primaryButtonText, string secondaryButtonText, Action primaryAction, Action secondaryAction, string closeButtonText = "Close", Action? closeAction = null)
    {
        MessageBox messageBox = new()
        {
            Title = title,
            Content = content,

            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            CloseButtonText = closeButtonText
        };

        MessageBoxResult result = await messageBox.ShowDialogAsync();

        if (result is MessageBoxResult.Primary)
        {
            primaryAction();
        }
        else if (result is MessageBoxResult.Secondary)
        {
            secondaryAction();
        }
        else if (result is MessageBoxResult.None)
        {
            closeAction?.Invoke();
        }
    }
    #endregion

    #region Methods: Interface
    public void Dispose()
    {
        SVCLogger.LogOnConsole("Disposing the MystWindow...", $"MystWindow-{Name}", "Dispose");

        GC.SuppressFinalize(this);
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SVCLogger.LogOnConsole("Loaded the MystWindow into interface.", $"MystWindow-{Name}", "OnLoaded");
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        SVCLogger.LogOnConsole("Unloaded the MystWindow from interface.", $"MystWindow-{Name}", "OnUnloaded");
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        SVCViewManager.Unrender(this);
        Dispose();
    }
    #endregion

}
