namespace Mystrose.DataRecords.ReadableModels;

public class RMCombatMessage(CombatMessage? model = null, World? world = null) : ReadableModel(model ?? new CombatMessage(), world ?? new World())
{

    #region Properties: I/O
    public new CombatMessage Model
    {
        get => (CombatMessage)base.Model;
    }
    #endregion

    #region Properties: Attributes
    public string Animation_Label => Model.AnimationString;
    public string Text => Model.Text;
    public string Cell => Model.Cell;
    public string Source_Type => Model.SourceType.ToString();
    public string Source_ID => Model.SourceID;
    public string Target_Type => Model.TargetType.ToString();
    public string Target_ID => Model.TargetID;
    #endregion

    #region Methods: Conversion
    public new CombatMessage ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        string entitiesResponsible = Source_ID.Equals(Target_ID) ? 
            $"On {Target_Type.ToLower()} {Target_ID}" : 
            $"From {Source_Type.ToLower()} {Source_ID} to {Target_Type.ToLower()} {Target_ID}";
        string textLabel = string.IsNullOrEmpty(Text) ? 
            ": No text provided" : 
            $": {Text}";
        return $"{Animation_Label} | {entitiesResponsible}{textLabel}";
    }
    #endregion

}