using Mystrose.GameModels.General;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

public class RMAura : ReadableModel
{

    #region Constructor
    public RMAura(Aura? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Aura();
        MandatorySearchProperties = new()
        {
            [nameof(Name)] = Name,
            [nameof(Target_Type)] = Target_Type,
            [nameof(Target_ID)] = Target_ID
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Aura Aura
    {
        get => (Aura)Model;
    }
    #endregion

    #region Fields
    public string Name
    {
        get => Aura.Name;
    }

    public string Value
    {
        get => Aura.Value;
    }

    public int Stack_Value
    {
        get => Aura.StackValue;
    }

    public int Duration
    {
        get => Aura.Duration;
    }

    public int Runtime
    {
        get => Aura.Runtime;
    }

    public string Source_Type
    {
        get => Aura.SourceType.ToString();
    }

    public string Source_ID
    {
        get => Aura.SourceID;
    }

    public string Target_Type
    {
        get => Aura.TargetType.ToString();
    }

    public string Target_ID
    {
        get => Aura.TargetID;
    }

    public string Disable_Type
    {
        get => Aura.DisableType.ToString();
    }

    public bool Is_Added
    {
        get => Aura.IsAdded;
    }
    #endregion

}
