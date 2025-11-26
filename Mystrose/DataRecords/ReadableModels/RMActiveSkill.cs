
namespace Mystrose.DataRecords.ReadableModels;

public class RMActiveSkill(ActiveSkill? model = null, World? world = null) : ReadableModel(model ?? new ActiveSkill(), world ?? new World())
{

    #region Properties: I/O
    public new ActiveSkill Model
    {
        get => (ActiveSkill)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Index)] = Index
        };
    }
    #endregion

    #region Properties: Attributes
    public int Index => Model.Index;
    public string Name => Model.Name;
    public bool Is_Safely_Usable => Model.IsSafeToUse;
    public int Range => Model.Range;
    public int Mana_Cost => Model.ManaCost;
    public int Cooldown => Model.Cooldown;
    public int Cooldown_Duration => Model.CooldownDuration;
    #endregion

    #region Methods: Conversion
    public new ActiveSkill ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | {NumberHelper.ToOrdinal(Index)} active skill";
    }
    #endregion

}
