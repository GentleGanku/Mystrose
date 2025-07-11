namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMInventoryItem : ReadableModel<InventoryItem>
{

    #region Constructor
    public RMInventoryItem(InventoryItem? model = null, World? world = null) 
        : base(model ?? new InventoryItem(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Name)] = Name,
            [nameof(Inventory_Type)] = Inventory_Type
        };
    }
    #endregion

    #region Properties
    public int ID => Model.ID;
    public string Name => Model.Name;
    public string Inventory_Type => Model.InventoryType.ToString();
    public string Equipment_Type => Model.EquipmentType.ToString();
    public string Category_Type => Model.Type.ToString();
    public int Quantity => Model.Quantity;
    public int Maximum_Stack => Model.MaxStack;
    public bool Is_Equipped => Model.IsEquipped;
    public bool Is_Member_Tagged => Model.IsMemberTagged;
    public bool Is_Coin_Tagged => Model.IsCoinTagged;
    public string Metadata => Model.Metadata;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | {Category_Type} / ID {ID}";
    }
    #endregion

}