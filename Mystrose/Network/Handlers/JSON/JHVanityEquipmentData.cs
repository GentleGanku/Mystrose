namespace Mystrose.Network.Handlers.JSON;

public class JHVanityEquipmentData() : MessageHandler<JSONMessage>(new()
{
    ["wearItem"] = HandleItemWearing,
    ["unwearItem"] = HandleItemUnwearing
})
{

    #region Methods: Handlers
    private static void HandleItemWearing(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["sES"]!.GetValue<string>();

        BaseItem vanityItem = message.DataObject.Deserialize<BaseItem>()!;

        if (message.HostWorld.Avatar.EntityID == userId)
        {
            message.HostWorld.Avatar.VanityEquipments[eqpType] = vanityItem;
        }

        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar is null)
        {
            return;
        }

        avatar.VanityEquipments[eqpType] = vanityItem;
    }

    private static void HandleItemUnwearing(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["sES"]!.GetValue<string>();

        if (message.HostWorld.Avatar.EntityID == userId)
        {
            message.HostWorld.Avatar.VanityEquipments[eqpType] = null;
        }

        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar is null)
        {
            return;
        }

        avatar.VanityEquipments[eqpType] = null;
    }
    #endregion

}
