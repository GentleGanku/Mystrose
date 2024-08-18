namespace Mystrose.DataFormats.GameModels.Base;

/// <summary>
/// A base class that represents a skill in the game.
/// </summary>
public class BaseSkill
{

    #region Properties
    /// <summary>
    /// The ID of the skill.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The type of action the skill acts as.
    /// </summary>
    [JsonPropertyName("ref")]
    public ActionType ActionType
    {
        get;
        set;
    } = ActionType.Unknown;

    /// <summary>
    /// The type of target the skill can be used on.
    /// </summary>
    [JsonPropertyName("tgt")]
    public TargetType TargetType
    {
        get;
        set;
    } = TargetType.Unknown;

    /// <summary>
    /// The name of the skill.
    /// </summary>
    [JsonPropertyName("nam")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The name of the skill.
    /// </summary>
    [JsonPropertyName("desc")]
    public string Description
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The amount of range the skill can be used with.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Range
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The condition of whether the skill can be used or not.
    /// </summary>
    [JsonPropertyName("isOK")]
    public bool IsUsable
    {
        get;
        set;
    } = true;
    #endregion

    #region Methods
    /// <summary>
    /// A method that returns the skill's name.
    /// </summary>
    /// <returns>
    /// A string representing the skill's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
