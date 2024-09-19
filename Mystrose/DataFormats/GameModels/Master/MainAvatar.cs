namespace Mystrose.DataFormats.GameModels.Master;

/// <summary>
/// A class that represents the host's avatar in the game.
/// </summary>
public class MainAvatar : Avatar
{

    #region Constructor
    public MainAvatar()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The player's universal User ID.
    /// </summary>
    [JsonPropertyOrder(100)]
    [JsonPropertyName("UserID")]
    public int UserID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The amount of Gold the player has.
    /// </summary>
    [JsonPropertyOrder(101)]
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
    [JsonPropertyOrder(102)]
    [JsonPropertyName("intCoins")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Coins
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The list of costumes the player has.
    /// </summary>
    [JsonPropertyOrder(103)]
    [JsonPropertyName("costumes")]
    public Dictionary<string, Loadout> Costumes
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of loadouts the player has.
    /// </summary>
    [JsonPropertyOrder(104)]
    [JsonPropertyName("loadouts")]
    public Dictionary<string, Loadout> Loadouts
    {
        get;
        set;
    } = [];

    /// <summary>
    /// The list of stats the player has.
    /// </summary>
    [JsonPropertyOrder(105)]
    [JsonPropertyName("sta")]
    public Stats Stats
    {
        get;
        set;
    } = new();

    /// <summary>
    /// The player's total inventory slots.
    /// </summary>
    [JsonPropertyOrder(106)]
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
    [JsonPropertyOrder(107)]
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
    [JsonPropertyOrder(108)]
    [JsonPropertyName("iBankSlots")]
    [JsonConverter(typeof(StringIntConverter))]
    public int BankSlots
    {
        get;
        set;
    } = 0;
    #endregion

}
