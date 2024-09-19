namespace Mystrose.Network.Messages.Interfaces;

public interface IZMMessageHandler
{

    string[] HandledCommands
    {
        get;
    }

    void Handle(ZMMessage message);

}
