namespace Mystrose.Network.Handlers.JSON;

public class JHItemEquipment() : MessageHandler<JSONMessage>(new()
{
    ["equipItem"] = HandleEquip,
    ["unequipItem"] = HandleUnequip
})
{

    #region Methods: Handlers
    private static void HandleEquip(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["strES"]!.GetValue<string>();

        BaseItem baseItem = message.DataObject.Deserialize<BaseItem>()!;
        baseItem.EquipmentType = JsonSerializer.Deserialize<EquipmentType>($"\"{eqpType}\"");
        
        if (message.HostWorld.Avatar.EntityID == userId)
        {
            message.HostWorld.Avatar.Equipments[eqpType] = baseItem;

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.Equipments[eqpType] = baseItem;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    private static void HandleUnequip(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["strES"]!.GetValue<string>();

        if (message.HostWorld.Avatar.EntityID == userId)
        {
            message.HostWorld.Avatar.Equipments[eqpType] = null;

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Avatar);
        }

        Avatar? avatar = message.HostWorld.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.Equipments[eqpType] = null;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
