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
    [JsonPropertyName("UserID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int UserID
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
    public int Coins
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The list of costumes the player has.
    /// </summary
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
    /// The player's total inventory slots.
    /// </summary>
    [JsonPropertyName("iBagSlots")]
    public int InventorySlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's total house slots.
    /// </summary>
    [JsonPropertyName("iHouseSlots")]
    public int HouseSlots
    {
        get;
        set;
    } = 0;

    /// <summary>
    /// The player's total bank slots.
    /// </summary>
    [JsonPropertyName("iBankSlots")]
    public int BankSlots
    {
        get;
        set;
    } = 0;
    #endregion

}
