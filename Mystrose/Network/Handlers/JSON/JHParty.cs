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
    public void Handle(GameHost host, JSONMessage message)
    {
        switch (message.Command)
        {
            case "pi":
                HandleInvite(host, message.DataObject);
                break;
            case "pa":
                HandleAccept(host, message.DataObject);
                break;
            case "pr":
                HandleRemove(host, message.DataObject);
                break;
            case "pp":
                HandlePromote(host, message.DataObject);
                break;
            case "ps":
                HandleSummon(host, message.DataObject);
                break;
            case "pd":
                HandleDecline(host, message.DataObject);
                break;
            case "pc":
                HandleDisband(host, message.DataObject);
                break;
        }
    }
    #endregion

    #region Methods: Party
    private void HandleInvite(GameHost host, JsonObject obj)
    {
        string? owner = obj["owner"]?.Deserialize<string>().ToLower();
        int? id = obj["pid"]?.Deserialize<int>();

        // WIP
    }

    private void HandleAccept(GameHost host, JsonObject obj)
    {
        Party? party = obj.Deserialize<Party>();

        if (host.World.Party is null)
        {
            party.Owner = party.Owner.ToLower();
            host.World.Party = party;
        }
        else
        {
            host.World.Party.Members = party.Members;
        }
    }

    private void HandleRemove(GameHost host, JsonObject obj)
    {
        string? owner = obj["owner"]?.Deserialize<string>();
        string? removedMember = obj["unm"]?.Deserialize<string>();

        host.World.Party.Owner = owner;
        host.World.Party.Members.Remove(removedMember);
    }

    private void HandlePromote(GameHost host, JsonObject obj)
    {
        string? owner = obj["owner"]?.Deserialize<string>();

        host.World.Party.Owner = owner;
    }

    private void HandleSummon(GameHost host, JsonObject obj)
    {
        string? inviter = obj["unm"]?.Deserialize<string>().ToLower();

        // WIP
    }

    private void HandleDecline(GameHost host, JsonObject obj)
    {
        string? target = obj["unm"]?.Deserialize<string>().ToLower();

        // WIP
    }

    private void HandleDisband(GameHost host, JsonObject obj)
    {
        int? id = obj["pid"]?.Deserialize<int>();

        if (host.World.Party.ID != id)
        {
            return;
        }

        host.World.Party = null;
    }
    #endregion

}