using Mystrose.GameModels.General;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

public class RMActiveSkill : ReadableModel
{

    #region Constructor
    public RMActiveSkill(ActiveSkill? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new ActiveSkill();
        MandatorySearchProperties = new()
        {
            [nameof(Index)] = Index
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private ActiveSkill ActiveSkill
    {
        get => (ActiveSkill)Model;
    }
    #endregion

    #region Properties
    public int Index
    {
        get => ActiveSkill.Index;
    }

    public string Name
    {
        get => ActiveSkill.Name;
    }

    public bool Is_Safely_Usable
    {
        get => ActiveSkill.IsSafeToUse;
    }

    public int Range
    {
        get => ActiveSkill.Range;
    }

    public int Mana_Cost
    {
        get => ActiveSkill.ManaCost;
    }

    public int Cooldown
    {
        get => ActiveSkill.Cooldown / 1000;
    }
    #endregion

}
