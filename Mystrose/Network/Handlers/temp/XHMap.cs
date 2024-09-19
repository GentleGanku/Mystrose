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
    public void Handle(XTMessage message)
    {
        switch (message.Command)
        {
            case "exitArea":
                HandleExit(message);
                break;
        }
    }
    #endregion

    #region Methods: Exit
    private void HandleExit(XTMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        int id = int.Parse(args[4]);

        Avatar? avatar = world.Environment.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == id;
            });

        if (avatar is null)
        {
            return;
        }

        world.Environment.Area.Players.Remove(avatar);

        avatar.Cell = "None";
        avatar.Pad = "None";

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
