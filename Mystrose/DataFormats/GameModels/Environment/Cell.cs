namespace Mystrose.DataFormats.GameModels.Environment;

/// <summary>
/// A class that represents a Map Cell in the game.
/// </summary>
public class Cell : GameObject
{

    #region Constructor
    public Cell()
    {
        // Empty
    }
    #endregion

    #region Properties
    /// <summary>
    /// The name of the cell.
    /// </summary>
    /// <returns>
    /// A string representing the cell's name.
    /// </returns>
    [JsonPropertyName("name")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The list of pads that exists in the cell.
    /// </summary>
    /// <returns>
    /// A list of strings representing the cell's pads.
    /// </returns>
    [JsonPropertyName("pads")]
    public List<string> Pads
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of Map Items that exists in the cell.
    /// </summary>
    /// <returns>
    /// A list representing the Map Items.
    /// </returns>
    [JsonPropertyName("mapItems")]
    public List<MapItem> MapItems
    {
        get;
        set;
    } = [];
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the cell's name.
    /// </summary>
    /// <returns>
    /// A string representing the cell's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
