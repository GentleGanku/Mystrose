namespace Mystrose.DataFormats.GameModels.Environment;

/// <summary>
/// A class that represents a quest item requirement in the game.
/// </summary>
public class QuestItemRequirement : GameObject
{

    #region Properties
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
    #endregion

}
