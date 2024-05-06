using Mystrose.Controls.Main;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;

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
    public void Handle(GameHost host, ZMMessage message)
    {
        switch (message.Command)
        {
            case "mv":
                HandleMove(host, message.Arguments);
                break;
            case "moveToCell":
                HandleMoveToCell(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Cell
    private void HandleMoveToCell(GameHost host, string[] args)
    {
        host.World.Master.Cell = args[5];
        host.World.Master.Pad = args[6];

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
    }
    #endregion

    #region Methods: Movement
    private void HandleMove(GameHost host, string[] args)
    {
        host.World.Master.X = double.Parse(args[5]);
        host.World.Master.Y = double.Parse(args[6]);

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Self, host.World.Master);
    }
    #endregion

}
