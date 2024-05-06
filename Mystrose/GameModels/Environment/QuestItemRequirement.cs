using System.Text.Json.Serialization;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a quest item requirement in the game.
/// </summary>
public class QuestItemRequirement
{

    [JsonPropertyName("ItemID")]
    public int ID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("iQty")]
    public int Quantity
    {
        get;
        set;
    } = 1;

}
