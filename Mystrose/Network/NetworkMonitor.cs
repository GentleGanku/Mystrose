using Message = Mystrose.Network.Messages.Message;

namespace Mystrose.Network;

public class NetworkMonitor
{

    #region Constructor
    public NetworkMonitor()
    {
        JSONHandlers =
        [
            // new TestJSON()
        ];
        XMLHandlers =
        [

        ];
        XTHandlers =
        [
            // new TestXT()
        ];
        ZMHandlers =
        [
            // new TestZM()
        ];

        JSONHandlers.AddRange(JSONBaseHandlers);
        XMLHandlers.AddRange(XMLBaseHandlers);
        XTHandlers.AddRange(XTBaseHandlers);
        ZMHandlers.AddRange(ZMBaseHandlers);

        GameEvent += InterceptPacket;
        GameEvent += HandlePacket;
        ClientEvent += InterceptClient;
        ClientEvent += HandleClient;
    }
    #endregion

    #region Destructor
    ~NetworkMonitor()
    {
        JSONHandlers = null;
        XMLHandlers = null;
        XTHandlers = null;
        ZMHandlers = null;

        GameEvent -= InterceptPacket;
        GameEvent -= HandlePacket;
        GameEvent = null;

        ClientEvent -= InterceptClient;
        ClientEvent -= HandleClient;
        ClientEvent = null;
    }
    #endregion

    #region Delegates
    public delegate void TrafficHandler(GameHost host, string args);
    #endregion

    #region Event Handlers
    public event TrafficHandler GameEvent;
    public event TrafficHandler ClientEvent;
    #endregion

    #region Properties: Handlers
    protected internal List<IJSONMessageHandler>? JSONHandlers
    {
        get;
        private set;
    }

    protected internal List<IXMLMessageHandler>? XMLHandlers
    {
        get;
        private set;
    }

    protected internal List<IXTMessageHandler>? XTHandlers
    {
        get;
        private set;
    }

    protected internal List<IZMMessageHandler>? ZMHandlers
    {
        get;
        private set;
    }
    #endregion

    #region Properties: Base Handlers
    protected internal List<IJSONMessageHandler> JSONBaseHandlers
    {
        get;
        set;
    } =
    [
        new JHAction(),
        new JHBoost(),
        new JHClass(),
        new JHEntityData(),
        new JHEvent(),
        new JHGender(),
        // new JHItemDrop(),
        new JHLevel(),
        new JHMap(),
        new JHParty(),
        new JHQuest(),
        new JHSkill()
    ];

    protected internal List<IXMLMessageHandler> XMLBaseHandlers
    {
        get;
        set;
    } =
    [
    ];

    protected internal List<IXTMessageHandler> XTBaseHandlers
    {
        get;
        set;
    } =
    [
        new XHAction(),
        new XHDungeon(),
        new XHMap(),
        new XHRespawn(),
        new XHResponse()
    ];

    protected internal List<IZMMessageHandler> ZMBaseHandlers
    {
        get;
        set;
    } =
    [
        new ZHMovement(),
        new ZHSkill()
    ];
    #endregion

    #region Methods: Invoke
    protected internal void InvokeEvent(NetworkHandlerType handlerType, GameHost host, string args)
    {
        (handlerType switch
        {
            NetworkHandlerType.Game => GameEvent,
            NetworkHandlerType.Client => ClientEvent,
            _ => null
        })?.Invoke(host, args);
    }
    #endregion

    #region Methods: Register
    public void RegisterHandler(object[] handlers)
    {
        foreach (var handler in handlers)
        {
            switch (handler)
            {
                case IJSONMessageHandler JSONHandler:
                    if (!JSONHandlers.Contains(JSONHandler))
                    {
                        JSONHandlers.Add(JSONHandler);
                    }
                    break;
                case IXMLMessageHandler XMLHandler:
                    if (!XMLHandlers.Contains(XMLHandler))
                    {
                        XMLHandlers.Add(XMLHandler);
                    }
                    break;
                case IXTMessageHandler XTHandler:
                    if (!XTHandlers.Contains(XTHandler))
                    {
                        XTHandlers.Add(XTHandler);
                    }
                    break;
                case IZMMessageHandler ZMHandler:
                    if (!ZMHandlers.Contains(ZMHandler))
                    {
                        ZMHandlers.Add(ZMHandler);
                    }
                    break;
                default:
                    // WIP
                    break;
            }
        }
    }
    #endregion

    #region Methods: Unregister
    public void UnregisterHandler(object[] handlers)
    {
        foreach (var handler in handlers)
        {
            switch (handler)
            {
                case IJSONMessageHandler JSONHandler:
                    if (JSONHandlers.Contains(JSONHandler))
                    {
                        JSONHandlers.Remove(JSONHandler);
                    }
                    break;
                case IXMLMessageHandler XMLHandler:
                    if (XMLHandlers.Contains(XMLHandler))
                    {
                        XMLHandlers.Remove(XMLHandler);
                    }
                    break;
                case IXTMessageHandler XTHandler:
                    if (XTHandlers.Contains(XTHandler))
                    {
                        XTHandlers.Remove(XTHandler);
                    }
                    break;
                case IZMMessageHandler ZMHandler:
                    if (ZMHandlers.Contains(ZMHandler))
                    {
                        ZMHandlers.Remove(ZMHandler);
                    }
                    break;
                default:
                    // WIP
                    break;
            }
        }
    }
    #endregion

    #region Methods: Packet Interceptor
    protected internal void InterceptPacket(GameHost host, string args)
    {
        // WIP
    }

    protected internal async void HandlePacket(GameHost host, string args)
    {
        await Task.Delay(10);

        Message message = args switch
        {
            _ when args[0] == '{' => new JSONMessage(args),
            _ when args[0] == '<' => new XMLMessage(args),
            _ when args.Substring(4, 2) == "zm" => new ZMMessage(args),
            _ => new XTMessage(args)
        };

        switch (message)
        {
            case JSONMessage jsonMessage:
                foreach (var handler in JSONHandlers.Where(h => h.HandledCommands.Contains(jsonMessage.Command)))
                {
                    try
                    {
                        handler.Handle(host, jsonMessage);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
                break;
            case XMLMessage xmlMessage:
                foreach (var handler in XMLHandlers.Where(h => h.HandledCommands.Contains(xmlMessage.Command)))
                {
                    try
                    {
                        handler.Handle(host, xmlMessage);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
                break;
            case XTMessage xtMessage:
                foreach (var handler in XTHandlers.Where(h => h.HandledCommands.Contains(xtMessage.Command)))
                {
                    try
                    {
                        handler.Handle(host, xtMessage);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
                break;
            case ZMMessage zmMessage:
                foreach (var handler in ZMHandlers.Where(h => h.HandledCommands.Contains(zmMessage.Command)))
                {
                    try
                    {
                        handler.Handle(host, zmMessage);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
                break;
        }
    }
    #endregion

    #region Methods: Client Interceptor
    protected internal void InterceptClient(GameHost host, string args)
    {
        // WIP
    }

    protected internal void HandleClient(GameHost host, string args)
    {
        // WIP
    }
    #endregion

}