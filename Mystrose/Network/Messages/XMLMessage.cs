namespace Mystrose.Network.Messages;

public class XMLMessage : Message
{

    #region Constructor
    public XMLMessage(string raw)
    {
        try
        {
            RawContent = raw;
            Body = new XmlDocument();
            Body.LoadXml(raw);
            Command = raw.Contains("cross-domain-policy") ? "policy" : Body.DocumentElement?["body"]?.Attributes["action"]?.Value;
        }
        catch (XmlException e)
        {
            // WIP
        }
    }
    #endregion

    #region Destructor
    ~XMLMessage()
    {
        RawContent = null;
        Command = null;
        Body = null;
    }
    #endregion

    #region Properties
    public XmlDocument Body
    {
        get;
        private set;
    }
    #endregion

    #region Override Methods
    public override string ToString()
    {
        return Body.OuterXml;
    }
    #endregion

}
