namespace Mystrose.Network.Handlers.JSON;

public class JHEventMessage() : MessageHandler<JSONMessage>(new()
{
    ["event"] = HandleEventMessage
})
{

    #region Methods: Handlers
    private static void HandleEventMessage(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> eventObj in message.DataObject)
        {
            EventMessage evtMsg = new(eventObj.Key, eventObj.Value);

            MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, evtMsg);
        }
    }
    #endregion

}
