namespace Mystrose.DataFormats.GameModels.Character;

/// <summary>
/// A base class that represents a character's avatar in the game.
/// </summary>
public class Avatar : GameObject
{

    #region Constructor
    public Avatar()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Fields
    /// <summary>
    /// The percentage of the player's health.
    /// </summary>
    /// <returns>
    /// An integer representing the avatar's health percentage.
    /// </returns>
    [JsonPropertyOrder(25)]
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
    [JsonPropertyOrder(26)]
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
    [JsonPropertyOrder(27)]
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
    [JsonPropertyOrder(28)]
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
    [JsonPropertyOrder(29)]
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
    [JsonPropertyOrder(30)]
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
    [JsonPropertyOrder(31)]
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
    [JsonPropertyOrder(0)]
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
    [JsonPropertyOrder(1)]
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
    [JsonPropertyOrder(2)]
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
    [JsonPropertyOrder(3)]
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
    [JsonPropertyOrder(4)]
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
    [JsonPropertyOrder(5)]
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
    [JsonPropertyOrder(6)]
    [JsonPropertyName("strClassName")]
    public string Class
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The rank of the player's class (in Class Points).
    /// </summary>
    [JsonPropertyOrder(7)]
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
    [JsonPropertyOrder(8)]
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
    [JsonPropertyOrder(9)]
    public Dictionary<string, BaseItem?> VanityEquipments
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
    [JsonPropertyOrder(10)]
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
    [JsonPropertyOrder(11)]
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
    [JsonPropertyOrder(12)]
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
    [JsonPropertyOrder(13)]
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
    [JsonPropertyOrder(14)]
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
    [JsonPropertyOrder(15)]
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
    [JsonPropertyOrder(16)]
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
    [JsonPropertyOrder(17)]
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
    [JsonPropertyOrder(18)]
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
    [JsonPropertyOrder(19)]
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
    [JsonPropertyOrder(20)]
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
    [JsonPropertyOrder(21)]
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
    [JsonPropertyOrder(22)]
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
    [JsonPropertyOrder(23)]
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
    [JsonPropertyOrder(24)]
    [JsonPropertyName("ty")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double Y
    {
        get;
        set;
    } = 0.1;
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
