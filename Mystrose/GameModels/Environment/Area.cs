using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mystrose.Utilities.Converters;
using Mystrose.GameModels.General;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a map in the game.
/// </summary>
public class Area
{

    #region Private Fields
    private MapFormat _format = new();
    #endregion

    #region Fields
    /// <summary>
    /// The instance for the current map area.
    /// </summary>
    /// <returns>
    /// An integer representing the instance, which can be either a number or an identifier.
    /// </returns>
    public int Instance
    {
        get
        {
            if (Name.Contains('-') && int.TryParse(Name.Split('-')[1], out int instanceNumber))
            {
                return instanceNumber;
            }
            else
            {
                return 0;
            }
        }
    }
    #endregion

    #region Properties
    /// <summary>
    /// The map format of the current area instance.
    /// </summary>
    /// <returns>
    /// A map format representing the current area.
    /// </returns>
    public MapFormat Format
    {
        get => _format;
        set
        {
            _format = value;
            Monsters = new(value.Monsters);
        }
    }

    /// <summary>
    /// The area's ID for the current area instance.
    /// </summary>
    /// <returns>
    /// An integer representing the area's ID.
    /// </returns>
    [JsonPropertyName("areaId")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The area's name for the current area instance.
    /// </summary>
    /// <returns>
    /// A string representing the area's name.
    /// </returns>
    [JsonPropertyName("areaName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The list of Avatars that are currently residing in the current area.
    /// </summary>
    /// <returns>
    /// A list representing the Avatars.
    /// </returns>
    [JsonPropertyName("uoBranch")]
    public List<Avatar> Players
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of existing Monsters that resides in the map.
    /// </summary>
    /// <returns>
    /// A list representing the Monsters.
    /// </returns>
    public List<Monster> Monsters
    {
        get;
        set;
    } = [];
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the instance name.
    /// </summary>
    /// <returns>
    /// A string representing the instance name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}