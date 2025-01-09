namespace Mystrose.Network.Handlers.JSON;

public class JHBoostStatus() : MessageHandler<JSONMessage>(new()
{
    ["xpboost"] = HandleBoost,
    ["gboost"] = HandleBoost,
    ["repboost"] = HandleBoost,
    ["cpboost"] = HandleBoost
})
{

    #region Methods: Handlers
    private static void HandleBoost(JSONMessage message)
    {
        bool isActive = message.DataObject["op"].Deserialize<string>() == "+";

        message.HostWorld.Boosts.SetBoost(message.Command, isActive);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Boosts);
    }
    #endregion

}
