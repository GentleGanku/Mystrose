using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

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
