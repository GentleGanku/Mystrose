namespace Mystrose.DataRecords.ReadableModels;

public class RMFaction(Faction? model = null, World? world = null) : ReadableModel(model ?? new Faction(), world ?? new World())
{

    #region Properties: I/O
    public new Faction Model
    {
        get => (Faction)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public string Name => Model.Name;
    public int Reputation_Points => Model.Points;
    public int Rank => Model.Rank;
    public int Required_Rank_Points => Model.RequiredRankPoints;
    public int Required_Maximum_Points => Model.RequiredMaxPoints;
    #endregion

    #region Methods: Overrides
    public new Faction ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | Rank {Rank}";
    }
    #endregion

}