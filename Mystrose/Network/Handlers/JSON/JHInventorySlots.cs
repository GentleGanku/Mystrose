namespace Mystrose.Network.Handlers.JSON;

public class JHInventorySlots() : MessageHandler<JSONMessage>(new()
{
    ["buyBagSlots"] = HandleBagSlots,
    ["buyBankSlots"] = HandleBankSlots,
    ["buyHouseSlots"] = HandleHouseSlots
})
{

    #region Methods: Handlers
    private static void HandleBagSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.HostWorld.Avatar.Coins -= slots * 200;
        message.HostWorld.Avatar.InventorySlots += slots;

        message.HostWorld.Inventories[InventoryType.Base].TotalSlots += slots;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }

    private static void HandleBankSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.HostWorld.Avatar.Coins -= slots * 200;
        message.HostWorld.Avatar.BankSlots += slots;

        message.HostWorld.Inventories[InventoryType.Bank].TotalSlots += slots;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }

    private static void HandleHouseSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.HostWorld.Avatar.Coins -= slots * 200;
        message.HostWorld.Avatar.HouseSlots += slots;

        message.HostWorld.Inventories[InventoryType.House].TotalSlots += slots;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
    }
    #endregion

}
