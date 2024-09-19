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
    public void Handle(JSONMessage message)
    {
        World world = message.World;
        bool isActive = message.DataObject["op"].Deserialize<string>() == "+";

        switch (message.Command)
        {
            case "xpboost":
                world.Boosts.SetBoost(nameof(world.Boosts.ExpBoost), isActive);
                break;
            case "gboost":
                world.Boosts.SetBoost(nameof(world.Boosts.GoldBoost), isActive);
                break;
            case "repboost":
                world.Boosts.SetBoost(nameof(world.Boosts.RepBoost), isActive);
                break;
            case "cpboost":
                world.Boosts.SetBoost(nameof(world.Boosts.ClassBoost), isActive);  
                break;
        }

        //host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Boost, host.World.BoostManager);
    }
    #endregion

}
