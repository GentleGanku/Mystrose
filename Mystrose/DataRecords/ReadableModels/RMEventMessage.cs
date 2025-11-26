namespace Mystrose.DataRecords.ReadableModels;

public class RMEventMessage(EventMessage? model = null, World? world = null) : ReadableModel(model ?? new EventMessage(), world ?? new World())
{

    #region Properties: I/O
    public new EventMessage Model
    {
        get => (EventMessage)base.Model;
    }
    #endregion

    #region Properties: Attributes
    public string Message => Model.Sign;
    public string Event_Code => Model.Value;
    #endregion

    #region Methods: Overrides
    public new EventMessage ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Message} | Highlights for {Event_Code}";
    }
    #endregion

}