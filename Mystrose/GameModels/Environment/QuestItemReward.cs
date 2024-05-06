using Mystrose.GameModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A base class that represents a quest item reward in the game.
/// </summary>
public class QuestItemReward : BaseItem
{

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
     
}
