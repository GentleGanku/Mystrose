using Mystrose.GameModels.General;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

public class RMCombatMessage : ReadableModel
{

    #region Constructor
    public RMCombatMessage(CombatMessage? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new CombatMessage();
        MandatorySearchProperties = new()
        {
            // 
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private CombatMessage CombatMessage
    {
        get => (CombatMessage)Model;
    }
    #endregion

    #region Properties
    public string Animation_Label
    {
        get => CombatMessage.AnimationString;
    }

    public string Text
    {
        get => CombatMessage.Text;
    }

    public string Cell
    {
        get => CombatMessage.Cell;
    }

    public string Source_Type
    {
        get => CombatMessage.SourceType.ToString();
    }

    public string Source_ID
    {
        get => CombatMessage.SourceID;
    }

    public string Target_Type
    {
        get => CombatMessage.TargetType.ToString();
    }

    public string Target_ID
    {
        get => CombatMessage.TargetID;
    }
    #endregion

}
