namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMCombatMessage : ReadableModel<CombatMessage>
{

    #region Constructor
    public RMCombatMessage(CombatMessage? model = null, World? world = null) 
        : base(model ?? new CombatMessage(), world ?? new World())
    {
        KeyProperties = new();
    }
    #endregion

    #region Properties
    public string Animation_Label => Model.AnimationString;
    public string Text => Model.Text;
    public string Cell => Model.Cell;
    public string Source_Type => Model.SourceType.ToString();
    public string Source_ID => Model.SourceID;
    public string Target_Type => Model.TargetType.ToString();
    public string Target_ID => Model.TargetID;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"[{Target_ID}] {Text}";
    }
    #endregion

}