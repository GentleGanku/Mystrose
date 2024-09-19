namespace Mystrose.Network.Messages;

public class XTMessage : Message
{

    #region Constructor
    public XTMessage(ClientUseIdentifier identifier, string raw) : base(identifier)
    {
        RawContent = raw;
        Arguments = raw.Split('%');

        if (Arguments.Length >= 4)
        {
            Command = Arguments[2] != "zm" ? Arguments[2] : (Arguments[3] == "cmd" ? Arguments[5] : Arguments[3]);
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
