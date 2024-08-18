namespace Mystrose.Network.Messages.Interfaces;

public interface IJSONMessageHandler
{
    string[] HandledCommands
    {
        get;
    }

    void Handle(GameHost host, JSONMessage message);
}
