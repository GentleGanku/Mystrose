namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMShopItem : ReadableModel<ShopItem>
{

    #region Constructor
    public RMShopItem(ShopItem? model = null, World? world = null)
        : base(model ?? new ShopItem(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Shop_Item_ID)] = Shop_Item_ID
        };
    }
    #endregion

    #region Properties
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

    #region Methods: Overrides
    public override string ToString()
    {
        return $"[{Shop_Item_ID}] {Name}";
    }
    #endregion

}