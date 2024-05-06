using Mystrose.GameModels.General;
using Mystrose.GameModels.Master;
using Mystrose.ReadableModels.Base;
using System.Text.Json.Serialization;

namespace Mystrose.ReadableModels.General;

public class RMInventoryItem : ReadableModel
{

    #region Constructor
    public RMInventoryItem(InventoryItem? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new InventoryItem();
        MandatorySearchProperties = new()
        {
            [nameof(Name)] = Name,
            [nameof(Inventory_Type)] = Inventory_Type
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private InventoryItem InventoryItem
    {
        get => (InventoryItem)Model;
    }
    #endregion

    #region Properties
    public int ID
    {
        get => InventoryItem.ID;
    }

    public string Name
    {
        get => InventoryItem.Name;
    }

    public string Inventory_Type
    {
        get => InventoryItem.InventoryType.ToString();
    }

    public string Equipment_Type
    {
        get => InventoryItem.EquipmentType.ToString();
    }

    public string Category_Type
    {
        get => InventoryItem.Type.ToString();
    }

    public int Quantity
    {
        get => InventoryItem.Quantity;
    }

    public int Maximum_Stack
    {
        get => InventoryItem.MaxStack;
    }

    public bool Is_Equipped
    {
        get => InventoryItem.IsEquipped;
    }

    public bool Is_Member_Tagged
    {
        get => InventoryItem.IsMemberTagged;
    }

    public bool Is_Coin_Tagged
    {
        get => InventoryItem.IsCoinTagged;
    }

    public string Metadata
    {
        get => InventoryItem.Metadata;
    }
    #endregion

}
