namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMActiveSkill : ReadableModel<ActiveSkill>
{

    #region Constructor
    public RMActiveSkill(ActiveSkill? model = null, World? world = null) 
        : base(model ?? new ActiveSkill(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Index)] = Index
        };
    }
    #endregion

    #region Properties
    public int Index => Model.Index;
    public string Name => Model.Name;
    public bool Is_Safely_Usable => Model.IsSafeToUse;
    public int Range => Model.Range;
    public int Mana_Cost => Model.ManaCost;
    public int Cooldown => Model.Cooldown;
    public int Cooldown_Duration => Model.CooldownDuration;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | {NumberHelper.ToOrdinal(Index)} active skill";
    }
    #endregion

}
