namespace Mystrose.Network.Messages;

/// <summary>
/// A base class that represents the Game Packet.
/// </summary>
public class Message
{
    #region Properties
    /// <summary>
    /// The message's entire content.
    /// </summary>
    /// <returns>
    /// A string representing the message's raw content, purely without any convertion attached.
    /// </returns>
    public string RawContent
    {
        get;
        set;
    }

    /// <summary>
    /// The message's main command.
    /// </summary>
    /// <returns>
    /// A string representing the message's command.
    /// </returns>
    public string Command
    {
        get;
        set;
    }
    #endregion
}
