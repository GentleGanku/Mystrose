namespace Mystrose.Network.Handlers.JSON;

public static class JHItemEquipment
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["equipItem"] = HandleEquip,
        ["unequipItem"] = HandleUnequip
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
    public static void HandleEquip(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["strES"]!.GetValue<string>();

        BaseItem baseItem = message.DataObject.Deserialize<BaseItem>()!;
        baseItem.EquipmentType = JsonSerializer.Deserialize<EquipmentType>($"\"{eqpType}\"");
        
        if (message.World.Avatar.EntityID == userId)
        {
            message.World.Avatar.Equipments[eqpType] = baseItem;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.Equipments[eqpType] = baseItem;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }

    public static void HandleUnequip(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["strES"]!.GetValue<string>();

        if (message.World.Avatar.EntityID == userId)
        {
            message.World.Avatar.Equipments[eqpType] = null;

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
        }

        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar == null)
        {
            return;
        }

        avatar.Equipments[eqpType] = null;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
