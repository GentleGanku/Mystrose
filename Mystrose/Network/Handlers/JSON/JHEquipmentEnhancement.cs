namespace Mystrose.Network.Handlers.JSON;

public class JHEquipmentEnhancement() : MessageHandler<JSONMessage>(new()
{
    ["enhanceItemShop"] = HandleShopEnhancement,
    ["enhanceItemLocal"] = HandleLocalEnhancement
})
{

    #region Methods: Handlers
    private static void HandleShopEnhancement(JSONMessage message)
    {
        if (message.DataObject.ContainsKey("iCost"))
        {
            int gold = message.DataObject["iCost"].GetValue<int>();
            message.HostWorld.Avatar.Gold -= gold;

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        // TODO: Implement shop enhancement
    }

    private static void HandleLocalEnhancement(JSONMessage message)
    {
        // TODO: Implement local enhancement
    }
    #endregion

}
