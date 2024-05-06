using Mystrose.GameModels.Base.Interfaces;
using Mystrose.Utilities.Converters;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Mystrose.GameModels.General;

/// <summary>
/// A class that represents a complete set of Stats on an Avatar in the game.
/// </summary>
public class Stats : IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public Stats()
    {
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

    #region Properties
    /// <summary>
    /// The avatar's haste stat.
    /// </summary>
    [JsonPropertyName("$tha")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double Haste
    {
        get;
        set;
    } = 0.1;
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
            ["$tha"] = GetType().GetProperty(nameof(Haste))
        };
    }
    #endregion

}
