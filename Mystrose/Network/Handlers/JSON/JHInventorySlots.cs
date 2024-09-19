namespace Mystrose.Network.Handlers.JSON;

public static class JHInventorySlots
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["buyBagSlots"] = HandleBagSlots,
        ["buyBankSlots"] = HandleBankSlots,
        ["buyHouseSlots"] = HandleHouseSlots
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
    public static void HandleBagSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.World.Avatar.Coins -= slots * 200;
        message.World.Avatar.InventorySlots += slots;

        message.World.Inventories[InventoryType.Base].TotalSlots += slots;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }

    public static void HandleBankSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.World.Avatar.Coins -= slots * 200;
        message.World.Avatar.BankSlots += slots;

        message.World.Inventories[InventoryType.Bank].TotalSlots += slots;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }

    public static void HandleHouseSlots(JSONMessage message)
    {
        bool isSuccess = message.DataObject["bitSuccess"]!.GetValue<int>() == 1;

        if (!isSuccess)
        {
            return;
        }

        int slots = message.DataObject["iSlots"]!.GetValue<int>();

        message.World.Avatar.Coins -= slots * 200;
        message.World.Avatar.HouseSlots += slots;

        message.World.Inventories[InventoryType.House].TotalSlots += slots;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }
    #endregion

}
