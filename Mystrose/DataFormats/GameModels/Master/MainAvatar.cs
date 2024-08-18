namespace Mystrose.DataFormats.GameModels.Master;

/// <summary>
/// A class that represents the host's avatar in the game.
/// </summary>
public class MainAvatar : Avatar, IPropertyManager
{

    #region Constructor
    [JsonConstructor]
    public MainAvatar()
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
    /// The player's universal User ID.
    /// </summary>
    [JsonPropertyName("UserID")]
    public int UserID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The player's character ID.
    /// </summary>
    [JsonPropertyName("CharID")]
    public int CharID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The amount of Gold the player has.
    /// </summary>
    [JsonPropertyName("intGold")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Gold
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The amount of AdventureCoins the player has.
    /// </summary>
    [JsonPropertyName("intCoins")]
    [JsonConverter(typeof(StringIntConverter))]
    public int AdventureCoins
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The state of the player's reputation booster.
    /// </summary>
    [JsonPropertyName("iBoostRep")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool RepBoost
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The state of the player's gold booster.
    /// </summary>
    [JsonPropertyName("iBoostG")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool GoldBoost
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The state of the player's experience booster.
    /// </summary>
    [JsonPropertyName("iBoostXP")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool XPBoost
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The state of the player's class points booster.
    /// </summary>
    [JsonPropertyName("iBoostCP")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool CPBoost
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The list of costumes the player has.
    /// </summary>
    [JsonPropertyName("costumes")]
    public Dictionary<string, Loadout> Costumes
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of loadouts the player has.
    /// </summary>
    [JsonPropertyName("loadouts")]
    public Dictionary<string, Loadout> Loadouts
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of stats the player has.
    /// </summary>
    [JsonPropertyName("sta")]
    public Stats Stats
    {
        get;
        set;
    } = new();

    /// <summary>
    /// The list of active skills the player has.
    /// </summary>
    [JsonPropertyName("active")]
    public List<ActiveSkill> ActiveSkills
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of factions the player has.
    /// </summary>
    [JsonPropertyName("factions")]
    public List<Faction> Factions
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The player's total inventory slots.
    /// </summary>
    [JsonPropertyName("iBagSlots")]
    [JsonConverter(typeof(StringIntConverter))]
    public int InventorySlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's total house slots.
    /// </summary>
    [JsonPropertyName("iHouseSlots")]
    [JsonConverter(typeof(StringIntConverter))]
    public int HouseSlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's total bank slots.
    /// </summary>
    [JsonPropertyName("iBankSlots")]
    [JsonConverter(typeof(StringIntConverter))]
    public int BankSlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's used bank slots.
    /// </summary>
    [JsonPropertyName("bankCount")]
    [JsonConverter(typeof(StringIntConverter))]
    public int UsedBankSlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's activation flag.
    /// </summary>
    [JsonPropertyName("intActivationFlag")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ActivationFlag
    {
        get;
        set;
    } = 0;
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
            ["ty"] = GetType().GetProperty(nameof(Y)),
            ["UserID"] = GetType().GetProperty(nameof(UserID)),
            ["CharID"] = GetType().GetProperty(nameof(CharID)),
            ["intGold"] = GetType().GetProperty(nameof(Gold)),
            ["intCoins"] = GetType().GetProperty(nameof(AdventureCoins)),
            ["iBoostRep"] = GetType().GetProperty(nameof(RepBoost)),
            ["iBoostG"] = GetType().GetProperty(nameof(GoldBoost)),
            ["iBoostXP"] = GetType().GetProperty(nameof(XPBoost)),
            ["iBoostCP"] = GetType().GetProperty(nameof(CPBoost)),
            ["costumes"] = GetType().GetProperty(nameof(Costumes)),
            ["loadouts"] = GetType().GetProperty(nameof(Loadouts)),
            ["sta"] = GetType().GetProperty(nameof(Stats)),
            ["active"] = GetType().GetProperty(nameof(ActiveSkills)),
            ["factions"] = GetType().GetProperty(nameof(Factions)),
            ["iBagSlots"] = GetType().GetProperty(nameof(InventorySlots)),
            ["iHouseSlots"] = GetType().GetProperty(nameof(HouseSlots)),
            ["iBankSlots"] = GetType().GetProperty(nameof(BankSlots)),
            ["bankCount"] = GetType().GetProperty(nameof(UsedBankSlots)),
            ["intActivationFlag"] = GetType().GetProperty(nameof(ActivationFlag))
        };
    }
    #endregion

}
