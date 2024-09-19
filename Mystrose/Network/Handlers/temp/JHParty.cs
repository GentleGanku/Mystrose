namespace Mystrose.Network.Handlers.JSON;

public class JHParty : IJSONMessageHandler
{

    #region Commands
    public string[] HandledCommands
    {
        get =>
        [
            "pi",
            "pa",
            "pr",
            "pp",
            "ps",
            "pd",
            "pc"
        ];
    }
    #endregion

    #region Methods: Handler
    public void Handle(JSONMessage message)
    {
        switch (message.Command)
        {
            case "pi":
                HandleInvite(message);
                break;
            case "pa":
                HandleAccept(message);
                break;
            case "pr":
                HandleRemove(message);
                break;
            case "pp":
                HandlePromote(message);
                break;
            case "ps":
                HandleSummon(message);
                break;
            case "pd":
                HandleDecline(message);
                break;
            case "pc":
                HandleDisband(message);
                break;
        }
    }
    #endregion

    #region Methods: Party
    private void HandleInvite(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        string? owner = obj["owner"]?.Deserialize<string>().ToLower();
        int? id = obj["pid"]?.Deserialize<int>();

        // WIP
    }

    private void HandleAccept(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        Party? party = obj.Deserialize<Party>();

        if (world.Environment.Party is null)
        {
            party.Owner = party.Owner.ToLower();
            world.Environment.Party = party;
        }
        else
        {
            world.Environment.Party.Members = party.Members;
        }
    }

    private void HandleRemove(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        string? owner = obj["owner"]?.Deserialize<string>();
        string? removedMember = obj["unm"]?.Deserialize<string>();

        world.Environment.Party.Owner = owner;
        world.Environment.Party.Members.Remove(removedMember);
    }

    private void HandlePromote(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        string? owner = obj["owner"]?.Deserialize<string>();

        world.Environment.Party.Owner = owner;
    }

    private void HandleSummon(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        string? inviter = obj["unm"]?.Deserialize<string>().ToLower();

        // WIP
    }

    private void HandleDecline(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        string? target = obj["unm"]?.Deserialize<string>().ToLower();

        // WIP
    }

    private void HandleDisband(JSONMessage message)
    {
        World world = message.World;
        JsonObject obj = message.DataObject;

        int? id = obj["pid"]?.Deserialize<int>();

        if (world.Environment.Party.ID != id)
        {
            return;
        }

        world.Environment.Party = null;
    }
    #endregion

}