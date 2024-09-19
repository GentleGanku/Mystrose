namespace Mystrose.Network.Messages;

/// <summary>
/// A base class that represents a packet for in-game.
/// </summary>
public class Message
{

    #region Constructor
    public Message(ClientUseIdentifier identifier)
    {
        Identifier = identifier;
        Host = SVCGameManager.GetGameDict().Output[identifier.Codename]!;
        World = SVCWorldVisualizer.GetWorldDict().Output[identifier.Codename]!;
    }
    #endregion

    #region Properties
    public ClientUseIdentifier Identifier
    {
        get;
        set;
    }

    public HSTGame Host
    {
        get;
        set;
    }

    public World World
    {
        get;
        set;
    }

    public string RawContent
    {
        get;
        set;
    }

    public string Command
    {
        get;
        set;
    }
    #endregion

}
