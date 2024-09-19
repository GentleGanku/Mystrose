using Message = Mystrose.Network.Messages.Message;

namespace Mystrose.Network;

public class NetworkMonitor
{

    #region Constructor
    public NetworkMonitor()
    {
        Initialize();
    }
    #endregion

    #region Delegates & Handlers
    public delegate void PacketHandler(GameHost host, string args);
    public event PacketHandler ServerPacketEvent;
    public event PacketHandler ClientPacketEvent;
    #endregion

    #region Fields: Handlers
    private readonly List<IJSONMessageHandler> JSONHandlers = new()
    {
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
    };

    private readonly List<IXMLMessageHandler> XMLHandlers = new()
    {
        // Empty 
    };

    private readonly List<IXTMessageHandler> XTHandlers = new()
    {
        new XHAction(),
        new XHDungeon(),
        new XHMap(),
        new XHRespawn(),
        new XHResponse()
    };

    private readonly List<IZMMessageHandler> ZMHandlers = new()
    {
        new ZHMovement(),
        new ZHSkill()
    };
    #endregion

    #region Methods: Setup
    public void Initialize()
    {
        ServerPacketEvent += InterceptServerPacket;
        ClientPacketEvent += InterceptClientPacket;
    }

    public void Dispose()
    {
        ServerPacketEvent -= InterceptServerPacket;
        ClientPacketEvent -= InterceptClientPacket;

        JSONHandlers.Clear();
        XMLHandlers.Clear();
        XTHandlers.Clear();
        ZMHandlers.Clear();
    }
    #endregion

    #region Methods: Interceptors
    private async void InterceptServerPacket(GameHost host, string args)
    {
    //    await Task.Delay(10);

    //    Message message = args switch
    //    {
    //        _ when args[0].Equals('{') => new JSONMessage(args),
    //        _ when args[0].Equals('<') => new XMLMessage(args),
    //        _ when args.Substring(4, 2).Equals("zm") => new ZMMessage(args),
    //        _ => new XTMessage(args)
    //    };

    //    switch (message)
    //    {
    //        case JSONMessage jsonMessage:
    //            foreach (var handler in JSONHandlers.Where(h => h.HandledCommands.Contains(jsonMessage.Command)))
    //            {
    //                try
    //                {
    //                    handler.Handle(host, jsonMessage);
    //                }
    //                catch (Exception e)
    //                {
    //                    SVCLogger.LogOnException($"(JSONMessage - {jsonMessage.Command}) " + e.ToString());
    //                }
    //            }
    //            break;
    //        case XMLMessage xmlMessage:
    //            foreach (var handler in XMLHandlers.Where(h => h.HandledCommands.Contains(xmlMessage.Command)))
    //            {
    //                try
    //                {
    //                    handler.Handle(host, xmlMessage);
    //                }
    //                catch (Exception e)
    //                {
    //                    SVCLogger.LogOnException($"(XMLMessage - {xmlMessage.Command}) " + e.ToString());
    //                }
    //            }
    //            break;
    //        case XTMessage xtMessage:
    //            foreach (var handler in XTHandlers.Where(h => h.HandledCommands.Contains(xtMessage.Command)))
    //            {
    //                try
    //                {
    //                    handler.Handle(host, xtMessage);
    //                }
    //                catch (Exception e)
    //                {
    //                    SVCLogger.LogOnException($"(XTMessage - {xtMessage.Command}) " + e.ToString());
    //                }
    //            }
    //            break;
    //        case ZMMessage zmMessage:
    //            foreach (var handler in ZMHandlers.Where(h => h.HandledCommands.Contains(zmMessage.Command)))
    //            {
    //                try
    //                {
    //                    handler.Handle(host, zmMessage);
    //                }
    //                catch (Exception e)
    //                {
    //                    SVCLogger.LogOnException($"(ZMMessage - {zmMessage.Command}) " + e.ToString());
    //                }
    //            }
    //            break;
    //    }
    }

    protected internal void InterceptClientPacket(GameHost host, string args)
    {
        // WIP
    }
    #endregion

}