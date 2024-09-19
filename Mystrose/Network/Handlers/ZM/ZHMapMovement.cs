namespace Mystrose.Network.Handlers.ZM;

public static class ZHMapMovement
{

    #region Fields
    private static readonly Dictionary<string, Action<ZMMessage>> _handlers = new()
    {
        ["mv"] = HandlePositionChange,
        ["moveToCell"] = HandleFrameChange,
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(ZMMessage message)
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
    public static void HandlePositionChange(ZMMessage message)
    {
        double x = double.Parse(message.Arguments[5]);
        double y = double.Parse(message.Arguments[6]);

        message.World.Avatar.X = x;
        message.World.Avatar.Y = y;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }

    public static void HandleFrameChange(ZMMessage message)
    {
        string cell = message.Arguments[5];
        string pad = message.Arguments[6];

        message.World.Avatar.Cell = cell;
        message.World.Avatar.Pad = pad;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Avatar);
    }
    #endregion

}
