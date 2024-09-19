namespace Mystrose.Network.Handlers.JSON;

public static class JHFactionData
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["loadFactions"] = HandleLoadFactions,
        ["addFaction"] = HandleAddFaction
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
    public static void HandleLoadFactions(JSONMessage message)
    {
        List<Faction> factions = JsonSerializer.Deserialize<List<Faction>>(message.DataObject["factions"])!;

        message.World.Factions = new(factions);
    }

    public static void HandleAddFaction(JSONMessage message)
    {
        Faction faction = JsonSerializer.Deserialize<Faction>(message.DataObject["faction"])!;

        message.World.Factions.Add(faction);
    }
    #endregion

}
