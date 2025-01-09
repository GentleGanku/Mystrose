namespace Mystrose.DataFormats.GameModels.Character;

/// <summary>
/// A class that represents an active skill in the game.
/// </summary>
public class ActiveSkill : BaseSkill
{

    #region Constructor
    public ActiveSkill()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Private Fields
    private int _minTarget = 1;
    private int _maxTarget = 1;
    private bool _isLocked = false;
    #endregion

    #region Fields
    /// <summary>
    /// The condition of whether the skill can be safely used or not.
    /// </summary>
    public bool IsSafeToUse
    {
        get => IsUsable && !IsDisabled && !IsLocked;
    }

    /// <summary>
    /// The cooldown duration of the skill (in milliseconds).
    /// </summary>
    public int CooldownDuration
    {
        get
        {
            if (_lastCooldownTiming == DateTime.MinValue)
            {
                return 0;
            }

            return (int)(DateTime.Now - _lastCooldownTiming).TotalMilliseconds;
        }
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

    #region Methods: Override
    public override string ToString()
    {
        return $"[{ActionType}] {Name}";
    }
    #endregion

}
