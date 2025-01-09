namespace Mystrose.Services.Manager;

public class MSVCInterceptor() : ManagerService<InterceptorMessage[]>(nameof(MSVCInterceptor))
{

    #region Delegates & Handlers
    public delegate void InterceptHandler(string codename, InterceptorMessage response);
    public event InterceptHandler InterceptEvent;
    #endregion

    #region (Static) Fields
    public static MSVCInterceptor Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCInterceptor();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCInterceptor? _instance;
    #endregion

    #region Fields
    private bool _isIntercepting = false;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
    #endregion

    #region Methods: Interception
    public Response<bool> GetInterceptionStatus()
    {
        string[] viewsResponse = [.. MSVCView.Instance.Collection.Keys];
        if (viewsResponse.FirstOrDefault(v => v.Equals(nameof(VWPacketInterceptor))) is null)
        {
            return new(false,
                "Packet interceptor view is not loaded. Please load the view first.",
                false);
        }

        return new(true,
            "Packet interceptor is currently " + (_isIntercepting ? "enabled" : "disabled") + ".",
            _isIntercepting);
    }

    public Response<bool> SetIntercepting(bool isIntercepting)
    {
        string[] viewsResponse = [.. MSVCView.Instance.Collection.Keys];
        if (viewsResponse.FirstOrDefault(v => v.Equals(nameof(VWPacketInterceptor))) is null)
        {
            return new(false,
                "Packet interceptor view is not loaded. Please load the view first.",
                false);
        }

        _isIntercepting = isIntercepting;

        return new(true,
            "Packet interceptor is now " + (_isIntercepting ? "enabled" : "disabled") + ".",
            _isIntercepting);
    }
    #endregion

    #region Methods: Formatting
    public Response<string> FormatInJson(string codename, int index)
    {
        Response<InterceptorMessage?> response = ReadIntercept(codename, index);
        if (!response.IsSuccess)
        {
            return new(false,
                response.Message,
                string.Empty);
        }

        InterceptorMessage message = (InterceptorMessage)response.Output!;
        if (message.Type != 0)
        {
            return new(false,
                "The message is not a JSON packet.",
                string.Empty);
        }

        try
        {
            JsonObject jsonPacket = JsonSerializer.Deserialize<JsonObject>(message.Message)!;
            string formattedPacket = JsonSerializer.Serialize(jsonPacket, _jsonOptions);

            return new(true,
                "JSON Packet successfully formatted.",
                formattedPacket);
        }
        catch (JsonException ex)
        {
            return new(false,
                "Failed to format JSON packet: " + ex.Message,
                string.Empty);
        }
    }

    public Response<string> FormatInJson(InterceptorMessage message)
    {
        if (message.Type != 0)
        {
            return new(false,
                "The message is not a JSON packet.",
                string.Empty);
        }

        try
        {
            JsonObject jsonPacket = JsonSerializer.Deserialize<JsonObject>(message.Message)!;
            string formattedPacket = JsonSerializer.Serialize(jsonPacket, _jsonOptions);

            return new(true,
                "JSON Packet successfully formatted.",
                formattedPacket);
        }
        catch (JsonException ex)
        {
            return new(false,
                "Failed to format JSON packet: " + ex.Message,
                string.Empty);
        }
    }

    public Response<string> FormatInXml(string codename, int index)
    {
        Response<InterceptorMessage?> response = ReadIntercept(codename, index);
        if (!response.IsSuccess)
        {
            return new(false,
                response.Message,
                string.Empty);
        }

        InterceptorMessage message = (InterceptorMessage)response.Output!;
        if (message.Type != 1)
        {
            return new(false,
                "The message is not an XML packet.",
                string.Empty);
        }

        try
        {
            XElement xElement = XElement.Parse(message.Message);

            return new(true,
                "XML Packet successfully formatted.",
                xElement.ToString());
        }
        catch (XmlException ex)
        {
            return new(false,
                "Failed to format XML packet: " + ex.Message,
                string.Empty);
        }
    }

    public Response<string> FormatInXml(InterceptorMessage message)
    {
        if (message.Type != 1)
        {
            return new(false,
                "The message is not an XML packet.",
                string.Empty);
        }

        try
        {
            XElement xElement = XElement.Parse(message.Message);

            return new(true,
                "XML Packet successfully formatted.",
                xElement.ToString());
        }
        catch (XmlException ex)
        {
            return new(false,
                "Failed to format XML packet: " + ex.Message,
                string.Empty);
        }
    }

    public Response<string> FormatInXt(string codename, int index)
    {
        Response<InterceptorMessage?> response = ReadIntercept(codename, index);
        if (!response.IsSuccess)
        {
            return new(false,
                response.Message,
                string.Empty);
        }

        InterceptorMessage message = (InterceptorMessage)response.Output!;
        if (message.Type != 2)
        {
            return new(false,
                "The message is not an XT packet.",
                string.Empty);
        }

        string[] xtContent = message.Message.Split("%").Skip(1).ToArray();
        string formattedPacket = string.Join("\r\n", xtContent);

        return new(true,
            "XT Packet successfully formatted.",
            formattedPacket);
    }

    public Response<string> FormatInXt(InterceptorMessage message)
    {
        if (message.Type != 2)
        {
            return new(false,
                "The message is not an XT packet.",
                string.Empty);
        }

        string[] xtContent = message.Message.Split("%").Skip(1).ToArray();
        string formattedPacket = string.Join("\r\n", xtContent);

        return new(true,
            "XT Packet successfully formatted.",
            formattedPacket);
    }

    public Response<string> FormatInZm(string codename, int index)
    {
        Response<InterceptorMessage?> response = ReadIntercept(codename, index);
        if (!response.IsSuccess)
        {
            return new(false,
                response.Message,
                string.Empty);
        }

        InterceptorMessage message = (InterceptorMessage)response.Output!;
        if (message.Type != 3)
        {
            return new(false,
                "The message is not a ZM packet.",
                string.Empty);
        }

        string[] zmContent = message.Message.Split("%").Skip(1).ToArray();
        string formattedPacket = string.Join("\r\n", zmContent);

        return new(true,
            "ZM Packet successfully formatted.",
            formattedPacket);
    }

    public Response<string> FormatInZm(InterceptorMessage message)
    {
        if (message.Type != 3)
        {
            return new(false,
                "The message is not a ZM packet.",
                string.Empty);
        }

        string[] zmContent = message.Message.Split("%").Skip(1).ToArray();
        string formattedPacket = string.Join("\r\n", zmContent);

        return new(true,
            "ZM Packet successfully formatted.",
            formattedPacket);
    }
    #endregion

    #region Methods: Read/Write
    public Response<InterceptorMessage?> WriteInJson(string codename, string command, string text)
    {
        if (!GetInterceptionStatus().Output)
        {
            return new(false,
                "The packet interceptor is currently disabled.",
                null);
        }

        InterceptorMessage msg = new(0, command, text);

        Items[codename] = [.. Items[codename], msg];
        InterceptEvent?.Invoke(codename, msg);

        return new(true,
            "JSON Packet written successfully to interceptor.",
            msg);
    }

    public Response<InterceptorMessage?> WriteInXml(string codename, string command, string text)
    {
        if (!GetInterceptionStatus().Output)
        {
            return new(false,
                "The packet interceptor is currently disabled.",
                null);
        }

        InterceptorMessage msg = new(1, command, text);

        Items[codename] = [.. Items[codename], msg];
        InterceptEvent?.Invoke(codename, msg);

        return new(true,
            "XML Packet written successfully to interceptor.",
            msg);
    }

    public Response<InterceptorMessage?> WriteInXt(string codename, string command, string text)
    {
        if (!GetInterceptionStatus().Output)
        {
            return new(false,
                "The packet interceptor is currently disabled.",
                null);
        }

        InterceptorMessage msg = new(2, command, text);

        Items[codename] = [.. Items[codename], msg];
        InterceptEvent?.Invoke(codename, msg);

        return new(true,
            "XT Packet written successfully to interceptor.",
            msg);
    }

    public Response<InterceptorMessage?> WriteInZm(string codename, string command, string text)
    {
        if (!GetInterceptionStatus().Output)
        {
            return new(false,
                "The packet interceptor is currently disabled.",
                null);
        }

        InterceptorMessage msg = new(3, command, text);

        Items[codename] = [.. Items[codename], msg];
        InterceptEvent?.Invoke(codename, msg);

        return new(true,
            "ZM Packet written successfully to interceptor.",
            msg);
    }
    #endregion

    #region Methods: Utility
    public Response<InterceptorMessage[]> ReadIntercepts(string codename)
    {
        if (!Items.ContainsKey(codename))
        {
            return new(false,
                "The codename provided does not exist in the packet interceptors.",
                []);
        }

        return new(true,
            "Packet interceptors successfully read.",
            Items[codename]!);
    }

    public Response<InterceptorMessage?> ReadIntercept(string codename, int index)
    {
        if (!Items.ContainsKey(codename))
        {
            return new(false,
                "The codename provided does not exist in the packet interceptors.",
                null);
        }

        if (index < 0 || index >= Items[codename]!.Length)
        {
            return new(false,
                "The index provided is out of bounds for the packet interceptors.",
                null);
        }

        return new(true,
            "Packet interceptor successfully read.",
            Items[codename]![index]);
    }

    public Response<InterceptorMessage[]> ClearIntercepts(string codename)
    {
        if (!Items.ContainsKey(codename))
        {
            return new(false,
                "The codename provided does not exist in the packet interceptors.",
                []);
        }

        Response<InterceptorMessage[]> response = Items[codename]!.Length > 0
            ? new(true,
                "Packet interceptors successfully cleared.",
                Items[codename]!)
            : new(false,
                "No packet interceptors to clear.",
                []);

        Items[codename] = [];

        return response;
    }
    #endregion

}
