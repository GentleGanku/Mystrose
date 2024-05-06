using Mystrose.GameModels.Base;
using Mystrose.Utilities.Converters;
using Mystrose.Utilities.Enumerations;
using System.Text.Json.Serialization;

namespace Mystrose.GameModels.General;

/// <summary>
/// A class that represents an inventory item in the game.
/// </summary>
public class InventoryItem : BaseItem
{

    #region Properties
    /// <summary>
    /// The item's inventory type; a purposed placement for the item to be put in.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the item's inventory type.
    /// </returns>
    public InventoryType InventoryType
    {
        get;
        set;
    } = InventoryType.Unknown;

    /// <summary>
    /// The ID of the item instance.
    /// </summary>
    /// <returns>
    /// An integer representing the item's character ID.
    /// </returns>
    [JsonPropertyName("CharItemID")]
    public int CharacterItemID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The item's tag of whether it is currently equipped. 
    /// </summary>
    /// <returns>
    /// A boolean representing the item's tag for Equipped state.
    /// </returns>
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
    public EnhancementType EnhancementType
    {
        get;
        set;
    } = EnhancementType.Unknown;
    #endregion

}
