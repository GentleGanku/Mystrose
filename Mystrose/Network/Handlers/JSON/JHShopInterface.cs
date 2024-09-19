namespace Mystrose.Network.Handlers.JSON;

public static class JHShopInterface
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["loadShop"] = HandleShopLoad,
        ["loadEnhShop"] = HandleEnhShopLoad,
        ["buyItem"] = HandleItemPurchase,
        ["sellItem"] = HandleItemSale,
        ["removeItem"] = HandleItemRemoval
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    public static void HandleShopLoad(JSONMessage message)
    {
        Shop shop = message.DataObject["shopinfo"].Deserialize<Shop>()!;

        message.World.Shop = shop;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Shop);
    }

    public static void HandleEnhShopLoad(JSONMessage message)
    {
        Shop enhShop = message.DataObject["shopinfo"].Deserialize<Shop>()!;

        message.World.EnhancementShop = enhShop;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.EnhancementShop);
    }

    public static void HandleItemPurchase(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"]!.GetValue<int>();
        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();

        ShopItem shopItem = message.World.Shop.Items.Find(
            (item) =>
            {
                return item.ID == id;
            })!;

        if (shopItem.IsCoinTagged)
        {
            message.World.Avatar.Coins -= shopItem.Cost * qty;
        }
        else
        {
            message.World.Avatar.Gold -= shopItem.Cost * qty;
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);

        if (shopItem.IsHouseItem)
        {
            if (message.World.Inventories[InventoryType.House].TryGetValue(shopItem.ID, out InventoryItem? houseItem))
            {
                houseItem.Quantity += qty;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, houseItem);
            }
            else
            {
                InventoryItem newHouseItem = JsonSerializer.Deserialize<InventoryItem>(JsonSerializer.Serialize(shopItem))!;
                newHouseItem.InventoryType = InventoryType.House;
                newHouseItem.CharacterItemID = charItemID;
                newHouseItem.Quantity = qty;

                message.World.Inventories[InventoryType.House].Add(id, newHouseItem);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, newHouseItem);
            }
        }
        else
        {
            if (message.World.Inventories[InventoryType.Base].TryGetValue(shopItem.ID, out InventoryItem? baseItem))
            {
                baseItem.Quantity += qty;

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, baseItem);
            }
            else
            {
                InventoryItem newBaseItem = JsonSerializer.Deserialize<InventoryItem>(JsonSerializer.Serialize(shopItem))!;
                newBaseItem.InventoryType = InventoryType.Base;
                newBaseItem.CharacterItemID = charItemID;
                newBaseItem.Quantity = qty;

                message.World.Inventories[InventoryType.Base].Add(id, newBaseItem);

                SVCScriptManager.InvokeTrigger(message.Identifier.Codename, newBaseItem);
            }
        }
    }

    public static void HandleItemSale(JSONMessage message)
    {
        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();
        int currencyAmount = message.DataObject["intAmount"]!.GetValue<int>();
        bool isCoins = message.DataObject["bCoins"]!.GetValue<int>() == 1;

        if (isCoins)
        {
            message.World.Avatar.Coins += currencyAmount;
        }
        else if (message.World.Avatar.Gold < 100000000)
        {
            message.World.Avatar.Gold += currencyAmount;
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);

        InventoryItem? item = message.World.Inventories[InventoryType.Base].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item ??= message.World.Inventories[InventoryType.House].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item!.Quantity -= qty;

        if (item.Quantity <= 0)
        {
            item.Quantity = 0;
            message.World.Inventories[item.InventoryType].Remove(item.ID);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
    }

    public static void HandleItemRemoval(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();

        InventoryItem? item = message.World.Inventories[InventoryType.Base].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item ??= message.World.Inventories[InventoryType.House].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item!.Quantity -= qty;

        if (item.Quantity <= 0)
        {
            item.Quantity = 0;
            message.World.Inventories[item.InventoryType].Remove(item.ID);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, item);
    }
    #endregion

}
