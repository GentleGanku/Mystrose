namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCShop(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void Load(int shopId)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.sendLoadShopRequest", shopId);
        });
    }
    
    public bool BuyItem(ShopItem item, int shopId, int quantityToBuy = 1)
    {
        return Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);
            int roomId = Service.Map.GetRoomID();
            
            Service.SetGameObject("world.shopBuyItem", itemString);
            return Service.SendToServer($"%xt%zm%buyItem%{roomId}%{item.ID}%{shopId}%{item.ShopItemID}%{quantityToBuy}%");
        });
    }
    
    public bool BuyItemAtMax(ShopItem item, int shopId)
    {
        return Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);
            int quantityToBuy = Service.CallGameFunction<int>("world.maximumShopBuys", itemString);
            
            if (quantityToBuy <= 0)
            {
                return false;
            }
            
            int roomId = Service.Map.GetRoomID();
            Service.SetGameObject("world.shopBuyItem", itemString);
            return Service.SendToServer($"%xt%zm%buyItem%{roomId}%{item.ID}%{shopId}%{item.ShopItemID}%{quantityToBuy}%");
        });
    }
    
    public bool SellItem(InventoryItem item, int quantityToSell = 1)
    {
        return Execute(() =>
        {
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%sellItem%{roomId}%{item.ID}%{quantityToSell}%{item.CharacterItemID}%");
        });
    }
    
    public bool SellItemAtMax(InventoryItem item)
    {
        return Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);
            int quantityToSell = Service.CallGameFunction<int>("world.maximumShopSells", itemString);
            
            if (quantityToSell <= 0)
            {
                return false;
            }
            
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%sellItem%{roomId}%{item.ID}%{quantityToSell}%{item.CharacterItemID}%");
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCShop");
    }
    #endregion

}
