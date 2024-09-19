namespace Mystrose.Network.Handlers.JSON;

public static class JHEquipmentEnhancement
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["enhanceItemShop"] = HandleShopEnhancement,
        //["enhanceItemLocal"] = HandleLocalEnhancement // WIP
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
    public static void HandleShopEnhancement(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("iCost"))
        {
            int gold = message.DataObject["iCost"].GetValue<int>();
            message.World.Avatar.Gold -= gold;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        // WIP
    }
    #endregion

}
