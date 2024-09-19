namespace Mystrose.DataFormats.GameModels.Base;

public class BoostStatuses : GameObject
{

    #region Properties
    [JsonPropertyName("iBoostRep")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool ReputationBoost
    {
        get;
        private set;
    } = false;

    [JsonPropertyName("iBoostG")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool GoldBoost
    {
        get;
        private set;
    } = false;

    [JsonPropertyName("iBoostXP")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool ExperienceBoost
    {
        get;
        private set;
    } = false;

    [JsonPropertyName("iBoostCP")]
    [JsonConverter(typeof(StringBoolConverter))]
    public bool ClassBoost
    {
        get;
        private set;
    } = false;
    #endregion

    #region Methods
    public void SetBoost(string boostName, bool value)
    {
        switch (boostName)
        {
            case "xpboost":
                ExperienceBoost = value;
                break;
            case "gboost":
                GoldBoost = value;
                break;
            case "repboost":
                ReputationBoost = value;
                break;
            case "cpboost":
                ClassBoost = value;
                break;
        }
    }
    #endregion

}
