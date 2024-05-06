using Mystrose.Controls.Main;
using Mystrose.GameModels.Environment;
using Mystrose.Network.Messages;
using Mystrose.Network.Messages.Interfaces;
using Mystrose.ScriptMachine.Enumerations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Mystrose.Network.Handlers.JSON;

public class JHMap : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "reloadmap",
            "moveToArea"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "reloadmap":
                HandleReload(host, message.DataObject);
                break;
            case "moveToArea":
                HandleMove(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Reload
    private void HandleReload(GameHost host, JsonObject obj)
    {
        if (host.World.Area.Format.Name != obj["sName"].Deserialize<string>())
        {
            return;
        }

        host.World.Area.Format.FilePath = obj["sFileName"].Deserialize<string>();
    }
    #endregion

    #region Methods: Movement
    private async void HandleMove(GameHost host, JsonObject obj)
    {
        Area area = obj.Deserialize<Area>();
        MapFormat mapFormat = obj.Deserialize<MapFormat>();

        area.Format = mapFormat;

        host.World.Area = area;
        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Map, area);
    }
    #endregion

}
