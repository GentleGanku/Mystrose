using Mystrose.Services.Instantiable.Subservices;

namespace Mystrose.Services.Instantiable;

public class ISVCFlashAPI : InstantiableService
{

    #region Constructor
    public ISVCFlashAPI(ClientInstanceIdentifier identifier) : base(identifier, nameof(ISVCFlashAPI))
    {
        Construct();
    }
    #endregion

    #region Delegates & Handlers
    public delegate void CallHandler(string function, string args);
    public event CallHandler CallEvent;
    #endregion

    #region Fields
    private AxShockwaveFlash _client = new();
    #endregion

    #region Properties
    public SSVCBank Bank
    {
        get;
        private set;
    }

    public SSVCCombat Combat
    {
        get;
        private set;
    }
    
    public SSVCDrop Drop
    {
        get;
        private set;
    }
    
    public SSVCInventory Inventory
    {
        get;
        private set;
    }
    
    public SSVCMap Map
    {
        get;
        private set;
    }
    
    public SSVCQuest Quest
    {
        get;
        private set;
    }

    public SSVCServer Server
    {
        get;
        private set;
    }
    
    public SSVCShop Shop
    {
        get;
        private set;
    }
    
    
    #endregion

    #region (Private) Methods: Helper
    private void CheckupProgress(int percentile)
    {
        if (percentile < 100)
        {
            return;
        }

        //SVCGameManager.InstanceSelect(_identifier.Codename);
    }

    private string TamperServerInfo(string response)
    {
        JsonObject responsePacket = JsonSerializer.Deserialize<JsonObject>(response)!;

        if (responsePacket["bSuccess"] is not null && responsePacket["bSuccess"].Deserialize<int>() == 0)
        {
            return response;
        }

        JsonObject loginObj = responsePacket["login"].Deserialize<JsonObject>()!;

        loginObj["iAge"] = 99;
        loginObj["iEmailStatus"] = 5;

        RegisterServerInfo(responsePacket["servers"]!.ToJsonString());

        return responsePacket.ToJsonString();
    }

    private void RegisterServerInfo(string serverInfo)
    {
        List<Server> servers = JsonSerializer.Deserialize<List<Server>>(serverInfo)!;

        HSVCRepository.Instance.AddModel(servers);
    }

    private async void OpenExternalLink(string args)
    {
        if (args.StartsWith("async: "))
        {
            args = args.Replace("async: ", "");
            await Task.Delay(1000);
        }

        Process.Start(new ProcessStartInfo(args)
        {
            UseShellExecute = true
        });
    }
    #endregion

    #region (Private) Methods: Handlers
    private void OnCall(object sender, _IShockwaveFlashEvents_FlashCallEvent e)
    {
        XElement el = XElement.Parse(e.request);
        string? name = el.Attribute("name")?.Value;
        string? args = el.Element("arguments")?.Value;

        switch (name)
        {
            case "progress":
                CheckupProgress(int.Parse(args));
                break;
            case "registerServers":
                _client.SetReturnValue("<string>" + TamperServerInfo(args) + "</string>");
                break;
            case "openLink":
                OpenExternalLink(args);
                break;
        }

        CallEvent?.Invoke(name, args);
    }
    #endregion
    
    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            Bank = new(this);
            Drop = new(this);
            Inventory = new(this);
            Map = new(this);
            Quest = new(this);
            Shop = new(this);
            
            Log("Flash API has been constructed.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            _client.FlashCall -= OnCall;
            _client?.Dispose();

            Log("Flash API has been deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }

    public void Initialize(HSTGame game)
    {
        _client?.Dispose();

        _client = new AxShockwaveFlash()
        {
            Dock = DockStyle.Fill
        };

        _client.BeginInit();
        _client.FlashCall += OnCall;
        game.Child = _client;
        _client.EndInit();

        byte[] swf = Properties.Resources.Mystrose;
        MemoryStream stream = new();

        using BinaryWriter writer = new(stream);
        writer.Write(8 + swf.Length);
        writer.Write(1432769894);
        writer.Write(swf.Length);
        writer.Write(swf);
        writer.Seek(0, SeekOrigin.Begin);

        _client.OcxState = new AxHost.State(stream, 1, manualUpdate: false, null);

        Construct();
    }
    #endregion

    #region Methods: ActionScript
    /// <summary>
    /// Calls the Send Packet function in the actionscript object with the given packet string.
    /// </summary>
    /// <param name="packetString">The string of the packet to apply with.</param>
    public bool SendToServer(string packetString)
    {
        bool isValid = packetString switch
        {
            _ when packetString[0].Equals('{') => false,
            _ when packetString[0].Equals('<') => false,
            _ when packetString.Substring(1, 2).Equals("ct") => false,
            _ when packetString.Substring(1, 2).Equals("xt") => true,
            _ => false
        };

        if (isValid)
        {
            Call("sendPacket", packetString);
        }

        return isValid;
    }

    /// <summary>
    /// <summary>
    /// Calls the Send Client Packet function in the actionscript object with the given packet string.
    /// </summary>
    /// <param name="packetString">The string of the packet to apply with.</param>
    public bool SendToClient(string packetString)
    {
        bool isValid = packetString switch
        {
            _ when packetString[0].Equals('{') => true,
            _ when packetString[0].Equals('<') => true,
            _ when packetString.Substring(1, 2).Equals("ct") => false,
            _ when packetString.Substring(1, 2).Equals("xt") => true,
            _ => false
        };

        if (isValid)
        {
            Call("sendClientPacket", packetString);
        }

        return isValid;
    }

    /// <summary>
    /// Checks if the actionscript object at the given path is null.
    /// </summary>
    /// <param name="path">The path of the object to check.</param>
    /// <returns>True if the object at the given path is null (unset).</returns>
    public bool IsNull(string path)
    {
        return Call<bool>("isNull", path);
    }

    /// <summary>
    /// Gets an actionscript object at the given location as a JSON string.
    /// </summary>
    /// <param name="path">The path of the object to get.</param>
    /// <returns>The value of the object at the given path as a serialized JSON string.</returns>
    public string GetGameObject(string path)
    {
        if (path.Contains('['))
        {
            string key = path.Split(new char[] { '"', '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Last();
            string finalPath = path.Split('[')[0];
            return Call("getObjectKey", finalPath, key);
        }
        return Call("getGameObject", path);
    }

    /// <summary>
    /// Gets an actionscript object at the given location and deserializes it as JSON to the given type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the object to.</typeparam>
    /// <param name="path">The path of the object to get (i.e. world.myAvatar.sta.$tha will get your haste stat).</param>
    /// <param name="def">The default value to return if the call/deserialization fails.</param>
    /// <returns>The deserialized value of the object at the given path.</returns>
    public T GetGameObject<T>(string path, T def = default)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(GetGameObject(path)) ?? def;
        }
        catch
        {
            return def;
        }
    }

    /// <summary>
    /// Gets a static actionscript object at the given location as a JSON string.
    /// </summary>
    /// <param name="path">The path of the object to get.</param>
    /// <returns>The value of the object at the given path as a serialized JSON string.</returns>
    public string GetGameObjectStatic(string path)
    {
        return Call("getStaticObject", path);
    }

    /// <summary>
    /// Gets a static actionscript object at the given location and deserializes it as JSON to the given type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the object to.</typeparam>
    /// <param name="path">The path of the object to get.</param>
    /// <param name="def">The default value to return if the call/deserialization fails.</param>
    /// <returns>The deserialized value of the object at the given path.</returns>
    public T GetGameObjectStatic<T>(string path, T def = default)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(GetGameObjectStatic(path));
        }
        catch
        {
            return def;
        }
    }

    /// <summary>
    /// Sets the value of the actionscript object at the given path.
    /// </summary>
    /// <param name="path">The path of the object to set.</param>
    /// <param name="value">The value to set the object to. This can be a string, any number type or a bool.</param>
    public void SetGameObject(string path, object value)
    {
        if (path.Contains('['))
        {
            string key = path.Split(new char[] { '"', '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Last();
            string finalPath = path.Split('[')[0];
            Call("setObjectKey", finalPath, key, value);
            return;
        }
        Call("setGameObject", path, value);
    }

    /// <summary>
    /// Calls the actionscript object with the given name at the given location.
    /// </summary>
    /// <param name="path">The path to the object and its function name.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The value of the object returned by calling the function as a serialzied JSON string.</returns>
    public string CallGameFunction(string path, params object[] args)
    {
        return args.Length > 0 ? Call("callGameFunctionByArgs", [path, true, .. args]) : Call("callGameFunction", path, true);
    }

    /// <summary>
    /// Calls the actionscript object with the given name at the given location.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the return of the function as.</typeparam>
    /// <param name="path">The path to the object and its function name.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The deserialized value of the object returned by the function.</returns>
    public T CallGameFunction<T>(string path, params object[] args)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(CallGameFunction(path, args));
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Calls the actionscript object with the given name at the given location.
    /// </summary>
    /// <param name="path">The path to the first object and its function name.</param>
    /// <param name="path2">The path to the second object and its function name.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The value of the object returned by calling the function as a serialzied JSON string.</returns>
    public string CallGameFunctionOnFunc(string path, string path2, params object[] args)
    {
        return args.Length > 0 ? Call("callGameFunctionOnFuncArgs", [path, path2, true, .. args]) : Call("callGameFunctionOnFunc", path, path2, true);
    }

    /// <summary>
    /// Calls the actionscript object with the given name at the given location.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the return of the function as.</typeparam>
    /// <param name="path">The path to the first object and its function name.</param>
    /// <param name="path2">The path to the second object and its function name.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The deserialized value of the object returned by the function.</returns>
    public T CallGameFunctionOnFunc<T>(string path, string path2, params object[] args)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(CallGameFunctionOnFunc(path, path2, args));
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Gets the actionscript object of the array at the given path at the given index in that array.
    /// </summary>
    /// <param name="path">The path to the array.</param>
    /// <param name="index">The index in the array to get the object from.</param>
    /// <returns>The value of the object at the given index in the array as a serialzied JSON string.</returns>
    public string GetArrayObject(string path, int index)
    {
        return Call("getArrayObject", path, index);
    }

    /// <summary>
    /// Gets the actionscript object of the array at the given path at the given index in that array.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the object in the array as.</typeparam>
    /// <param name="path">The path to the array.</param>
    /// <param name="index">The index in the array to get the object from.</param>
    /// <param name="def">The default value to return if the call/deserialization fails.</param>
    /// <returns>The deserialized value of the object at the given index in the array.</returns>
    public T GetArrayObject<T>(string path, int index, T def = default)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(GetArrayObject(path, index));
        }
        catch
        {
            return def;
        }
    }

    /// <summary>
    /// Selects the members of each object in the array at the given path and puts them into a new array and returns them.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the contents of the array as.</typeparam>
    /// <param name="path">The path to the array.</param>
    /// <param name="selector">The name of the field to use to populate the new array.</param>
    /// <returns>A list of deserialized objects from the selected array.</returns>
    public List<T> SelectArrayObjects<T>(string path, string selector)
    {
        try
        {
            return JsonSerializer.Deserialize<List<T>>(Call("selectArrayObjects", path, selector));
        }
        catch
        {
            return new List<T>();
        }
    }

    public string Call(string function, params object[] args)
    {
        return Call<string>(function, args);
    }

    public T Call<T>(string function, params object[] args)
    {
        try
        {
            object o = Call(function, typeof(T), args);
            if (o != null)
            {
                return (T)o;
            }
            return default;
        }
        catch
        {
            return default;
        }
    }

    public object Call(string function, Type type, params object[] args)
    {
        try
        {
            StringBuilder req = new StringBuilder().Append($"<invoke name=\"{function}\" returntype=\"xml\">");
            if (args.Length > 0)
            {
                req.Append("<arguments>");
                args.ToList().ForEach(o => req.Append(ToFlashXml(o)));
                req.Append("</arguments>");
            }
            req.Append("</invoke>");
            string result = _client.CallFunction(req.ToString());
            XElement el = XElement.Parse(result);
            return el == null || el.FirstNode == null ? default : Convert.ChangeType(el.FirstNode.ToString(), type);
        }
        catch (Exception e)
        {
            HSVCLogger.Instance.LogOnException("(Flash Call) " + e.ToString());
            return default;
        }
    }

    public string ToFlashXml(object o)
    {
        switch (o)
        {
            case null:
                return "<null/>";
            case bool _:
                return $"<{o.ToString().ToLower()}/>";
            case double _:
            case float _:
            case long _:
            case int _:
                return $"<number>{o}</number>";
            case ExpandoObject _:
                StringBuilder sb = new StringBuilder().Append("<object>");
                foreach (KeyValuePair<string, object> kvp in o as IDictionary<string, object>)
                {
                    sb.Append($"<property id=\"{kvp.Key}\">{ToFlashXml(kvp.Value)}</property>");
                }
                return sb.Append("</object>").ToString();
            default:
                if (o is Array)
                {
                    StringBuilder _sb = new StringBuilder().Append("<array>");
                    int k = 0;
                    foreach (object el in o as Array)
                        _sb.Append($"<property id=\"{k++}\">{ToFlashXml(el)}</property>");
                    return _sb.Append("</array>").ToString();
                }
                return $"<string>{SecurityElement.Escape(o.ToString())}</string>";
        }
    }

    public object FromFlashXml(XElement el)
    {
        switch (el.Name.ToString())
        {
            case "number":
                return int.TryParse(el.Value, out int i) ? i : float.TryParse(el.Value, out float f) ? f : 0;
            case "true":
                return true;
            case "false":
                return false;
            case "null":
                return null;
            case "array":
                return el.Elements().Select(e => FromFlashXml(e)).ToArray();
            case "object":
                dynamic d = new ExpandoObject();
                el.Elements().ToList().ForEach(e => d[e.Attribute("id").Value] = FromFlashXml(e.Elements().First()));
                return d;
            default:
                return el.Value;
        }
    }
    #endregion

}