namespace Mystrose.Network.Handlers.XT;

public class XHResponse : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "loginResponse",
            "loginMulti",
            "logoutWarning",
            "multiLoginWarning"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, XTMessage message)
    {
        switch (message.Command)
        {
            case "loginResponse":
                HandleLogin(host, message.Arguments);
                break;
            case "loginMulti":
                HandleLogout(host, message.Arguments);
                break;
            case "multiLoginWarning":
                HandleLogout(host, ["", "", "", "", "false"]);
                break;
            case "logoutWarning":
                HandleWarning(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Login
    private void HandleLogin(GameHost host, string[] args)
    {
        int entId = int.Parse(args[5]);
        string name = args[6];

        host.World.Master.EntityID = entId;
        host.World.Master.Name = name;

        host.State = GameStateType.Logged;

        //MainWindow.Instance.NavigationBar.NotifsFlyoutContent.AddItem(host.GroupIndex, host.MainAvatar.Username, "Heart24", "Logged in to the game.");
    }
    #endregion

    #region Methods: Logout
    private void HandleLogout(GameHost host, string[] args)
    {
        bool isSuccess = args[4] == "true";

        if (isSuccess)
        {
            return;
        }

        host.State = GameStateType.Idle;
    }
    #endregion

    #region Methods: Warning
    private void HandleWarning(GameHost host, string[] args)
    {
        host.State = GameStateType.Locked;
    }
    #endregion

}
