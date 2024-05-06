using Mystrose.GameModels.Environment;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.Environment;

public class RMQuest : ReadableModel
{

    #region Constructor
    public RMQuest(Quest? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Quest();
        MandatorySearchProperties = new()
        {
            [nameof(ID)] = ID
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Quest Quest
    {
        get => (Quest)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => Quest.ID;
    }

    public bool Is_One_Time
    {
        get => Quest.IsOneTime;
    }

    public string Status
    {
        get => Quest.StatusType.ToString();
    }
    #endregion

}
