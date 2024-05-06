using System;

namespace Mystrose.Network.Messages;

public class ZMMessage : Message
{

    #region Constructor
    public ZMMessage(string raw)
    {
        try
        {
            RawContent = raw;
            Arguments = raw.Split('%');
            if (Arguments.Length >= 4)
            {
                Command = Arguments[3] == "cmd" ? Arguments[5] : Arguments[3];
            }
        }
        catch (Exception e)
        {
            // WIP
        }
    }
    #endregion

    #region Destructor
    ~ZMMessage()
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
