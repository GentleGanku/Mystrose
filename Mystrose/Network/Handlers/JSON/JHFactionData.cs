namespace Mystrose.Network.Handlers.JSON;

public class JHFactionData() : MessageHandler<JSONMessage>(new()
{
    ["loadFactions"] = HandleLoadFactions,
    ["addFaction"] = HandleAddFaction
})
{

    #region Methods: Handlers
    private static void HandleLoadFactions(JSONMessage message)
    {
        List<Faction> factions = JsonSerializer.Deserialize<List<Faction>>(message.DataObject["factions"])!;

        message.HostWorld.Factions = new(factions);
    }

    private static void HandleAddFaction(JSONMessage message)
    {
        Faction faction = JsonSerializer.Deserialize<Faction>(message.DataObject["faction"])!;

        message.HostWorld.Factions.Add(faction);
    }
    #endregion

}
