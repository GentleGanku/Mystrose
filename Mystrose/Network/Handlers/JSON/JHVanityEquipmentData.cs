namespace Mystrose.Network.Handlers.JSON;

public static class JHVanityEquipmentData
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["wearItem"] = HandleItemWearing,
        ["unwearItem"] = HandleItemUnwearing
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
    public static void HandleItemWearing(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["sES"]!.GetValue<string>();

        BaseItem vanityItem = message.DataObject.Deserialize<BaseItem>()!;

        if (message.World.Avatar.UserID == userId)
        {
            message.World.Avatar.VanityEquipments[eqpType] = vanityItem;
        }

        Avatar? avatar = message.World.Area.Players.Find(
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

    public static void HandleItemUnwearing(JSONMessage message)
    {
        int userId = message.DataObject["uid"]!.GetValue<int>();
        string eqpType = message.DataObject["sES"]!.GetValue<string>();

        if (message.World.Avatar.UserID == userId)
        {
            message.World.Avatar.VanityEquipments[eqpType] = null;
        }

        Avatar? avatar = message.World.Area.Players.Find(
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
