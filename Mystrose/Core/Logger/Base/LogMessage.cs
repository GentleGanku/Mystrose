namespace Mystrose.Core.Logger.Base;

public struct LogMessage
{

    #region Constructor
    public LogMessage(int type, string message)
    {
        Timeframe = DateTime.Now;
        Type = type;
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
    /// 0 = Debug; 1 = Script; 2 = Exception
    /// </example>
    public int Type
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
        return $"[{Timeframe:HH:mm:ss | dd MMM yy}] {Message}";
    }
    #endregion

}
