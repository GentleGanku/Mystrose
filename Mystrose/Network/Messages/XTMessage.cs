using System;

namespace Mystrose.Network.Messages;

public class XTMessage : Message
{

    #region Constructor
    public XTMessage(string raw)
    {
        try
        {
            RawContent = raw;
            Arguments = raw.Split('%');
            if (Arguments.Length >= 4)
            {
                Command = Arguments[2] != "zm" ? Arguments[2] : (Arguments[3] == "cmd" ? Arguments[5] : Arguments[3]);
            }
        }
        catch (Exception e)
        {
            // WIP
        }
    }
    #endregion

    #region Destructor
    ~XTMessage()
    {
        RawContent = null;
        Arguments = null;
        Command = null;
    }
    #endregion

    #region Properties
    public string[] Arguments
    {
        get;
        private set;
    }
    #endregion

    #region Override Methods
    public override string ToString()
    {
        return string.Join("%", Arguments);
    }
    #endregion

}
