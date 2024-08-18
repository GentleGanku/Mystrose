namespace Mystrose.Network.Handlers.JSON;

public class JHStat : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "stu"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "stu":
                HandleStatUpdate(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Stat Update
    private void HandleStatUpdate(GameHost host, JsonObject obj)
    {
        host.World.Master.Stats.SetProperties(obj["sta"].Deserialize<JsonObject>());
    }
    #endregion

}
