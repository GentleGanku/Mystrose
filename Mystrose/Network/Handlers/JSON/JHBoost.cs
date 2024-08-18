namespace Mystrose.Network.Handlers.JSON;

public class JHBoost : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "xpboost",
            "gboost",
            "repboost",
            "cpboost"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, JSONMessage message)
    {
        bool isActive = message.DataObject["op"].Deserialize<string>() == "+";

        switch (message.Command)
        {
            case "xpboost":
                host.World.Master.XPBoost = isActive;
                break;
            case "gboost":
                host.World.Master.GoldBoost = isActive;
                break;
            case "repboost":
                host.World.Master.RepBoost = isActive;
                break;
            case "cpboost":
                host.World.Master.CPBoost = isActive;
                break;
        }

        //host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Boost, host.World.BoostManager);
    }
    #endregion

}
