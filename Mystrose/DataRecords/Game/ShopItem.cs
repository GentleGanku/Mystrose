namespace Mystrose.DataRecords.Game;

/// <summary>
/// A class that represents a shop item in the game.
/// </summary>
public class ShopItem : BaseItem
{

    #region Properties
    /// <summary>
    /// The sale ID of the item in the shop.
    /// </summary>
    /// <returns>
    /// An integer representing the item's sale ID.
    /// </returns>
    [JsonPropertyName("ShopItemID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ShopItemID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The price that the item is sold for.
    /// </summary>
    /// <returns>
    /// An integer representing the item's cost.
    /// </returns>
    [JsonPropertyName("iCost")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Cost
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The required faction to buy the item.
    /// </summary>
    /// <returns>
    /// A string representing the required faction.
    /// </returns>
    [JsonPropertyName("sFaction")]
    public string Faction
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The required reputation points to buy the item.
    /// </summary>
    /// <returns>
    /// An integer representing the required reputation points.
    /// </returns>
    [JsonPropertyName("iReqRep")]
    [JsonConverter(typeof(StringIntConverter))]
    public int FactionPoints
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The required items to merge the item.
    /// </summary>
    /// <returns>
    /// A list representing the required items.
    /// </returns>
    [JsonPropertyName("turnin")]
    public List<BaseItem> TurninItems
    {
        get;
        set;
    } = [];
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the avatar's data.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's data.
    /// </returns>
    public override string ToString()
    {
        return $"[{ShopItemID}] {Name}";
    }
    #endregion

}
