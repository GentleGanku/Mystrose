namespace Mystrose.DataFormats.GameModels.Environment;

/// <summary>
/// A class that represents an inventory item in the game.
/// </summary>
public class InventoryItem : BaseItem
{

    #region Constructor
    public InventoryItem()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The ID of the item instance.
    /// </summary>
    /// <returns>
    /// An integer representing the item's character ID.
    /// </returns>
    [JsonPropertyOrder(105)]
    [JsonPropertyName("CharItemID")]
    public float CharacterItemID
    {
        get;
        set;
    } = -1.0;

    /// <summary>
    /// The item's inventory type; a purposed placement for the item to be put in.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the item's inventory type.
    /// </returns>
    [JsonPropertyOrder(100)]
    public InventoryType InventoryType
    {
        get;
        set;
    } = InventoryType.Unknown;

    /// <summary>
    /// The item's tag of whether it is currently equipped. 
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for Equipped state.
    /// </returns>
    [JsonPropertyOrder(101)]
    [JsonPropertyName("bEquip")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsEquipped
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The item's enhancement level.
    /// </summary>
    /// <returns>
    /// An integer representing the item's enhancement level.
    /// </returns>
    [JsonPropertyOrder(102)]
    [JsonPropertyName("EnhLvl")]
    public int EnhancementLevel
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The item's enhancement pattern ID.
    /// </summary>
    /// <returns>
    /// An integer representing the item's enhancement pattern ID.
    /// </returns>
    [JsonPropertyOrder(103)]
    [JsonPropertyName("InvEnhPatternID")]
    public int EnhancementPatternID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The item's enhancement type.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the item's enhancement type.
    /// </returns>
    [JsonPropertyOrder(104)]
    public EnhancementType EnhancementType
    {
        get;
        set;
    } = EnhancementType.Unknown;
    #endregion

}
