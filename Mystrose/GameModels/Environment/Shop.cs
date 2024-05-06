using Mystrose.Utilities.Converters;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A base class that represents a shop in the game.
/// </summary>
public class Shop
{

    #region Fields
    /// <summary>
    /// The list of locations that allows you to access the shop from.
    /// </summary>
    /// <returns>
    /// A list representing the locations, in string form.
    /// </returns>
    [JsonIgnore]
    public List<string> Locations
    {
        get => [.. LocationString.Split(',') ?? []];
    }
    #endregion

    #region Properties
    /// <summary>
    /// The ID of the shop.
    /// </summary>
    /// <returns>
    /// An integer representing the shop's ID.
    /// </returns>
    [JsonPropertyName("ShopID")]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the shop.
    /// </summary>
    /// <returns>
    /// A string representing the shop's name, in trimmed form.
    /// </returns>
    [JsonPropertyName("sName")]
    [JsonConverter(typeof(TrimConverter))]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The location string of the shop, split by commas.
    /// </summary>
    /// <returns>
    /// A string representing the shop's location string.
    /// </returns>
    [JsonPropertyName("Location")]
    public string LocationString
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The list of items that the shop sells.
    /// </summary>
    /// <returns>
    /// A list representing the shop items.
    /// </returns>
    [JsonPropertyName("items")]
    public List<ShopItem> Items
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The type field of the shop.
    /// </summary>
    /// <returns>
    /// A string representing the shop's field.
    /// </returns>
    [JsonPropertyName("sField")]
    [JsonConverter(typeof(IntStringConverter))]
    public string Field
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The index of the shop.
    /// </summary>
    /// <returns>
    /// An integer representing the shop's index.
    /// </returns>
    [JsonPropertyName("iIndex")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Index
    {
        get;
        set;
    } = -1;
     
    /// <summary>
    /// The shop's tag of whether it is member-only.
    /// </summary>
    /// <returns>
    /// A boolean representing the shop's tag for Upgrade state.
    /// </returns>
    [JsonPropertyName("bUpgrd")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsMemberOnly
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The shop's tag of whether it is staff-only.
    /// </summary>
    /// <returns>
    /// A boolean representing the shop's tag for Staff state.
    /// </returns>
    [JsonPropertyName("bStaff")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsStaffOnly
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The shop's tag of whether it is of a house type.
    /// </summary>
    /// <returns>
    /// A boolean representing the shop's tag for House state.
    /// </returns>
    [JsonPropertyName("bHouse")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsHouseShop
    {
        get;
        set;
    } = false;
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the shop's name.
    /// </summary>
    /// <returns>
    /// A string representing the shop's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
