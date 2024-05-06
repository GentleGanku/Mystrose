using Mystrose.Controls.Main;

namespace Mystrose.Network.Messages.Interfaces;

public interface IXMLMessageHandler
{
    string[] HandledCommands
    {
        get;
    }

    void Handle(GameHost host, XMLMessage message);
}
