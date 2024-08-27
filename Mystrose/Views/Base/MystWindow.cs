namespace Mystrose.Views.Base;

public class MystWindow : FluentWindow, IDestructible
{

    #region Constructor
    public MystWindow() : base()
    {
        Loaded += OnLoaded;
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

    #region Methods: Pre-setup
    protected void InitializeComponent()
    {
        SVCLogger.LogOnConsole("MystWindow pre-initialized.", $"MystWindow-{Name}", "InitializeComponent");
    }
    #endregion

    #region Methods: Invoker
    protected Response<Action> Invoke(Action action)
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
            SVCLogger.LogOnException($"({action.Method.Name}) " + ex.ToString());
        }

        Response<Action> response = new(isInvoked,
            $"({action.Method.Name}) " + (isInvoked is true ? "Action invoked to the interface successfully." : "Action failed to invoke."),
            action);

        SVCLogger.LogOnTrace(response.Message);

        return response;
    }
    #endregion

    #region Methods: Notifier
    protected void NotifyInfo(string title, string content)
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

    protected void NotifySuccess(string title, string content)
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

    protected void NotifyFailure(string title, string content)
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

    protected void NotifyException(string title, string content)
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
    protected void ShowDialog(string title, string content)
    {
        ContentDialog contentDialog = new(CPST_ContentDialog)
        {
            Title = title,
            Content = content,

            CloseButtonText = "Cancel"
        };

        contentDialog.ShowAsync();
    }

    protected async void ShowActionDialog(string title, string content, string primaryButtonText, string secondaryButtonText, Action primaryAction, Action secondaryAction, string closeButtonText = "Cancel")
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

    #region Methods: Interface Handlers
    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        InitializeComponent();

        SVCLogger.LogOnConsole("MystWindow is ready to go.", $"MystWindow-{Name}", "OnLoaded");
    }
    #endregion

    #region Overrides: Interface
    public virtual void Destruct()
    {
        return;
    }

    public virtual void Dispose()
    {
        SVCLogger.LogOnConsole("MystWindow is disposed.", $"MystWindow-{Name}", "Dispose");

        GC.SuppressFinalize(this);
    }
    #endregion

    #region Overrides: Events
    protected override void OnClosed(EventArgs e)
    {
        Destruct();

        base.OnClosed(e);
        Dispose();
    }
    #endregion

}
