using Message = Mystrose.Network.Messages.Message;

namespace Mystrose.Services.Instantiable;

public class ISVCNetwork
{

    #region Constructor
    public ISVCNetwork(ClientUseIdentifier identifier)
    {
        _identifier = identifier;
        Initialize();
        Checkup();
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
    private ClientUseIdentifier _identifier;
    #endregion

    #region Methods: Setup
    public void Initialize()
    {
        JSONMessageEvent += JHArea.Invoke;
        JSONMessageEvent += JHBoostStatus.Invoke;
        JSONMessageEvent += JHClassData.Invoke;
        JSONMessageEvent += JHCombat.Invoke;
        JSONMessageEvent += JHDrop.Invoke;
        JSONMessageEvent += JHEntityStats.Invoke;
        JSONMessageEvent += JHEventMessage.Invoke;
        JSONMessageEvent += JHInventory.Invoke;
        JSONMessageEvent += JHPartyInterface.Invoke;
        JSONMessageEvent += JHQuestData.Invoke;
        JSONMessageEvent += JHUserData.Invoke;
        JSONMessageEvent += JHItemEquipment.Invoke;
        JSONMessageEvent += JHFactionData.Invoke;
        JSONMessageEvent += JHCurrency.Invoke;
        JSONMessageEvent += JHShopInterface.Invoke;
        JSONMessageEvent += JHBankInterface.Invoke;
        JSONMessageEvent += JHVanityEquipmentData.Invoke;
        JSONMessageEvent += JHInventorySlots.Invoke;
        JSONMessageEvent += JHEquipmentEnhancement.Invoke;

        XTMessageEvent += XHEntityStats.Invoke;
        XTMessageEvent += XHRespawn.Invoke;
        XTMessageEvent += XHMapPlayer.Invoke;
        XTMessageEvent += XHDungeonInterface.Invoke;
        XTMessageEvent += XHConnectionResponse.Invoke;

        ZMMessageEvent += ZHConnection.Invoke;
        ZMMessageEvent += ZHBank.Invoke;
        ZMMessageEvent += ZHMapMovement.Invoke;
        ZMMessageEvent += ZHSkillCooldown.Invoke;
    }

    public void Checkup()
    {
        if (!SVCSettings.Get("debugNetwork").Output!.Get<bool>())
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

        JSONMessageEvent += JHTester.Invoke;
        XTMessageEvent += XHTester.Invoke;
        ZMMessageEvent += ZHTester.Invoke;
    }

    public void Dispose()
    {
        JSONMessageEvent -= JHArea.Invoke;
        JSONMessageEvent -= JHBoostStatus.Invoke;
        JSONMessageEvent -= JHClassData.Invoke;
        JSONMessageEvent -= JHCombat.Invoke;
        JSONMessageEvent -= JHDrop.Invoke;
        JSONMessageEvent -= JHEntityStats.Invoke;
        JSONMessageEvent -= JHEventMessage.Invoke;
        JSONMessageEvent -= JHInventory.Invoke;
        JSONMessageEvent -= JHPartyInterface.Invoke;
        JSONMessageEvent -= JHQuestData.Invoke;
        JSONMessageEvent -= JHUserData.Invoke;
        JSONMessageEvent -= JHItemEquipment.Invoke;
        JSONMessageEvent -= JHFactionData.Invoke;
        JSONMessageEvent -= JHCurrency.Invoke;
        JSONMessageEvent -= JHShopInterface.Invoke;
        JSONMessageEvent -= JHBankInterface.Invoke;
        JSONMessageEvent -= JHVanityEquipmentData.Invoke;
        JSONMessageEvent -= JHInventorySlots.Invoke;
        JSONMessageEvent -= JHEquipmentEnhancement.Invoke;

        XTMessageEvent -= XHEntityStats.Invoke;
        XTMessageEvent -= XHRespawn.Invoke;
        XTMessageEvent -= XHMapPlayer.Invoke;
        XTMessageEvent -= XHDungeonInterface.Invoke;

        ZMMessageEvent -= ZHConnection.Invoke;
        ZMMessageEvent -= ZHBank.Invoke;
        ZMMessageEvent -= ZHMapMovement.Invoke;
        ZMMessageEvent -= ZHSkillCooldown.Invoke;
    }
    #endregion

    #region Methods: Monitoring
    public async void MonitorPacket(string args)
    {
        await Task.Delay(10);

        Message message = args switch
        {
            _ when args[0].Equals('{') => new JSONMessage(_identifier, args),
            _ when args[0].Equals('<') => new XMLMessage(_identifier, args),
            _ when args.Substring(4, 2).Equals("zm") => new ZMMessage(_identifier, args),
            _ => new XTMessage(_identifier, args)
        };

        switch (message)
        {
            case JSONMessage jsonMessage:
                JSONMessageEvent?.Invoke(jsonMessage);
                break;
            case XMLMessage xmlMessage:
                XMLMessageEvent?.Invoke(xmlMessage);
                break;
            case XTMessage xtMessage:
                XTMessageEvent?.Invoke(xtMessage);
                break;
            case ZMMessage zmMessage:
                ZMMessageEvent?.Invoke(zmMessage);
                break;
        }
    }
    #endregion

}
