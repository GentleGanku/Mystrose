namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMEventMessage : ReadableModel
{

    #region Constructor
    public RMEventMessage(EventMessage? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new EventMessage();
        MandatorySearchProperties = new()
        {
            // 
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private EventMessage EventMessage
    {
        get => (EventMessage)Model;
    }
    #endregion

    #region Properties
    public string Signature_Header
    {
        get => EventMessage.Sign;
    }

    public string Value
    {
        get => EventMessage.Value;
    }
    #endregion

}
