namespace Mystrose.DataFormats.GameModels.Character;

/// <summary>
/// A class that represents an Effect Aura in the game.
/// </summary>
public class Aura : GameObject
{

    #region Constructor
    public Aura()
    {
        Refresh();
        RefreshProperties(this);
    }
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
    [JsonPropertyOrder(10)]
    public int Runtime
    {
        get => Duration - (DateTime.Now - _startingSpan).Seconds;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The condition of whether the aura is added to or removed from the dictionary.
    /// </summary>
    /// <returns>
    /// A boolean representing the aura's condition.
    /// </returns>
    [JsonPropertyOrder(0)]
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
    [JsonPropertyOrder(1)]
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
    [JsonPropertyOrder(2)]
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
    [JsonPropertyOrder(3)]
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
    [JsonPropertyOrder(4)]
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
    [JsonPropertyOrder(5)]
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
    [JsonPropertyOrder(6)]
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
    [JsonPropertyOrder(7)]
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
    [JsonPropertyOrder(8)]
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
    [JsonPropertyOrder(9)]
    public string TargetID
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods: Data
    public Aura SetHeader(string sourceData, string targetData)
    {
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
