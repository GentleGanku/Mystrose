using Mystrose.Utilities.Enumerations;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Mystrose.Utilities.Converters;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json;
using Mystrose.GameModels.Base.Interfaces;
using System;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a monster in the game.
/// </summary>
public class Monster : IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public Monster()
    {
        Targets = [];

        RefreshProperties();
    }
    #endregion

    #region Manager
    [JsonIgnore]
    public Dictionary<string, PropertyInfo> Properties
    {
        get;
        set;
    }
    #endregion

    #region Fields
    /// <summary>
    /// The percentage of the player's health.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's health percentage.
    /// </returns>
    public int HPPercentage
    {
        get => (int)Math.Round((double)HP / MaxHP * 100);
    }

    /// <summary>
    /// The percentage of the player's mana.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's mana percentage.
    /// </returns>
    public int MPPercentage
    {
        get => (int)Math.Round((double)MP / MaxMP * 100);
    }

    /// <summary>
    /// The condition of whether the avatar is currently alive or not.
    /// </summary>
    /// <returns>
    /// A boolean representing the avatar's life status.
    /// </returns>
    public bool IsAlive
    {
        get => HP > 0 && State > 0;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The ID of the monster in the game.
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
    /// The level of the monster.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's level.
    /// </returns>
    [JsonPropertyName("iLvl")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Level
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The map ID of the monster.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's map ID.
    /// </returns>
    [JsonPropertyName("MonMapID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MonMapID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The cell where the monster is currently located.
    /// </summary>
    /// <returns>
    /// An object representing the monster's cell.
    /// </returns>
    [JsonPropertyName("strFrame")]
    public string Cell
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The current state of the monster.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the monster's state.
    /// </returns>
    [JsonPropertyName("intState")]
    public StateType State
    {
        get;
        set;
    } = StateType.Idle;

    /// <summary>
    /// The list of targets that the monster is currently in combat with.
    /// </summary>
    /// <returns>
    /// A list representing the Avatar Names.
    /// </returns>
    public List<string> Targets
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The maximum HP that the monster can have.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's maximum HP.
    /// </returns>
    [JsonPropertyName("intHPMax")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double MaxHP
    {
        get;
        set;
    } = 0.1;

    /// <summary>
    /// The current HP that the monster has.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's current HP.
    /// </returns>
    [JsonPropertyName("intHP")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double HP
    {
        get;
        set;
    } = 0.1;

    /// <summary>
    /// The maximum MP that the monster can have.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's maximum MP.
    /// </returns>
    [JsonPropertyName("intMPMax")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxMP
    {
        get;
        set;
    } = 1;
     
    /// <summary>
    /// The current MP that the monster has.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's current MP.
    /// </returns>
    [JsonPropertyName("intMP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MP
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The DPS range that the monster can deal.
    /// </summary>
    /// <returns>
    /// An integer representing the monster's DPS range.
    /// </returns>
    [JsonPropertyName("wDPS")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double DPS
    {
        get;
        set;
    } = 0.1;

    /// <summary>
    /// The monster's tag of whether it is aggressive or not.
    /// </summary>
    /// <returns>
    /// A boolean representing the monster's tag for Aggressiveness state.
    /// </returns>
    [JsonPropertyName("bRed")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsAggressive
    {
        get;
        set;
    } = false;
    #endregion

    #region Methods: Properties
    /// <summary>
    /// A method that gets a property's value based on their property name in-game.
    /// </summary>
    public PropertyInfo? GetProperty(string key)
    {
        if (!Properties.TryGetValue(key, out PropertyInfo? value))
        {
            return null;
        }

        return value;
    }

    /// <summary>
    /// A method that sets a property's value based on their property name in-game.
    /// </summary>
    public void SetProperty(string key, JsonNode node)
    {
        PropertyInfo? propInfo = GetProperty(key);
        if (propInfo is null)
        {
            return;
        }

        JsonSerializerOptions options = new()
        {
            Converters =
            {
                new StringIntConverter(), 
                new StringDoubleConverter(), 
                new StringBoolConverter()
            }
        };

        Properties[key].SetValue(this, node.Deserialize(propInfo.PropertyType, options));
    }

    /// <summary>
    /// A method that sets all predefined properties in the instance of this class.
    /// </summary>
    public void SetProperties(JsonObject jsonObj)
    {
        foreach (KeyValuePair<string, JsonNode> jsonProp in jsonObj)
        {
            SetProperty(jsonProp.Key, jsonProp.Value);
        }
    }

    /// <summary>
    /// A method that refreshes all properties in the instance of this class.
    /// </summary>
    public void RefreshProperties()
    {
        Properties = new()
        {
            ["MonID"] = GetType().GetProperty(nameof(ID)),
            ["iLvl"] = GetType().GetProperty(nameof(Level)),
            ["MonMapID"] = GetType().GetProperty(nameof(MonMapID)),
            ["strFrame"] = GetType().GetProperty(nameof(Cell)),
            ["intState"] = GetType().GetProperty(nameof(State)),
            ["intHPMax"] = GetType().GetProperty(nameof(MaxHP)),
            ["intHP"] = GetType().GetProperty(nameof(HP)),
            ["intMPMax"] = GetType().GetProperty(nameof(MaxMP)),
            ["intMP"] = GetType().GetProperty(nameof(MP)),
            ["wDPS"] = GetType().GetProperty(nameof(DPS)),
            ["bRed"] = GetType().GetProperty(nameof(IsAggressive))
        };
    }
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the monster's name.
    /// </summary>
    /// <returns>
    /// A string representing the monster's name.
    /// </returns>
    public override string ToString()
    {
        return $"{MonMapID} - {ID}";
    }
    #endregion

}
