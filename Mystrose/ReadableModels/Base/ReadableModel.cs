using Mystrose.GameModels.Master;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.Base;

public abstract class ReadableModel
{

    #region Constructor
    public ReadableModel(object? model, World? world)
    {
        World = world;
        Model = model;
        MandatorySearchProperties = [];
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public World? World
    {
        get;
        set;
    }

    [JsonIgnore]
    public object? Model
    {
        get;
        set;
    }

    [JsonIgnore]
    public Dictionary<string, object> MandatorySearchProperties
    {
        get;
        set;
    }
    #endregion

}
