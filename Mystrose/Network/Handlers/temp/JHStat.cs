namespace Mystrose.Network.Handlers.temp;

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
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "stu":
                HandleStatUpdate(message);
                break;
        }
    }
    #endregion

    #region Methods: Stat Update
    private void HandleStatUpdate(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        world.Avatar.Stats.SetProperties(obj["sta"].Deserialize<JsonObject>()!);
    }
    #endregion

}
