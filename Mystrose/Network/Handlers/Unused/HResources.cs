using Mystrose.Controls.Main;
using Mystrose.GameModels.General;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class HResources : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "balance",
            "addGoldExp"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "balance":
                HandleBalance(host, message.DataObject);
                break;
            case "addGoldExp":
                HandleAdd(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Balance
    private void HandleBalance(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> resourceObj in obj)
        {
            switch (resourceObj.Key)
            {
                case "intGold":
                    host.World.Master.Gold = obj[resourceObj.Key].GetValue<int>();
                    break;
                case "intCoins":
                    host.World.Master.AdventureCoins = obj[resourceObj.Key].GetValue<int>();
                    break;
            }
        }
    }
    #endregion

    #region Methods: Add
    private void HandleAdd(GameHost host, JsonObject obj)
    {
        foreach (KeyValuePair<string, JsonNode> resourceObj in obj)
        {
            switch (resourceObj.Key)
            {
                case "intGold":
                case "bonusGold":
                    host.World.Master.Gold += obj[resourceObj.Key].GetValue<int>();
                    break;
                case "iCP":
                case "bonusCP":
                    host.World.Master.ClassPoints += obj[resourceObj.Key].GetValue<int>();
                    break;
                case "iRep":
                case "bonusRep":
                    int factionId = obj["FactionID"].GetValue<int>();

                    Faction? faction = host.World.Master.Factions.Find(
                        (fct) =>
                        {
                            return fct.ID == factionId;
                        });

                    if (faction == null)
                    {
                        return;
                    }

                    faction.Points += obj[resourceObj.Key].GetValue<int>();
                    break;
            }
        }
    }
    #endregion

}
