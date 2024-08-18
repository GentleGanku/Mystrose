namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMFaction : ReadableModel
{

    #region Constructor
    public RMFaction(Faction? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Faction();
        MandatorySearchProperties = new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Faction Faction
    {
        get => (Faction)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => Faction.ID;
    }

    public string Name
    {
        get => Faction.Name;
    }

    public int Reputation_Points
    {
        get => Faction.Points;
    }

    public int Rank
    {
        get => Faction.Rank;
    }

    public int Required_Rank_Points
    {
        get => Faction.RequiredRankPoints;
    }

    public int Required_Maximum_Points
    {
        get => Faction.RequiredMaxPoints;
    }
    #endregion

}
