using Message = Mystrose.Network.Messages.Message;

namespace Mystrose.Services.Instantiable;

public class ISVCNetwork : InstantiableService
{

    #region Constructor
    public ISVCNetwork(ClientInstanceIdentifier identifier) : base(identifier, nameof(ISVCNetwork))
    {
        Construct();
    }
    #endregion

    #region Delegates & Handlers
    public delegate void JSONMessageHandler(JSONMessage message);
    public event JSONMessageHandler JSONMessageEvent;

    public delegate void XMLMessageHandler(XMLMessage message);
    public event XMLMessageHandler XMLMessageEvent;

    public delegate void XTMessageHandler(XTMessage message);
    public event XTMessageHandler XTMessageEvent;

    public delegate void ZMMessageHandler(ZMMessage message);
    public event ZMMessageHandler ZMMessageEvent;
    #endregion

    #region Fields
    private readonly List<MessageHandler<JSONMessage>> _jsonMessageHandlers =
    [
        new JHArea(),
        new JHBankInterface(),
        new JHBoostStatus(),
        new JHClassData(),
        new JHCombat(),
        new JHCurrency(),
        new JHDrop(),
        new JHEntityStats(),
        new JHEquipmentEnhancement(),
        new JHEventMessage(),
        new JHFactionData(),
        new JHInventory(),
        new JHInventorySlots(),
        new JHItemEquipment(),
        new JHPartyInterface(),
        new JHQuestData(),
        new JHShopInterface(),
        new JHUserData(),
        new JHVanityEquipmentData()
    ];
    private readonly List<MessageHandler<XTMessage>> _xtMessageHandlers =
    [
        new XHConnectionResponse(),
        new XHDungeonInterface(),
        new XHEntityStats(),
        new XHMapPlayer(),
        new XHRespawn()
    ];
    private readonly List<MessageHandler<ZMMessage>> _zmMessageHandlers =
    [
        new ZHBank(),
        new ZHConnection(),
        new ZHMapMovement(),
        new ZHSkillCooldown()
    ];
    #endregion
    
    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            QueueInvokes();

            Log("Network constructed successfully.", "Construct");

            if (!HSVCSettings.Instance.Get(SettingOption.DebugNetwork).Output!.Get<bool>())
            {
                return;
            }

            string pathToFolder = "packetFormats";
            string pathToJsonFolder = pathToFolder + "\\json";
            string pathToXtFolder = pathToFolder + "\\xt";
            string pathToZmFolder = pathToFolder + "\\zm";

            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }
            if (!Directory.Exists(pathToJsonFolder))
            {
                Directory.CreateDirectory(pathToJsonFolder);
            }
            if (!Directory.Exists(pathToXtFolder))
            {
                Directory.CreateDirectory(pathToXtFolder);
            }
            if (!Directory.Exists(pathToZmFolder))
            {
                Directory.CreateDirectory(pathToZmFolder);
            }

            JSONMessageEvent += new JHTester().Invoke;
            XTMessageEvent += new XHTester().Invoke;
            ZMMessageEvent += new ZHTester().Invoke;
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
            DequeueInvokes();

            Log("Network deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }

    public void QueueInvokes()
    {
        foreach (MessageHandler<JSONMessage> handler in _jsonMessageHandlers)
        {
            JSONMessageEvent += handler.Invoke;
        }
        
        foreach (MessageHandler<XTMessage> handler in _xtMessageHandlers)
        {
            XTMessageEvent += handler.Invoke;
        }
        
        foreach (MessageHandler<ZMMessage> handler in _zmMessageHandlers)
        {
            ZMMessageEvent += handler.Invoke;
        }
    }

    public void DequeueInvokes()
    {
        foreach (MessageHandler<JSONMessage> handler in _jsonMessageHandlers)
        {
            JSONMessageEvent -= handler.Invoke;
        }
        
        foreach (MessageHandler<XTMessage> handler in _xtMessageHandlers)
        {
            XTMessageEvent -= handler.Invoke;
        }
        
        foreach (MessageHandler<ZMMessage> handler in _zmMessageHandlers)
        {
            ZMMessageEvent -= handler.Invoke;
        }
    }
    #endregion

    #region Methods: Monitoring
    public async void MonitorPacket(string args)
    {
        await Task.Delay(10);

        Message message = args switch
        {
            _ when args[0].Equals('{') => new JSONMessage(Identifier, args),
            _ when args[0].Equals('<') => new XMLMessage(Identifier, args),
            _ when args.Substring(4, 2).Equals("zm") => new ZMMessage(Identifier, args),
            _ => new XTMessage(Identifier, args)
        };

        switch (message)
        {
            case JSONMessage jsonMessage:
                JSONMessageEvent?.Invoke(jsonMessage);
                MSVCInterceptor.Instance.WriteInJson(Identifier.Codename, jsonMessage.Command, jsonMessage.RawContent);
                break;
            case XMLMessage xmlMessage:
                XMLMessageEvent?.Invoke(xmlMessage);
                MSVCInterceptor.Instance.WriteInXml(Identifier.Codename, xmlMessage.Command, xmlMessage.RawContent);
                break;
            case XTMessage xtMessage:
                XTMessageEvent?.Invoke(xtMessage);
                MSVCInterceptor.Instance.WriteInXt(Identifier.Codename, xtMessage.Command, xtMessage.RawContent);
                break;
            case ZMMessage zmMessage:
                ZMMessageEvent?.Invoke(zmMessage);
                MSVCInterceptor.Instance.WriteInZm(Identifier.Codename, zmMessage.Command, zmMessage.RawContent);
                break;
        }
    }
    #endregion

}
