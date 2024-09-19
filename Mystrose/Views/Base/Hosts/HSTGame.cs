namespace Mystrose.Views.Base.Hosts;

public class HSTGame : WindowsFormsHost
{

    #region Constructor
    public HSTGame(ClientUseIdentifier identifier)
    {
        FlashAPI = new(identifier);
        NetworkMonitor = new(identifier);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    #endregion

    #region Properties
    public ISVCFlashAPI FlashAPI
    {
        get;
        set;
    }

    public ISVCNetwork NetworkMonitor
    {
        get;
        set;
    }
    #endregion

    #region Methods: Setup
    public void Destruct()
    {
        Child = null;

        FlashAPI.Dispose();
        NetworkMonitor.Dispose();
        Dispose();
    }
    #endregion

    #region Methods: Actions
    public Response<Action> Refresh()
    {
        Response<Action> response = Invoke(() =>
        {
            FlashAPI.Initialize(this);
            FlashAPI.CallEvent += InspectCall;
        });

        return response;
    }
    #endregion

    #region Methods: Invoker
    private Response<Action> Invoke(Action action)
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

    #region Methods: Service Handlers
    private void InspectCall(string function, string args)
    {
        switch (function)
        {
            case "interceptPacket":
                NetworkMonitor.MonitorServerPacket(args);
                break;
            case "interceptClient":
                NetworkMonitor.MonitorClientPacket(args);
                break;
        }
    }
    #endregion

    #region Handlers: Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SVCLogger.LogOnConsole("Loaded HSTGame into the screen.", "HSTGame", "OnLoaded");
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        SVCLogger.LogOnConsole("Unloaded HSTGame from the screen.", "HSTGame", "OnUnloaded");
    }
    #endregion

}
