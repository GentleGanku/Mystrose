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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "reloadmap":
                HandleReload(message);
                break;
            case "moveToArea":
                HandleMove(message);
                break;
        }
    }
    #endregion

    #region Methods: Reload
    private void HandleReload(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        if (world.Environment.Area.Format.Name != obj["sName"].Deserialize<string>())
        {
            return;
        }

        world.Environment.Area.Format.FilePath = obj["sFileName"].Deserialize<string>();
    }
    #endregion

    #region Methods: Movement
    private async void HandleMove(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Area area = obj.Deserialize<Area>();
        MapFormat mapFormat = obj.Deserialize<MapFormat>();

        area.Format = mapFormat;

        world.Environment.Area = area;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, area);
    }
    #endregion

}
