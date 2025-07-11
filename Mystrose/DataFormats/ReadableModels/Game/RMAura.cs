namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMAura : ReadableModel<Aura>
{

    #region Constructor
    public RMAura(Aura? model = null, World? world = null) 
        : base(model ?? new Aura(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Name)] = Name,
            [nameof(Target_Type)] = Target_Type,
            [nameof(Target_ID)] = Target_ID
        };
    }
    #endregion

    #region Properties
    public string Name => Model.Name;
    public string Value => Model.Value;
    public int Stack_Value => Model.StackValue;
    public int Duration => Model.Duration;
    public int Runtime => Model.Runtime;
    public string Source_Type => Model.SourceType.ToString();
    public string Source_ID => Model.SourceID;
    public string Target_Type => Model.TargetType.ToString();
    public string Target_ID => Model.TargetID;
    public string Disable_Type => Model.DisableType.ToString();
    public bool Is_Added => Model.IsAdded;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        string entitiesResponsible = Source_ID.Equals(Target_ID) ? 
            $"On {Target_Type.ToLower()} {Target_ID}" : 
            $"From {Source_Type.ToLower()} {Source_ID} to {Target_Type.ToLower()} {Target_ID}";
        return $"{Name} | {entitiesResponsible}";
    }
    #endregion

}