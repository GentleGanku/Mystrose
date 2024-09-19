namespace Mystrose.Network.Messages;

public class XMLMessage : Message
{

    #region Constructor
    public XMLMessage(ClientUseIdentifier identifier, string raw) : base(identifier)
    {
        RawContent = raw;
        Body = new XmlDocument();
        Body.LoadXml(raw);
        Command = raw.Contains("cross-domain-policy") ? "policy" : Body.DocumentElement?["body"]?.Attributes["action"]?.Value;
    }
    #endregion

    #region Properties
    public XmlDocument Body
    {
        get;
        private set;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return Body.OuterXml;
    }
    #endregion

}
