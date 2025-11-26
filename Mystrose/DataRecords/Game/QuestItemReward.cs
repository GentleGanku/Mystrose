namespace Mystrose.DataRecords.Game;

/// <summary>
/// A base class that represents a quest item reward in the game.
/// </summary>
public class QuestItemReward : BaseItem
{

    #region Properties
    /// <summary>
    /// The rate at which this reward drops.
    /// </summary>
    [JsonPropertyName("iRate")]
    public double Rate
    {
        get;
        set;
    } = 1.0;

    /// <summary>
    /// The type of the item as an integer ID.
    /// </summary>
    [JsonPropertyName("iType")]
    public int Type
    {
        get;
        set;
    } = 0;
    #endregion

}
