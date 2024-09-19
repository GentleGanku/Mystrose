namespace Mystrose.Network.Handlers.ZM;

public class ZHMovement : IZMMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "mv",
            "moveToCell"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(ZMMessage message)
    {
        switch (message.Command)
        {
            case "mv":
                HandleMove(message);
                break;

            case "moveToCell":
                HandleMoveToCell(message);
                break;
        }
    }
    #endregion

    #region Methods: Movement
    private void HandleMove(ZMMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        world.Avatar.X = double.Parse(args[5]);
        world.Avatar.Y = double.Parse(args[6]);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
    }
    #endregion

    #region Methods: Cell
    private void HandleMoveToCell(ZMMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        world.Avatar.Cell = args[5];
        world.Avatar.Pad = args[6];

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, world.Avatar);
    }
    #endregion

}
