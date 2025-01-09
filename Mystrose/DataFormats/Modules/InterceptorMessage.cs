namespace Mystrose.DataFormats.Modules;

public struct InterceptorMessage
{

    #region Constructor
    public InterceptorMessage(int type, string command, string message)
    {
        Timeframe = DateTime.Now;
        Type = type;
        Command = command;
        Message = message;
    }
    #endregion

    #region Properties
    /// <example>
    /// 2000-12-01 14:50:50
    /// </example>
    public DateTime Timeframe
    {
        get;
        private set;
    }

    /// <example>
    /// 0 = JSON; 1 = XML; 2 = XT; 3 = ZM
    /// </example>
    public int Type
    {
        get;
        private set;
    }

    /// <example>
    /// moveToCell
    /// </example>
    public string Command
    {
        get;
        private set;
    }

    /// <example>
    /// Hello World
    /// </example>
    public string Message
    {
        get;
        private set;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return $"[{Timeframe:HH:mm:ss | dd MMM yy}] {Message.Replace("\n", "")}";
    }
    #endregion

}
