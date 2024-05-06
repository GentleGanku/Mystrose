using Mystrose.Controls.Main;

namespace Mystrose.Network.Messages.Interfaces;

public interface IXTMessageHandler
{
    string[] HandledCommands
    {
        get;
    }

    void Handle(GameHost host, XTMessage message);
}
