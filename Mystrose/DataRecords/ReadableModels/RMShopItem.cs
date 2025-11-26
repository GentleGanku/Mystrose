namespace Mystrose.DataRecords.ReadableModels;

public class RMShopItem(ShopItem? model = null, World? world = null) : ReadableModel(model ?? new ShopItem(), world ?? new World())
{

    #region Properties: I/O
    public new ShopItem Model
    {
        get => (ShopItem)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Shop_Item_ID)] = Shop_Item_ID
        };
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public int Shop_Item_ID => Model.ShopItemID;
    public string Name => Model.Name;
    public int Cost => Model.Cost;
    public string Turnin_Items => string.Join("|", Model.TurninItems.Select(ti => $"{ti.ID}:{ti.Name}:{ti.Quantity}"));
    public string Equipment_Type => Model.EquipmentType.ToString();
    public string Category_Type => Model.Type.ToString();
    public int Quantity => Model.Quantity;
    public int Maximum_Stack => Model.MaxStack;
    public bool Is_Member_Tagged => Model.IsMemberTagged;
    public bool Is_Coin_Tagged => Model.IsCoinTagged;
    public string Metadata => Model.Metadata;
    #endregion

    #region Methods: Conversion
    public new ShopItem ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | {Category_Type} / SIID {Shop_Item_ID}";
    }
    #endregion

}