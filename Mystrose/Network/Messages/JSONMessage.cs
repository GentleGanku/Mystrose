namespace Mystrose.Network.Messages;

public class JSONMessage : Message
{

    #region Constructor
    public JSONMessage(string raw)
    {
        try
        {
            RawContent = raw;
            Object = (JsonObject)JsonObject.Parse(raw);
            DataObject = (JsonObject)Object?["b"]?["o"];
            Command = DataObject?["cmd"]?.GetValue<string>();
        }
        catch (JsonException e)
        {
            // WIP
        }
    }
    #endregion

    #region Destructor
    ~JSONMessage()
    {
        RawContent = null;
        Command = null;
        Object = null;
        DataObject = null;
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

    #region Override Methods
    public override string ToString()
    {
        return Object?.ToJsonString();
    }
    #endregion

}
