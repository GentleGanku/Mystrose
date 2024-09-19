namespace Mystrose.Network.Handlers.XT;

public class XHDungeon : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "dungeonCompleted",
            "dungeonMTC"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(XTMessage message)
    {
        switch (message.Command)
        {
            case "dungeonCompleted":
                HandleComplete(message);
                break;
            case "dungeonMTC":
                HandleMove(message);
                break;
        }
    }
    #endregion

    #region Methods: Complete
    private void HandleComplete(XTMessage message)
    {
        // WIP
    }
    #endregion

    #region Methods: Move
    private void HandleMove(XTMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        bool isSuccess = bool.Parse(args[2]);

        if (!isSuccess)
        {
            return;
        }

        string[] destination = args[3].Split(',');

        world.Avatar.Cell = destination[0];
        world.Avatar.Pad = destination[1];
    }
    #endregion

}
