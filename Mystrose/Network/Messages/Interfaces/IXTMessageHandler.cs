namespace Mystrose.Network.Messages.Interfaces;

public interface IXTMessageHandler
{

    string[] HandledCommands
    {
        get;
    }

    void Handle(XTMessage message);

}
