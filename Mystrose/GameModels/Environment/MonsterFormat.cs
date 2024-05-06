using Mystrose.Utilities.Enumerations;
using System.Text.Json.Serialization;
using Mystrose.Utilities.Converters;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a monster in the game.
/// </summary>
public class MonsterFormat
{

    #region Fields
    /// <summary>
    /// The monster's entire file path, consisting of the game's and the monster's file path combined.
    /// </summary>
    /// <returns>
    /// A string representing the monster's complete file path.
    /// </returns>
    public string File
    {
        get => "https://game.aq.com/game/gamefiles/mon/" + (FilePath ?? string.Empty);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The ID of the monster.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's ID.
    /// </returns>
    [JsonPropertyName("MonID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the monster.
    /// </summary>
    /// <returns>
    /// A string representing the monster's name, in trimmed form.
    /// </returns>
    [JsonPropertyName("strMonName")]
    [JsonConverter(typeof(TrimConverter))]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The race of the monster.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the monster's race.
    /// </returns>
    [JsonPropertyName("sRace")]
    public RaceType RaceType
    {
        get;
        set;
    } = RaceType.None;

    /// <summary>
    /// The file path of the monster, consisting of only its unique path.
    /// </summary>
    /// <returns>
    /// A string representing the monster's unique file path.
    /// </returns>
    [JsonPropertyName("strMonFileName")]
    public string FilePath
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The file linkage of the monster.
    /// </summary>
    /// <returns>
    /// A string representing the monster's file linkage.
    /// </returns>
    [JsonPropertyName("strLinkage")]
    public string Linkage
    {
        get;
        set;
    } = string.Empty;
    #endregion

}
