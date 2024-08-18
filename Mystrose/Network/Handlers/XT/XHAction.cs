namespace Mystrose.Network.Handlers.XT;

public class XHAction : IXTMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "uotls",
            "mtls",
            "mvna",
            "mvnb"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(GameHost host, XTMessage message)
    {
        switch (message.Command)
        {
            case "uotls":
                HandleUotls(host, message.Arguments);
                break;
            case "mtls":
                HandleMtls(host, message.Arguments);
                break;
        }
    }
    #endregion

    #region Methods: UOTLS
    private void HandleUotls(GameHost host, string[] args)
    {
        Avatar? avatar = host.World.Area.Players.Find(
            (avt) =>
            {
                return avt.Name == args[4];
            });

        if (avatar == null)
        {
            return;
        }

        string[] objs = args[5].Split(',');

        foreach (string dataObj in objs)
        {
            string[] data = dataObj.Split(":");
            avatar.SetProperty(data[0], data[1]);
        }

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Player, avatar);
    }
    #endregion

    #region Methods: MTLS
    private void HandleMtls(GameHost host, string[] args)
    {
        Monster? monster = host.World.Area.Monsters.Find(
            (mon) =>
            {
                return mon.MonMapID == int.Parse(args[4]);
            });

        if (monster == null)
        {
            return;
        }

        string[] objs = args[5].Split(',');

        foreach (string dataObj in objs)
        {
            string[] data = dataObj.Split(":");
            monster.SetProperty(data[0], data[1]);
        }

        host.ScriptManager.InvokeTriggerSystems(ScriptTriggerType.Monster, monster);
    }
    #endregion

}
