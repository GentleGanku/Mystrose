namespace Mystrose.DataFormats.GameModels.Character;

/// <summary>
/// A class that represents a monster in the game.
/// </summary>
public class Monster : GameObject
{

    #region Constructor
    public Monster()
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
    [JsonPropertyOrder(12)]
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
    [JsonPropertyOrder(13)]
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
    [JsonPropertyOrder(14)]
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
    [JsonPropertyOrder(0)]
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
    [JsonPropertyOrder(1)]
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
    [JsonPropertyOrder(2)]
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
    [JsonPropertyOrder(3)]
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
    [JsonPropertyOrder(4)]
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
    [JsonPropertyOrder(5)]
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
    [JsonPropertyOrder(6)]
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
    [JsonPropertyOrder(7)]
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
    [JsonPropertyOrder(8)]
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
    [JsonPropertyOrder(9)]
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
    [JsonPropertyOrder(10)]
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
    [JsonPropertyOrder(11)]
    [JsonPropertyName("bRed")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsAggressive
    {
        get;
        set;
    } = false;
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
        return $"{MonMapID}/{ID}";
    }
    #endregion

}
