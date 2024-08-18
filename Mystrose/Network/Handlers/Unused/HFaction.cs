namespace Mystrose.Network.Handlers.Unused;

public class HFaction : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "loadFactions",
            "addFaction"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "loadFactions":
                host.World.Master.Factions = JsonSerializer.Deserialize<List<Faction>>(message.DataObject["factions"]);
                break;
            case "addFaction":
                Faction faction = JsonSerializer.Deserialize<Faction>(message.DataObject["faction"]);
                host.World.Master.Factions.Add(faction);
                break;
        }
    }
    #endregion

}
