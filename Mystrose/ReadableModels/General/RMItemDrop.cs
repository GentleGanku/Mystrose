using Mystrose.GameModels.Base;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

public class RMItemDrop : ReadableModel
{

    #region Constructor
    public RMItemDrop(BaseItem? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new BaseItem();
        MandatorySearchProperties = new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private BaseItem BaseItem
    {
        get => (BaseItem)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => BaseItem.ID;
    }

    public string Name
    {
        get => BaseItem.Name;
    }

    public string Equipment_Type
    {
        get => BaseItem.EquipmentType.ToString();
    }

    public string Category_Type
    {
        get => BaseItem.Type.ToString();
    }

    public int Level
    {
        get => BaseItem.Level;
    }

    public int Quantity
    {
        get => BaseItem.Quantity;
    }

    public int Maximum_Stack
    {
        get => BaseItem.MaxStack;
    }

    public bool Is_Temporary
    {
        get => BaseItem.IsTemporary;
    }

    public bool Is_Member_Tagged
    {
        get => BaseItem.IsMemberTagged;
    }

    public bool Is_Coin_Tagged
    {
        get => BaseItem.IsCoinTagged;
    }

    public string Metadata
    {
        get => BaseItem.Metadata;
    }
    #endregion

}
