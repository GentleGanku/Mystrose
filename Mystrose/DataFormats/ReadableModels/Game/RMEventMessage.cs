namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMEventMessage : ReadableModel<EventMessage>
{

    #region Constructor
    public RMEventMessage(EventMessage? model = null, World? world = null)
        : base(model ?? new EventMessage(), world ?? new World())
    {
        KeyProperties = new();
    }
    #endregion

    #region Properties
    public string Signature_Header => Model.Sign;
    public string Value => Model.Value;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Signature_Header} | {Value}";
    }
    #endregion

}