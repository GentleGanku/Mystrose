namespace Mystrose.DataRecords.Game;

/// <summary>
/// A class that represents a map format in the game.
/// </summary>
public class MapFormat : GameObject
{

    #region Private Fields
    private List<MonsterFormat> _monsterFormats = [];
    #endregion

    #region Fields
    /// <summary>
    /// The map's entire file path, consisting of the game's and the item's file path combined.
    /// </summary>
    /// <returns>
    /// A string representing the map's entire file path.
    /// </returns>
    public string File
    {
        get => "https://game.aq.com/game/gamefiles/maps/" + (FilePath ?? string.Empty);
    }

    /// <summary>
    /// The list of cells that are safe to be resided in. This list excludes the Wait and Blank Cells.
    /// </summary>
    /// <returns>
    /// A list representing the safe cells. If no safe cells are found, the list will default to the Enter Cell.
    /// </returns>
    public List<Cell> SafeCells
    {
        get => Cells.Where(cell => cell.Name != "Wait" && cell.Name != "Blank" && !Monsters.Any(m => m.Cell.Equals(cell))).ToList() ?? [Cells.Find(cell => cell.Name.Equals("Enter"))];
    }

    /// <summary>
    /// The list of map items that are present in the map.
    /// </summary>
    /// <returns>
    /// A list representing the map items.
    /// </returns>
    public List<MapItem> MapItems
    {
        get => Cells.SelectMany(cell => cell.MapItems).ToList() ?? [];
    }
    #endregion

    #region Properties
    /// <summary>
    /// The map's name.
    /// </summary>
    /// <returns>
    /// A string representing the map's name, in lowercase form.
    /// </returns>
    [JsonPropertyName("strMapName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The map's file path, consisting of only its unique path.
    /// </summary>
    /// <returns>
    /// A string representing the map's unique file path.
    /// </returns>
    [JsonPropertyName("strMapFileName")]
    public string FilePath
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The map's type.
    /// </summary>
    /// <returns>
    /// An integer representing the map's typical type.
    /// </returns>
    [JsonPropertyName("intType")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Type
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The list of Monsters that resides in the map.
    /// </summary>
    /// <returns>
    /// A list representing the Monsters.
    /// </returns>
    [JsonPropertyName("mondef")]
    public List<MonsterFormat> MonsterFormats
    {
        get => _monsterFormats;
        set
        {
            _monsterFormats = value.DistinctBy(m => m.ID).ToList();
        }
    }

    /// <summary>
    /// The list of Monsters that resides in the map.
    /// </summary>
    /// <returns>
    /// A list representing the Monsters.
    /// </returns>
    [JsonPropertyName("monBranch")]
    public List<Monster> Monsters
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of Cells that exists in the map.
    /// </summary>
    /// <returns>
    /// A list representing the Cells.
    /// </returns>
    public List<Cell> Cells
    {
        get;
        set;
    } = [];
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the map's name.
    /// </summary>
    /// <returns>
    /// A string representing the map's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
