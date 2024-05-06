using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace Mystrose.Network.Handlers.JSON;

public class HLoadInventoryBig : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "loadInventoryBig"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        host.World.Master.UsedBankSlots = message.DataObject["bankCount"].GetValue<int>();
        host.World.Inventory.AddRange(message.DataObject["items"].Deserialize<List<InventoryItem>>());
        host.World.HouseInventory.AddRange(message.DataObject["hitems"].Deserialize<List<InventoryItem>>());
        host.World.Master.Factions = message.DataObject["factions"].Deserialize<List<Faction>>();
    }
    #endregion

}
