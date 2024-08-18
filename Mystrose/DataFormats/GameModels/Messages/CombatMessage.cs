namespace Mystrose.DataFormats.GameModels.Messages;

/// <summary>
/// A class that represents a combat message (fielding a yellow text) in the game.
/// </summary>
public class CombatMessage
{

    #region Fields
    /// <summary>
    /// The original data of the message source.
    /// </summary>
    [JsonPropertyName("cInf")]
    public string SourceData
    {
        set
        {
            if (value is null)
            {
                return;
            }

            string[] valueInfo = value.Split(":");

            SourceType = JsonSerializer.Deserialize<EntityType>("\"" + valueInfo[0] + "\"");
            SourceID = valueInfo[1];
        }
    }

    /// <summary>
    /// The original data of the message target.
    /// </summary>
    [JsonPropertyName("tInf")]
    public string TargetData
    {
        set
        {
            if (value is null)
            {
                return;
            }

            string[] valueInfo = value.Split(":");

            TargetType = JsonSerializer.Deserialize<EntityType>("\"" + valueInfo[0] + "\"");
            TargetID = valueInfo[1];
        }
    }
    #endregion

    #region Properties
    /// <summary>
    /// The power string of the message.
    /// </summary>
    [JsonPropertyName("animStr")]
    [JsonConverter(typeof(TrimConverter))]
    public string AnimationString
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The text of the message.
    /// </summary>
    [JsonPropertyName("msg")]
    [JsonConverter(typeof(TrimConverter))]
    public string Text
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The cell that the message is displayed from.
    /// </summary>
    [JsonPropertyName("strFrame")]
    public string Cell
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The source type of the message.
    /// </summary>
    public EntityType SourceType
    {
        get;
        set;
    } = EntityType.Unknown;

    /// <summary>
    /// The source ID of the message.
    /// </summary>
    public string SourceID
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The target type of the message.
    /// </summary>
    public EntityType TargetType
    {
        get;
        set;
    } = EntityType.Unknown;

    /// <summary>
    /// The target ID of the message.
    /// </summary>
    public string TargetID
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods: Data
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
    #endregion

}
