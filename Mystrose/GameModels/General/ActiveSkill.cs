using Mystrose.GameModels.Base;
using Mystrose.GameModels.Base.Interfaces;
using Mystrose.Utilities.Converters;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Mystrose.GameModels.General;

/// <summary>
/// A class that represents an active skill in the game.
/// </summary>
public class ActiveSkill : BaseSkill, IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public ActiveSkill() : base()
    {
        RefreshProperties();
    }
    #endregion

    #region Private Fields
    private int _minTarget = 1;
    private int _maxTarget = 1;
    private bool _isLocked = false;
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
    /// The condition of whether the skill can be safely used or not.
    /// </summary>
    public bool IsSafeToUse
    {
        get => IsUsable && !IsDisabled && !IsLocked;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The index of the skill.
    /// </summary>
    public int Index
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The action ID of the skill.
    /// </summary>
    [JsonPropertyName("actID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ActID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The amount of mana that the skill costs.
    /// </summary>
    [JsonPropertyName("mp")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ManaCost
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The cooldown of the skill (in milliseconds).
    /// </summary>
    [JsonPropertyName("cd")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Cooldown
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The base damage constant of the skill (multiplied with the other damage variables).
    /// </summary>
    [JsonPropertyName("damage")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double Damage
    {
        get;
        set;
    } = 0.1;

    /// <summary>
    /// The minimum number of targets the skill can be used on.
    /// </summary>
    [JsonPropertyName("tgtMin")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MinTarget
    {
        get => _minTarget;
        set
        {
            _minTarget = value;
        }
    }

    /// <summary>
    /// The maximum number of targets the skill can be used on.
    /// </summary>
    [JsonPropertyName("tgtMax")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxTarget
    {
        get => _maxTarget;
        set
        {
            _maxTarget = value;
        }
    }

    /// <summary>
    /// The condition of whether the skill is locked or not.
    /// </summary>
    public bool IsLocked
    {
        get => _isLocked;
        set
        {
            _isLocked = value;
        }
    }

    /// <summary>
    /// The condition of whether the skill can be used automatically.
    /// </summary>
    [JsonPropertyName("auto")]
    public bool IsAuto
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The condition of whether the skill is disabled or not.
    /// </summary>
    [JsonPropertyName("lock")]
    public bool IsDisabled
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
            ["id"] = GetType().GetProperty(nameof(ID)),
            ["ref"] = GetType().GetProperty(nameof(ActionType)),
            ["tgt"] = GetType().GetProperty(nameof(TargetType)),
            ["nam"] = GetType().GetProperty(nameof(Name)),
            ["desc"] = GetType().GetProperty(nameof(Description)),
            ["range"] = GetType().GetProperty(nameof(Range)),
            ["isOK"] = GetType().GetProperty(nameof(IsUsable)),
            ["actID"] = GetType().GetProperty(nameof(ActID)),
            ["mp"] = GetType().GetProperty(nameof(ManaCost)),
            ["cd"] = GetType().GetProperty(nameof(Cooldown)),
            ["damage"] = GetType().GetProperty(nameof(Damage)),
            ["tgtMin"] = GetType().GetProperty(nameof(MinTarget)),
            ["tgtMax"] = GetType().GetProperty(nameof(MaxTarget)),
            ["auto"] = GetType().GetProperty(nameof(IsAuto)),
            ["lock"] = GetType().GetProperty(nameof(IsDisabled))
        };
    }
    #endregion

    #region Methods: Override
    public override string ToString()
    {
        return $"[{ActionType}] {Name}";
    }
    #endregion

}
