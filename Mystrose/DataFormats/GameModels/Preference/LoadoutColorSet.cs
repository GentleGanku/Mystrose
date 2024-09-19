namespace Mystrose.DataFormats.GameModels.Preference;

public class LoadoutColorSet : GameObject
{

    #region Properties
    [JsonPropertyName("accessory")]
    public string AccessoryCode
    {
        get;
        set;
    } = "000000";

    [JsonPropertyName("base")]
    public string BaseCode
    {
        get;
        set;
    } = "000000";

    [JsonPropertyName("trim")]
    public string TrimCode
    {
        get;
        set;
    } = "000000";

    [JsonPropertyName("hair")]
    public string HairCode
    {
        get;
        set;
    } = "000000";

    [JsonPropertyName("skin")]
    public string SkinCode
    {
        get;
        set;
    } = "000000";

    [JsonPropertyName("eye")]
    public string EyeCode
    {
        get;
        set;
    } = "000000";
    #endregion

}
