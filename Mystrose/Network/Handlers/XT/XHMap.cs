namespace Mystrose.Network.Handlers.XT;

public class XHMap : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "exitArea"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, XTMessage message)
    {
        switch (message.Command)
        {
            case "exitArea":
                HandleExit(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Exit
    private void HandleExit(GameHost host, string[] args) 
    {
        int id = int.Parse(args[4]);

        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar == null)
        {
            return;
        }

        host.World.Area.Players.Remove(avatar);

        avatar.Cell = "None";
        avatar.Pad = "None";

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);
    }
    #endregion

}
