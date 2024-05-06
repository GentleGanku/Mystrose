using Mystrose.Network.Messages.Interfaces;
using Mystrose.Network.Messages;
using Mystrose.Controls.Main;

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
    public void Handle(GameHost host, XTMessage message)
    {
        switch (message.Command)
        {
            case "dungeonCompleted":
                HandleComplete(host, message.Arguments);
                break;
            case "dungeonMTC":
                HandleMove(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: Complete
    private void HandleComplete(GameHost host, string[] args)
    {
        // WIP
    }
    #endregion

    #region Methods: Move
    private void HandleMove(GameHost host, string[] args)
    {
        bool isSuccess = bool.Parse(args[2]);

        if (!isSuccess)
        {
            return;
        }

        string[] destination = args[3].Split(',');

        host.World.Master.Cell = destination[0];
        host.World.Master.Pad = destination[1];
    }
    #endregion

}
