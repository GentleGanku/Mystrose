using Mystrose.GameModels.Master;
using Mystrose.GameModels.SkillObjects;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

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
