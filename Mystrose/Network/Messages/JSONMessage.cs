namespace Mystrose.Network.Messages;

public class JSONMessage : Message
{

    #region Constructor
    public JSONMessage(ClientInstanceIdentifier identifier, string raw) : base(identifier)
    {
        RawContent = raw;
        Object = (JsonObject)JsonNode.Parse(raw)!;
        DataObject = (JsonObject)Object["b"]!["o"]!;
        Command = DataObject["cmd"]!.GetValue<string>()!;
    }
    #endregion

    #region Properties
    public JsonObject Object
    {
        get;
        private set;
    }

    public JsonObject DataObject
    {
        get;
        private set;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return Object.ToJsonString();
    }
    #endregion

}
