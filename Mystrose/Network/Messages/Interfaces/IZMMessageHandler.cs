using Mystrose.Controls.Main;

namespace Mystrose.Network.Messages.Interfaces;

public interface IZMMessageHandler
{
    string[] HandledCommands
    {
        get;
    }

    void Handle(GameHost host, ZMMessage message);
}
