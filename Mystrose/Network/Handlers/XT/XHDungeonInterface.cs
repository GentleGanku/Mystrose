namespace Mystrose.Network.Handlers.XT;

public static class XHDungeonInterface
{

    #region Fields
    private static readonly Dictionary<string, Action<XTMessage>> _handlers = new()
    {
        ["dungeonMTC"] = HandleMoveToCell,
        ["dungeonCompleted"] = HandleDungeonCompletion
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
    public static void HandleMoveToCell(XTMessage message)
    {
        bool isSuccess = bool.Parse(message.Arguments[4]);

        if (!isSuccess)
        {
            return;
        }

        string[] destination = message.Arguments[5].Split(',');

        message.World.Avatar.Cell = destination[0];
        message.World.Avatar.Pad = destination[1];

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }

    public static void HandleDungeonCompletion(XTMessage message)
    {
        // WIP
    }
    #endregion

}
