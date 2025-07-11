namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMFaction : ReadableModel<Faction>
{

    #region Constructor
    public RMFaction(Faction? model = null, World? world = null) 
        : base(model ?? new Faction(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Properties
    public int ID => Model.ID;
    public string Name => Model.Name;
    public int Reputation_Points => Model.Points;
    public int Rank => Model.Rank;
    public int Required_Rank_Points => Model.RequiredRankPoints;
    public int Required_Maximum_Points => Model.RequiredMaxPoints;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | Rank {Rank}";
    }
    #endregion

}