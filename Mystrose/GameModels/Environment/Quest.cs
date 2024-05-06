using Mystrose.Utilities.Converters;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Warthorn.Utilities.Converters;
using Mystrose.Utilities.Enumerations;
using Mystrose.GameModels.Base;

namespace Mystrose.GameModels.Environment;

/// <summary>
/// A class that represents a quest in the game.
/// </summary>
public class Quest
{

    #region Properties
    /// <summary>
    /// The ID of the quest.
    /// </summary>
    [JsonPropertyName("QuestID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The condition of whether the quest is one-time only or not.
    /// </summary>
    [JsonPropertyName("bOnce")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsOneTime
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The current status of the quest.
    /// </summary>
    [JsonPropertyName("status")]
    public QuestStatusType StatusType
    {
        get;
        set;
    } = QuestStatusType.Inactive;

    /// <summary>
    /// The name of the quest.
    /// </summary>
    [JsonPropertyName("sName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The description of the quest.
    /// </summary>
    [JsonPropertyName("sDesc")]
    public string Description
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The fulfilled description of the quest.
    /// </summary>
    [JsonPropertyName("sEndText")]
    public string FulfilledDescription
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The chain slot of the quest in the game.
    /// </summary>
    [JsonPropertyName("iSlot")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ChainSlot
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The index of the quest that is connected with the chain slot.
    /// </summary>
    [JsonPropertyName("iValue")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ChainIndex
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The ID of the faction that the quest is connected with.
    /// </summary>
    [JsonPropertyName("FactionID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int FactionID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the faction that the quest is connected with.
    /// </summary>
    [JsonPropertyName("sFaction")]
    public string FactionName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The ID of the class that the quest is connected with.
    /// </summary>
    [JsonPropertyName("iClass")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ClassID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the class that the quest is connected with.
    /// </summary>
    [JsonPropertyName("sClass")]
    public string ClassName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The ID of the war that the quest is connected with.
    /// </summary>
    [JsonPropertyName("iWar")]
    [JsonConverter(typeof(StringIntConverter))]
    public int WarID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The list of items that the quest requires for it to be accepted.
    /// </summary>
    //[JsonPropertyName("oReqd")] TODO: fix
    [JsonConverter(typeof(DictionaryListConverter<int, BaseItem>))]
    public List<BaseItem> AcceptItems
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of items that the quest requires for it to be completed.
    /// </summary>
    //[JsonPropertyName("oItems")] TODO: fix
    [JsonConverter(typeof(DictionaryListConverter<int, BaseItem>))]
    public List<BaseItem> TurninItems
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of items that the quest gives after completion.
    /// </summary>
    //[JsonPropertyName("oRewards")] TODO: fix
    [JsonConverter(typeof(QuestRewardConverter))]
    public List<BaseItem> RewardItems
    {
        get;
        set;
    } = [];
    #endregion

    #region Restrictions
    /// <summary>
    /// The minimum level required to accept the quest.
    /// </summary>
    [JsonPropertyName("iLvl")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RequiredLevel
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// The minimum reputation points required to accept the quest.
    /// </summary>
    [JsonPropertyName("iReqRep")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RequiredRepPoints
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The minimum class points required to accept the quest.
    /// </summary>
    [JsonPropertyName("iReqCP")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RequiredClassPoints
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The condition of whether the quest is for members or not.
    /// </summary>
    [JsonPropertyName("bUpg")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsMemberOnly
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The condition of whether the quest is for guild members or not.
    /// </summary>
    [JsonPropertyName("bGuild")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsGuildOnly
    {
        get;
        set;
    } = false;

    /// <summary>
    /// The condition of whether the quest is for staff members or not.
    /// </summary>
    [JsonPropertyName("bStaff")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool IsStaffOnly
    {
        get;
        set;
    } = false;
    #endregion

    #region Rewards
    /// <summary>
    /// The amount of gold that the quest gives.
    /// </summary>
    [JsonPropertyName("iGold")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RewardGold
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// [Reward] The amount of adventure coins that the quest gives.
    /// </summary>
    [JsonPropertyName("iAc")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RewardCoins
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The amount of experience that the quest gives.
    /// </summary>
    [JsonPropertyName("iExp")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RewardExp
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The amount of reputation points that the quest gives.
    /// </summary>
    [JsonPropertyName("iRep")]
    [JsonConverter(typeof(StringIntConverter))]
    public int RewardRep
    {
        get;
        set;
    } = 0;
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the quest's name.
    /// </summary>
    /// <returns>
    /// A string representing the quest's name.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
    #endregion

}
