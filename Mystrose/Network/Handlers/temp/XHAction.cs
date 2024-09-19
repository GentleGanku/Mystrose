namespace Mystrose.Network.Handlers.temp;

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
    public void Handle(XTMessage message)
    {
        switch (message.Command)
        {
            case "uotls":
                HandleUotls(message);
                break;
            case "mtls":
                HandleMtls(message);
                break;
        }
    }
    #endregion

    #region Methods: UOTLS
    private void HandleUotls(XTMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        Avatar? avatar = world.Environment.Area.Players.Find(
            (avt) =>
            {
                return avt.Name == args[4];
            });

        if (avatar is null)
        {
            return;
        }

        string[] objs = args[5].Split(',');

        foreach (string dataObj in objs)
        {
            string[] data = dataObj.Split(":");
            avatar.SetProperty(data[0], data[1]);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, avatar);
    }
    #endregion

    #region Methods: MTLS
    private void HandleMtls(XTMessage message)
    {
        World world = message.World;
        string[] args = message.Arguments;

        Monster? monster = world.Environment.Area.Monsters.Find(
            (mon) =>
            {
                return mon.MonMapID == int.Parse(args[4]);
            });

        if (monster is null)
        {
            return;
        }

        string[] objs = args[5].Split(',');

        foreach (string dataObj in objs)
        {
            string[] data = dataObj.Split(":");
            monster.SetProperty(data[0], data[1]);
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, monster);
    }
    #endregion

}
