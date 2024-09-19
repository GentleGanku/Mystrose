namespace Mystrose.Network.Handlers.XT;

public static class XHMapPlayer
{

    #region Fields
    private static readonly Dictionary<string, Action<XTMessage>> _handlers = new()
    {
        ["exitArea"] = HandleExit
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(XTMessage message)
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
    public static void HandleExit(XTMessage message)
    {
        int userId = int.Parse(message.Arguments[4]);

        Avatar? avatar = message.World.Area.Players.Find(
            (avt) =>
            {
                return avt.EntityID == userId;
            });

        if (avatar is null)
        {
            return;
        }

        message.World.Area.Players.Remove(avatar);

        avatar.Cell = avatar.Pad = "None";

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

}
