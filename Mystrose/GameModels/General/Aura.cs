using Mystrose.Utilities.Converters;
using Mystrose.Utilities.Enumerations;
using System.Text.Json.Serialization;
using System;
using System.Threading.Tasks;
using Mystrose.GameModels.Base.Interfaces;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json;
using Mystrose.GameModels.Base;
using Mystrose.GameModels.Environment;

namespace Mystrose.GameModels.General;

/// <summary>
/// A class that represents an Effect Aura in the game.
/// </summary>
public class Aura : IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public Aura()
    {
        StackValue = 0;

        Refresh();
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

    #region Actions
    public event Action CountdownEvent;
    #endregion

    #region Private Fields
    private DateTime _startingSpan;
    #endregion

    #region Fields
    /// <summary>
    /// The aura's runtime, which is based on how long it currently lasts (the remaining duration).
    /// </summary>
    /// <returns>
    /// An integer representing the aura's runtime.
    /// </returns>
    public int Runtime
    {
        get => Duration - (DateTime.Now - _startingSpan).Seconds;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The dictionary that the aura is based from.
    /// </summary>
    /// <returns>
    /// A dictionary representing the aura's source.
    /// </returns>
    [JsonIgnore]
    public AuraDictionary? SourceDict
    {
        get;
        set;
    } = null;

    /// <summary>
    /// The condition of whether the aura is added to or removed from the dictionary.
    /// </summary>
    /// <returns>
    /// A boolean representing the aura's condition.
    /// </returns>
    public bool IsAdded
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The name of the effect aura.
    /// </summary>
    /// <returns>
    /// A string representing the aura's name, in trimmed form.
    /// </returns>
    [JsonPropertyName("nam")]
    [JsonConverter(typeof(TrimConverter))]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The value of the aura, in the form of an integer (stack value) or a string (target value).
    /// </summary>
    /// <returns>
    /// A string representing the aura's value.
    /// </returns>
    [JsonPropertyName("val")]
    [JsonConverter(typeof(IntStringConverter))]
    public string Value
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The amount of stacks the aura has been applied on, during its runtime.
    /// </summary>
    /// <returns>
    /// An integer representing the aura's unique stack value.
    /// </returns>
    public int StackValue
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The set duration of the aura.
    /// </summary>
    /// <returns>
    /// An integer representing the aura's duration.
    /// </returns>
    [JsonPropertyName("dur")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Duration
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The disable type of the aura.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the aura's disable type.
    /// </returns>
    [JsonPropertyName("cat")]
    public DisableType DisableType
    {
        get;
        set;
    } = DisableType.None;

    /// <summary>
    /// The source type that the aura is sent from.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the aura source's type
    /// </returns>
    public EntityType SourceType
    {
        get;
        set;
    } = EntityType.Unknown;

    /// <summary>
    /// The source ID that the aura is sent from.
    /// </summary>
    /// <returns>
    /// A string representing the aura source's ID.
    /// </returns>
    public string SourceID
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The target type that the aura is sent to.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the aura target's type.
    /// </returns>
    public EntityType TargetType
    {
        get;
        set;
    } = EntityType.Unknown;

    /// <summary>
    /// The target ID that the aura is sent to.
    /// </summary>
    /// <returns>
    /// A string representing the aura target's ID.
    /// </returns>
    public string TargetID
    {
        get;
        set;
    } = string.Empty;
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
            ["nam"] = GetType().GetProperty(nameof(Name)),
            ["val"] = GetType().GetProperty(nameof(Value)),
            ["dur"] = GetType().GetProperty(nameof(Duration)),
            ["cat"] = GetType().GetProperty(nameof(DisableType))
        };
    }
    #endregion

    #region Methods: Data
    public Aura SetHeader(AuraDictionary dict, string sourceData, string targetData)
    {
        SourceDict = dict;

        if (!string.IsNullOrEmpty(sourceData))
        {
            string[] sourceInfo = sourceData.Split(":");
            SourceType = JsonSerializer.Deserialize<EntityType>("\"" + sourceInfo[0] + "\"");
            SourceID = sourceInfo[1];
        }

        if (!string.IsNullOrEmpty(targetData))
        {
            string[] targetInfo = targetData.Split(":");
            TargetType = JsonSerializer.Deserialize<EntityType>("\"" + targetInfo[0] + "\"");
            TargetID = targetInfo[1];
        }

        return this;
    }

    public void RealignHeader(Area area)
    {
        if (SourceType is EntityType.Player)
        {
            Avatar? avatar = area.Players.Find(p => p.EntityID.ToString().Equals(SourceID));
            SourceID = avatar is not null ? avatar.Name : SourceID;
        }

        if (TargetType is EntityType.Player)
        {
            Avatar? avatar = area.Players.Find(p => p.EntityID.ToString().Equals(TargetID));
            TargetID = avatar is not null ? avatar.Name : TargetID;
        }
    }

    public void Refresh()
    {
        _startingSpan = DateTime.Now;
        StackValue++;

        CountdownEvent -= Countdown;
        CountdownEvent += Countdown;
    }
    #endregion

    #region Methods: Countdown
    public async void Countdown()
    {
        await Task.Delay(Duration + 000);

        Expire();
        CountdownEvent -= Countdown;
    }

    public void Expire()
    {
        SourceDict.Remove(this);
    }
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the aura's name.
    /// </summary>
    /// <returns>
    /// A string representing the aura's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
