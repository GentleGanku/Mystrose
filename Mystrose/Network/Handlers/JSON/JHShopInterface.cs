using Mystrose.DataRecords.Game;

namespace Mystrose.Network.Handlers.JSON;

public class JHShopInterface() : MessageHandler<JSONMessage>(new()
{
    ["loadShop"] = HandleShopLoad,
    ["loadEnhShop"] = HandleEnhShopLoad,
    ["buyItem"] = HandleItemPurchase,
    ["sellItem"] = HandleItemSale,
    ["removeItem"] = HandleItemRemoval
})
{

    #region Methods: Handlers
    private static void HandleShopLoad(JSONMessage message)
    {
        Shop shop = message.DataObject["shopinfo"].Deserialize<Shop>()!;

        message.HostWorld.Shop = shop;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Shop);
    }

    private static void HandleEnhShopLoad(JSONMessage message)
    {
        Shop enhShop = message.DataObject["shopinfo"].Deserialize<Shop>()!;

        message.HostWorld.EnhancementShop = enhShop;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.EnhancementShop);
    }

    private static void HandleItemPurchase(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int id = message.DataObject["ItemID"]!.GetValue<int>();
        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();

        ShopItem shopItem = message.HostWorld.Shop.Items.Find(
            (item) =>
            {
                return item.ID == id;
            })!;

        if (shopItem.IsCoinTagged)
        {
            message.HostWorld.Avatar.Coins -= shopItem.Cost * qty;
        }
        else
        {
            message.HostWorld.Avatar.Gold -= shopItem.Cost * qty;
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);

        if (shopItem.IsHouseItem)
        {
            if (message.HostWorld.Inventories[InventoryType.House].TryGetValue(shopItem.ID, out InventoryItem? houseItem))
            {
                houseItem.Quantity += qty;

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, houseItem);
            }
            else
            {
                InventoryItem newHouseItem = JsonSerializer.Deserialize<InventoryItem>(JsonSerializer.Serialize(shopItem))!;
                newHouseItem.InventoryType = InventoryType.House;
                newHouseItem.CharacterItemID = charItemID;
                newHouseItem.Quantity = qty;

                message.HostWorld.Inventories[InventoryType.House].Add(id, newHouseItem);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, newHouseItem);
            }
        }
        else
        {
            if (message.HostWorld.Inventories[InventoryType.Base].TryGetValue(shopItem.ID, out InventoryItem? baseItem))
            {
                baseItem.Quantity += qty;

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, baseItem);
            }
            else
            {
                InventoryItem newBaseItem = JsonSerializer.Deserialize<InventoryItem>(JsonSerializer.Serialize(shopItem))!;
                newBaseItem.InventoryType = InventoryType.Base;
                newBaseItem.CharacterItemID = charItemID;
                newBaseItem.Quantity = qty;

                message.HostWorld.Inventories[InventoryType.Base].Add(id, newBaseItem);

                MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, newBaseItem);
            }
        }
    }

    private static void HandleItemSale(JSONMessage message)
    {
        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();
        int currencyAmount = message.DataObject["intAmount"]!.GetValue<int>();
        bool isCoins = message.DataObject["bCoins"]!.GetValue<int>() == 1;

        if (isCoins)
        {
            message.HostWorld.Avatar.Coins += currencyAmount;
        }
        else if (message.HostWorld.Avatar.Gold < 100000000)
        {
            message.HostWorld.Avatar.Gold += currencyAmount;
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);

        InventoryItem? item = message.HostWorld.Inventories[InventoryType.Base].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item ??= message.HostWorld.Inventories[InventoryType.House].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item!.Quantity -= qty;

        if (item.Quantity <= 0)
        {
            item.Quantity = 0;
            message.HostWorld.Inventories[item.InventoryType].Remove(item.ID);
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
    }

    private static void HandleItemRemoval(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int charItemID = message.DataObject["CharItemID"]!.GetValue<int>();
        int qty = message.DataObject["iQty"]!.GetValue<int>();

        InventoryItem? item = message.HostWorld.Inventories[InventoryType.Base].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item ??= message.HostWorld.Inventories[InventoryType.House].Values.FirstOrDefault(
            (item) =>
            {
                return item.CharacterItemID == charItemID;
            });

        item!.Quantity -= qty;

        if (item.Quantity <= 0)
        {
            item.Quantity = 0;
            message.HostWorld.Inventories[item.InventoryType].Remove(item.ID);
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, item);
    }
    #endregion

}
