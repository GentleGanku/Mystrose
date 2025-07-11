namespace Mystrose.DataFormats.GameModels.Base;

/// <summary>
/// A base class that represents a character item in the game.
/// </summary>
public class BaseItem : GameObject
{

    #region Constructor
    public BaseItem()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Fields
    /// <summary>
    /// The item's entire file path, consisting of the game's and the item's file path combined.
    /// </summary>
    /// <returns>
    /// A string representing the item's complete file path.
    /// </returns>
    public string File
    {
        get => Type switch
        {
            ItemType.Class or ItemType.Armor => $"https://www.aq.com/game/gamefiles/M/{FilePath} | https://www.aq.com/game/gamefiles/F/{FilePath}",
            _ => $"https://www.aq.com/game/gamefiles/{FilePath}"
        } ?? string.Empty;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The item's unique ID.
    /// </summary>
    /// <returns>
    /// An integer representing the item's ID.
    /// </returns>
    [JsonPropertyName("ItemID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The item's name.
    /// </summary>
    /// <returns>
    /// A string representing the item's name, in trimmed form.
    /// </returns>
    [JsonPropertyName("sName")]
    [JsonConverter(typeof(TrimConverter))]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The item's level.
    /// </summary>
    /// <returns>
    /// An integer representing the item's level.
    /// </returns>
    [JsonPropertyName("iLvl")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Level
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The item's description text.
    /// </summary>
    /// <returns>
    /// A string representing the item's description.
    /// </returns>
    [JsonPropertyName("sDesc")]
    public string Description
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The item's current quantity.
    /// </summary>
    /// <returns>
    /// An integer representing the item's quantity.
    /// </returns>
    [JsonPropertyName("iQty")]
    public int Quantity
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The item's maximum quantity size.
    /// </summary>
    /// <returns>
    /// An integer representing the item's maximum quantity.
    /// </returns>
    [JsonPropertyName("iStk")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxStack
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The item's tag of whether it is member-only.
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for Upgrade state.
    /// </returns>
    [JsonPropertyName("bUpg")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsMemberTagged
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The item's tag of whether it is coin-tagged (AdventureCoins).
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for Coin state.
    /// </returns>
    [JsonPropertyName("bCoins")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsCoinTagged
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The item's tag of whether it is temporary.
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for Temporary state.
    /// </returns>
    [JsonPropertyName("bTemp")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsTemporary
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The item's tag of whether it is a house item.
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for House Item state.
    /// </returns>
    [JsonPropertyName("bHouse")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsHouseItem
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The item's equipment type.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the item's equipment type.
    /// </returns>
    [JsonPropertyName("sES")]
    public EquipmentType EquipmentType
    {
        get;
        set;
    } = EquipmentType.Unknown;

    /// <summary>
    /// The item's category, that of an item type.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the item's type.
    /// </returns>
    [JsonPropertyName("sType")]
    public ItemType Type
    {
        get;
        set;
    } = ItemType.Unknown;

    /// <summary>
    /// The item's metadata that represents the in-game bonuses.
    /// </summary>
    /// <returns>
    /// A string representing the item's metadata, consisting of bonuses along with their own values.
    /// </returns>
    [JsonPropertyName("sMeta")]
    [JsonConverter(typeof(IntStringConverter))]
    public string Metadata
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The item's file path, consisting of only its unique path.
    /// </summary>
    /// <returns>
    /// A string representing the item's unique file path.
    /// </returns>
    [JsonPropertyName("sFile")]
    public string FilePath
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The item's file linkage.
    /// </summary>
    /// <returns>
    /// A string representing the item's file linkage.
    /// </returns>
    [JsonPropertyName("sLink")]
    [JsonConverter(typeof(IntStringConverter))]
    public string Linkage
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the item's name.
    /// </summary>
    /// <returns>
    /// A string representing the item's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
