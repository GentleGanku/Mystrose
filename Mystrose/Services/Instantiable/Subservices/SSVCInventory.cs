using Mystrose.DataRecords.Game;

namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCInventory(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void TryItem(BaseItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("xTryMe", itemString);
        });
    }
    
    public bool WearItem(BaseItem item)
    {
        return Execute(() =>
        {
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%wearItem%{roomId}%{item.ID}%");
        });
    }
    
    public bool UnwearItem(BaseItem item)
    {
        return Execute(() =>
        {
            string equipmentType = JSONParser.Serialize(item.EquipmentType);
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%unwearItem%{roomId}%{equipmentType}%");
        });
    }
    
    public bool EquipItem(InventoryItem item)
    {
        return Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            return Service.CallGameFunction<bool>("world.sendEquipItemRequest", itemString);
        });
    }
    
    public void UnequipItem(InventoryItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.sendUnequipItemRequest", itemString);
        });
    }
    
    public void EquipUseableItem(InventoryItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.equipUseableItem", itemString);
        });
    }
    
    public void UnequipUseableItem()
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.unequipUseableItem");
        });
    }
    
    public void UseItem(InventoryItem item)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.tryUseItem", itemString);
        });
    }
    
    public void RemoveItem(InventoryItem item, int quantityToRemove)
    {
        Execute(() =>
        {
            string itemString = JSONParser.Serialize(item);

            Service.CallGameFunction("world.sendRemoveItemRequest", itemString, quantityToRemove);
        });
    }
    
    public void RemoveTemporaryItem(InventoryItem item, int quantityToRemove)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.sendRemoveTempItemRequest", item.ID, quantityToRemove);
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCInventory");
    }
    #endregion

}
