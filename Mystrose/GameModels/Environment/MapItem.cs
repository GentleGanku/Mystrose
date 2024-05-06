using System.Text.Json.Serialization;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a Map Item in the game.
/// </summary>
public class MapItem
{

    #region Properties
    /// <summary>
    /// The ID of the map item.
    /// </summary>
    [JsonPropertyName("ID")]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the map item.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The quest ID for the map item.
    /// </summary>
    [JsonPropertyName("qID")]
    public int QuestID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The process message for the map item.
    /// </summary>
    [JsonPropertyName("itemName")]
    public string ProcessString
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The collect message for the map item.
    /// </summary>
    [JsonPropertyName("collectMsg")]
    public string CollectString
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the map item's name.
    /// </summary>
    /// <returns>
    /// A string representing the map item's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
