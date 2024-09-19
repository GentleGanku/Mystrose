namespace Mystrose.DataFormats.GameModels.Environment;

/// <summary>
/// A class that represents a Faction in the game.
/// </summary>
public class Faction : GameObject
{

    #region Constructor
    public Faction()
    {
        RefreshProperties(this);
    }
    #endregion

    #region Fields
    /// <summary>
    /// The current Reputation rank of the faction.
    /// </summary>
    /// <returns>
    /// An integer representing the faction's current rank.
    /// </returns>
    [JsonPropertyOrder(3)]
    public int Rank
    {
        get => GetRank();
    }

    /// <summary>
    /// The amount of Reputation Points that the faction needed to reach the next rank.
    /// </summary>
    /// <returns>
    /// An integer representing the faction's next required Reputation Points.
    /// </returns>
    [JsonPropertyOrder(4)]
    public int RequiredRankPoints
    {
        get => GetNextTotalPoints() - Points;
    }

    /// <summary>
    /// The amount of Reputation Points that the faction needed to reach Rank 10.
    /// </summary>
    /// <returns>
    /// An integer representing the faction's required Reputation Points at max.
    /// </returns>
    [JsonPropertyOrder(5)]
    public int RequiredMaxPoints
    {
        get => 302500 - Points;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The ID of the faction.
    /// </summary>
    /// <returns>
    /// An integer representing the faction's ID.
    /// </returns>
    [JsonPropertyOrder(0)]
    [JsonPropertyName("FactionID")]
    [JsonConverter(typeof(StringIntConverter))]
    public int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    /// The name of the faction.
    /// </summary>
    /// <returns>
    /// A string representing the faction's name.
    /// </returns>
    [JsonPropertyOrder(1)]
    [JsonPropertyName("sName")]
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// The amount of Reputation Points that the faction has in total.
    /// </summary>
    /// <returns>
    /// An integer representing the faction's total Reputation Points.
    /// </returns>
    [JsonPropertyOrder(2)]
    [JsonPropertyName("iRep")]
    [JsonConverter(typeof(StringIntConverter))]
    public int Points
    {
        get;
        set;
    } = 0;
    #endregion

    #region Methods: Reputation
    /// <summary>
    /// A method that returns a rank from calculating total points.
    /// </summary>
    /// <returns>
    /// An integer representing the rank.
    /// </returns>
    private int GetRank()
    {
        return Points switch
        {
            >= 302500 => 10,
            >= 202500 => 9,
            >= 129600 => 8,
            >= 78400 => 7,
            >= 44100 => 6,
            >= 22500 => 5,
            >= 10000 => 4,
            >= 3600 => 3,
            >= 900 => 2,
            _ => 1
        };
    }

    /// <summary>
    /// A method that returns the total points based on the rank.
    /// </summary>
    /// <returns>
    /// An integer representing the total points on one rank.
    /// </returns>
    private int GetNextTotalPoints()
    {
        return Rank switch
        {
            10 => 302500,
            9 => 202500,
            8 => 129600,
            7 => 78400,
            6 => 44100,
            5 => 22500,
            4 => 10000,
            3 => 3600,
            2 => 900,
            1 => 0
        };
    }
    #endregion

    #region Methods: Override
    /// <summary>
    /// A method that returns the faction's name.
    /// </summary>
    /// <returns>
    /// A string representing the faction's name.
    /// </returns>
    public override string ToString()
    {
        return $"{Name}, Rank {Rank}";
    }
    #endregion

}
