using Mystrose.GameModels.Environment;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.Environment;

public class RMArea : ReadableModel
{

    #region Constructor
    public RMArea(Area? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Area();
        MandatorySearchProperties = new()
        {
            //
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Area Area
    {
        get => (Area)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => Area.ID;
    }

    public string Name
    {
        get => Area.Format.Name;
    }

    public int Instance_Number
    {
        get => Area.Instance;
    }

    public string Players
    {
        get => string.Join("|", Area.Players.Select(p => p.Name));
    }

    public string Monsters
    {
        get => string.Join("|", Area.Monsters.Where(m => m.IsAlive).Select(m => m.MonMapID));
    }

    public int Player_Count_in_Area
    {
        get => Area.Players.Count;
    }

    public int Player_Count_in_Current_Cell
    {
        get => Area.Players.Where(p => p.Cell.Equals(World is not null ? World.Master.Cell : string.Empty)).Count();
    }

    public int Monster_Count_in_Area
    {
        get => Area.Monsters.Count;
    }

    public int Monster_Count_in_Current_Cell
    {
        get => Area.Monsters.Where(m => m.Cell.Equals(World is not null ? World.Master.Cell : string.Empty)).Count();
    }
    #endregion

}
