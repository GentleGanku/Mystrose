namespace Mystrose.Views.Base.Hosts;

public class HSTGame(ClientInstanceIdentifier identifier) : WindowsFormsHost
{

    #region Properties
    public ISVCFlashAPI FlashAPI
    {
        get;
        init;
    } = new(identifier);

    public ISVCNetwork NetworkMonitor
    {
        get;
        init;
    } = new(identifier);
    #endregion

    #region Methods: Setup
    public void Destruct()
    {
        Child = null;

        FlashAPI.Deconstruct();
        NetworkMonitor.Deconstruct();
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
            HSVCLogger.Instance.LogOnException($"({action.Method.Name}) " + ex.ToString());
        }

        Response<Action> response = new(isInvoked,
            $"({action.Method.Name}) " + (isInvoked is true ? "Action invoked to the interface successfully." : "Action failed to invoke."),
            action);

        HSVCLogger.Instance.LogOnTrace(response.Message);

        return response;
    }
    #endregion

    #region Methods: Service Handlers
    private void InspectCall(string function, string args)
    {
        switch (function)
        {
            case "interceptPacket":
                NetworkMonitor.MonitorPacket(args);
                break;
            case "interceptClient":
                break;
        }
    }
    #endregion

}
