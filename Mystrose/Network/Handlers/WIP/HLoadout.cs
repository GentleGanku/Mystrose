using Mystrose.Controls.Main;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;

namespace Mystrose.Network.Handlers.WIP;

public class HLoadout : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "addLoadout",
            "removeLoadout",
            "wearLoadout"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, XTMessage message)
    {
        // TODO: Implement
    }
    #endregion

}
