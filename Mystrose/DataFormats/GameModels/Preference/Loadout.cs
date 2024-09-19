namespace Mystrose.DataFormats.GameModels.Preference;

public class Loadout : GameObject
{

    #region Properties
    [JsonPropertyName("colors")]
    public LoadoutColorSet ColorSet
    {
        get;
        set;
    } = new();

    [JsonPropertyName("Weapon")]
    public int WeaponID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("ar")]
    public int ClassID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("co")]
    public int ArmorID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("he")]
    public int HelmID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("ba")]
    public int CapeID
    {
        get;
        set;
    } = -1;

    [JsonPropertyName("pe")]
    public int PetID
    {
        get;
        set;
    } = -1;
    #endregion

}
