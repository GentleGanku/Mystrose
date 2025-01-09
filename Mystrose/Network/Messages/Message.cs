namespace Mystrose.Network.Messages;

/// <summary>
/// A base class that represents a packet for in-game.
/// </summary>
public class Message
{

    #region Constructor
    public Message(ClientInstanceIdentifier identifier)
    {
        Identifier = identifier;
        HostWorld = MSVCWorld.Instance[identifier.Codename].Item2!;
    }
    #endregion

    #region Properties
    public ClientInstanceIdentifier Identifier
    {
        get;
        protected set;
    }

    public World HostWorld
    {
        get;
        protected set;
    }

    public string RawContent
    {
        get;
        protected set;
    }

    public string Command
    {
        get;
        protected set;
    }
    #endregion

}
