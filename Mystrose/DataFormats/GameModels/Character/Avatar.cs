

/// <summary>
/// A base class that represents a character's avatar in the game.
/// </summary>
namespace Mystrose.DataFormats.GameModels.Character;

public class Avatar : IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public Avatar()
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
    /// The rank of the player's class.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's class rank.
    /// </returns>
    public int ClassRank
    {
        get => GetClassRank();
    }

    /// <summary>
    /// The avatar's tag of whether it is a Member.
    /// </summary>
    /// <returns>
    /// A boolean representing the avatar's tag for Upgrade state.
    /// </returns>
    public bool IsMember
    {
        get => MemberDays >= 0;
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

    /// <summary>
    /// The condition of whether the avatar is currently out of mana or not.
    /// </summary>
    /// <returns>
    /// A boolean representing the avatar's OOM status.
    /// </returns>
    public bool IsOOM
    {
        get => MP <= 0;
    }

    /// <summary>
    /// The avatar's current position, which is based on its current X and Y coordinates.
    /// </summary>
    /// <returns>
    /// An array of float representing the avatar's position, with X and Y coordinates respectively.
    /// </returns>
    public double[] Position
    {
        get => [X, Y];
    }
    #endregion

    #region Properties
    /// <summary>
    /// The avatar's entity ID.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's entity ID.
    /// </returns>
    [JsonPropertyName("entID")]
    public int EntityID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The avatar's name, in lowercase form.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's name, in lowercase form.
    /// </returns>
    [JsonPropertyName("uoName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The avatar's name.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's name.
    /// </returns>
    [JsonPropertyName("strUsername")]
    public string Username
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The avatar's gender.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the avatar's gender type.
    /// </returns>
    [JsonPropertyName("strGender")]
    public GenderType Gender
    {
        get;
        set;
    } = GenderType.Unknown;

    /// <summary>
    /// The avatar's access type, which is based on its access level.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the avatar's access type.
    /// </returns>
    [JsonPropertyName("intAccessLevel")]
    public AccessType AccessType
    {
        get;
        set;
    } = AccessType.Player;

    /// <summary>
    /// The avatar's current level.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's level.
    /// </returns>
    [JsonPropertyName("intLevel")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Level
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The avatar's class name.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's class name.
    /// </returns>
    [JsonPropertyName("strClassName")]
    public string Class
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The rank of the player's class (in Class Points).
    /// </summary>
    [JsonPropertyName("iCP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ClassPoints
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's current equipments.
    /// </summary>
    /// <returns>
    /// A dictionary of equipment types representing the avatar's current equipment items.
    /// </returns>
    [JsonPropertyName("eqp")]
    public Dictionary<string, BaseItem?> Equipments
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The avatar's current cosmetic equipments.
    /// </summary>
    /// <returns>
    /// A dictionary of equipment types representing the avatar's current cosmetic equipment items.
    /// </returns>
    public Dictionary<string, BaseItem?> CosmeticEquipments
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The avatar's tag of whether it is currently AFK (Away From Keyboard).
    /// </summary>
    /// <returns>
    /// A boolean representing the avatar's tag for AFK state.
    /// </returns>
    [JsonPropertyName("afk")]
    public bool IsAFK
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The avatar's tag of whether it is currently resting.
    /// </summary>
    /// <returns>
    /// A boolean representing the avatar's tag for Resting state.
    /// </returns>
    public bool IsResting
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The avatar's member days, which is based on how long the Member state currently lasts for (in days).
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's member days.
    /// </returns>
    [JsonPropertyName("iUpgDays")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MemberDays
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The avatar's maximum HP (Health Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's maximum HP.
    /// </returns>
    [JsonPropertyName("intHPMax")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxHP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's current HP (Health Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's current HP.
    /// </returns>
    [JsonPropertyName("intHP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int HP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's maximum MP (Mana Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's maximum MP.
    /// </returns>
    [JsonPropertyName("intMPMax")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxMP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's current MP (Mana Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's current MP.
    /// </returns>
    [JsonPropertyName("intMP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's maximum SP (Stamina Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's maximum SP.
    /// </returns>
    [JsonPropertyName("intSPMax")]
    [JsonConverter(typeof(StringIntConverter))]
    public int MaxSP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's current SP (Stamina Points).
    /// </summary>
    /// <returns>
    /// A double representing the avatar's current SP.
    /// </returns>
    [JsonPropertyName("intSP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int SP
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The avatar's current state.
    /// </summary>
    /// <returns>
    /// An enumeration type representing the avatar's state.
    /// </returns>
    [JsonPropertyName("intState")]
    public StateType State
    {
        get;
        set;
    } = StateType.Idle;

    /// <summary>
    /// The list of Monsters that are currently targeted by the avatar.
    /// </summary>
    /// <returns>
    /// A list representing the Monster IDs.
    /// </returns>
    public List<int> Targets
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The cell that the avatar is currently residing in.
    /// </summary>
    /// <returns>
    /// An object representing the avatar's cell.
    /// </returns>
    [JsonPropertyName("strFrame")]
    public string Cell
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The pad that the avatar is currently residing in.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's pad.
    /// </returns>
    [JsonPropertyName("strPad")]
    public string Pad
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The avatar's current X coordinate.
    /// </summary>
    /// <returns>
    /// An object representing the avatar's X coordinate.
    /// </returns>
    [JsonPropertyName("tx")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double X
    {
        get;
        set;
    } = 0.1;

    /// <summary>
    /// The avatar's current Y coordinate.
    /// </summary>
    /// <returns>
    /// An object representing the avatar's Y coordinate.
    /// </returns>
    [JsonPropertyName("ty")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double Y
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
            ["entID"] = GetType().GetProperty(nameof(EntityID)),
            ["uoName"] = GetType().GetProperty(nameof(Name)),
            ["strUsername"] = GetType().GetProperty(nameof(Username)),
            ["strGender"] = GetType().GetProperty(nameof(Gender)),
            ["intAccessLevel"] = GetType().GetProperty(nameof(AccessType)),
            ["intLevel"] = GetType().GetProperty(nameof(Level)),
            ["strClassName"] = GetType().GetProperty(nameof(Class)),
            ["iCP"] = GetType().GetProperty(nameof(ClassPoints)),
            ["eqp"] = GetType().GetProperty(nameof(Equipments)),
            ["afk"] = GetType().GetProperty(nameof(IsAFK)),
            ["iUpgDays"] = GetType().GetProperty(nameof(MemberDays)),
            ["intHPMax"] = GetType().GetProperty(nameof(MaxHP)),
            ["intHP"] = GetType().GetProperty(nameof(HP)),
            ["intMPMax"] = GetType().GetProperty(nameof(MaxMP)),
            ["intMP"] = GetType().GetProperty(nameof(MP)),
            ["intSPMax"] = GetType().GetProperty(nameof(MaxSP)),
            ["intSP"] = GetType().GetProperty(nameof(SP)),
            ["intState"] = GetType().GetProperty(nameof(State)),
            ["strFrame"] = GetType().GetProperty(nameof(Cell)),
            ["strPad"] = GetType().GetProperty(nameof(Pad)),
            ["tx"] = GetType().GetProperty(nameof(X)),
            ["ty"] = GetType().GetProperty(nameof(Y))
        };
    }
    #endregion

    #region Methods: Data
    /// <summary>
    /// A method that returns a rank from calculating total points.
    /// </summary>
    /// <returns>
    /// An integer representing the rank.
    /// </returns>
    private int GetClassRank()
    {
        return ClassPoints switch
        {
            >= 302500 => 10,
            >= 202500 => 9,
            >= 129600 => 8,
            >= 78400 => 7,
            >= 44100 => 6,
            >= 22500 => 5,
            >= 10000 => 4,
            >= 3600 => 3,
            >= 900 => 2,
            _ => 1
        };
    }
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the avatar's data.
    /// </summary>
    /// <returns>
    /// A string representing the avatar's data.
    /// </returns>
    public override string ToString()
    {
        return $"{Username}, {Class} (Rank {GetClassRank()}";
    }
    #endregion

}
