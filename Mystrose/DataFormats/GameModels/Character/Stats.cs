namespace Mystrose.DataFormats.GameModels.Character;

/// <summary>
/// A class that represents a complete set of Stats on an Avatar in the game.
/// </summary>
public class Stats : GameObject
{

    #region Constructor
    public Stats()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Properties
    /// <summary>
    /// The avatar's haste stat.
    /// </summary>
    [JsonPropertyName("$tha")]
    [JsonConverter(typeof(StringDoubleConverter))]
    public double Haste
    {
        get;
        set;
    } = 0.1;
    #endregion

}
