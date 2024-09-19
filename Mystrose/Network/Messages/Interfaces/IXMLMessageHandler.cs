namespace Mystrose.Network.Messages.Interfaces;

public interface IXMLMessageHandler
{

    string[] HandledCommands
    {
        get;
    }

    void Handle(XMLMessage message);

}
