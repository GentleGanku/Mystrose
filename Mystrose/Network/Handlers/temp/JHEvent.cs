namespace Mystrose.Network.Handlers.JSON;

public class JHEvent : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "event"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        foreach (KeyValuePair<string, JsonNode> eventObj in message.DataObject)
        {
            EventMessage evtMsg = new(eventObj.Key, eventObj.Value);

            SVCScriptManager.InvokeTrigger(message.Identifier.Codename, evtMsg);
        }
    }
    #endregion

}
