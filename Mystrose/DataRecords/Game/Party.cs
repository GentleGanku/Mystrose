namespace Mystrose.DataRecords.Game;

/// <summary>
/// A class that represents a party in the game.
/// </summary>
public class Party : GameObject
{

    #region Properties
    /// <summary>
    /// The ID of the party.
    /// </summary>
    [JsonPropertyName("pid")]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The owner of the party.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The list of members in the party, including the owner (with lowercase names). 
    /// </summary>
    [JsonPropertyName("ul")]
    public List<string> Members
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The state of the party.
    /// </summary>
    public PartyProcessType Status
    {
        get;
        set;
    } = PartyProcessType.Inviting;
    #endregion

}
