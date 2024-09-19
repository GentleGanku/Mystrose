namespace Mystrose.Network.Handlers.JSON;

public static class JHArea
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["moveToArea"] = HandleArea
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
    public static void HandleArea(JSONMessage message)
    {
        Area area = message.DataObject.Deserialize<Area>()!;
        MapFormat mapFormat = message.DataObject.Deserialize<MapFormat>()!;

        area.Format = mapFormat;
        message.World.Area = area;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, area);
        SVCRepository.AddModel([mapFormat]);
    }
    #endregion

}
