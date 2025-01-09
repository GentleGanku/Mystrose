namespace Mystrose.Network.Messages;

public class ZMMessage : Message
{

    #region Constructor
    public ZMMessage(ClientInstanceIdentifier identifier, string raw) : base(identifier)
    {
        RawContent = raw;
        Arguments = raw.Split('%');

        if (Arguments.Length >= 4)
        {
            Command = Arguments[3] == "cmd" ? Arguments[5] : Arguments[3];
        }
    }
    #endregion

    #region Properties
    public string[] Arguments
    {
        get;
        private set;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return string.Join("%", Arguments);
    }
    #endregion

}
