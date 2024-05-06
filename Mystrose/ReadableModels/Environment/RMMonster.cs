using Mystrose.GameModels.Environment;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.Environment;

public class RMMonster : ReadableModel
{

    #region Constructor
    public RMMonster(Monster? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Monster();
        MandatorySearchProperties = new()
        {
            [nameof(Monster_Map_ID)] = Monster_Map_ID
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Monster Monster
    {
        get => (Monster)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => Monster.ID;
    }

    public int Monster_Map_ID
    {
        get => Monster.MonMapID;
    }

    public int State
    {
        get => (int)Monster.State;
    }

    public string Cell
    {
        get => Monster.Cell;
    }

    public string Targets
    {
        get => string.Join("|", Monster.Targets);
    }

    public double Max_HP
    {
        get => Monster.MaxHP;
    }

    public double HP
    {
        get => Monster.HP;
    }

    public int Max_MP
    {
        get => Monster.MaxMP;
    }

    public int MP
    {
        get => Monster.MP;
    }

    public double DPS
    {
        get => Monster.DPS;
    }

    public bool Is_Aggressive
    {
        get => Monster.IsAggressive;
    }
    #endregion

}
