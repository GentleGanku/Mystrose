using Mystrose.Controls.Main;
using Mystrose.GameModels.Preference;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class HPreferences : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "savePrefs",
            "loadPrefs"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "savePrefs":
                HandleSave(host, message.DataObject);
                break;
            case "loadPrefs":
                HandleLoad(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Save
    private void HandleSave(GameHost host, JsonObject dataObject)
    {
        switch (dataObject["section"].GetValue<string>())
        {
            case "boosts":
                break;
            case "loadouts":
                break;
            case "prefs":
                break;
        };
    }
    #endregion

    #region Methods: Load
    private void HandleLoad(GameHost host, JsonObject dataObject)
    {
        bool isSuccess = dataObject["success"].GetValue<bool>();

        if (!isSuccess)
        {
            return;
        }

        foreach (KeyValuePair<string, JsonNode> obj in (JsonObject)dataObject["result"])
        {
            switch (obj.Key)
            {
                case "costumes":
                    break;
                case "loadouts":
                    foreach (KeyValuePair<string, JsonNode> loadoutObj in (JsonObject)obj.Value)
                    {
                        Loadout loadout = loadoutObj.Value.Deserialize<Loadout>();
                        host.World.Master.Loadouts.Add(loadoutObj.Key, loadout);
                    }
                    break;
                case "prefs":
                    break;
            }
        }
    }
    #endregion

}
