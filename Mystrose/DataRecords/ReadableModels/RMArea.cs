namespace Mystrose.DataRecords.ReadableModels;

public class RMArea(Area? model = null, World? world = null) : ReadableModel(model ?? new Area(), world ?? new World())
{

    #region Properties: I/O
    public new Area Model
    {
        get => (Area)base.Model;
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public string Name => Model.Format.Name;
    public int Instance_Number => Model.Instance;
    public string Players => string.Join("|", Model.Players.Select(p => p.Name));
    public string Monsters => string.Join("|", Model.Monsters.Where(m => m.IsAlive).Select(m => m.MonMapID));
    public int Player_Count_in_Area => Model.Players.Count;
    public int Player_Count_in_Current_Cell => Model.Players.Where(p => p.Cell.Equals(World is not null ? World.Avatar.Cell : string.Empty)).Count();
    public int Monster_Count_in_Area => Model.Monsters.Count;
    public int Monster_Count_in_Current_Cell => Model.Monsters.Where(m => m.Cell.Equals(World is not null ? World.Avatar.Cell : string.Empty)).Count();
    #endregion

    #region Methods: Conversion
    public new Area ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name}-{Instance_Number} | {Player_Count_in_Area} players";
    }
    #endregion

}