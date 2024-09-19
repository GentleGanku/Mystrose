namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Party Process Type in the game.
/// </summary>
public enum PartyProcessType
{
    /// <summary>
    /// The party is currently in the process of being invited.
    /// </summary>
    Inviting,

    /// <summary>
    /// The party is currently in the process of being declined.
    /// </summary>
    Declining,

    /// <summary>
    /// The party is currently in the process of being disbanded.
    /// </summary>
    Disbanding,

    /// <summary>
    /// The party is currently in the process of being joined.
    /// </summary>
    Joining,

    /// <summary>
    /// The party is currently in the process of being promoted.
    /// </summary>
    Promoting,

    /// <summary>
    /// The party is currently in the process of being removed.
    /// </summary>
    Removing,

    /// <summary>
    /// The party is currently in the process of being summoned.
    /// </summary>
    Summoning
}
