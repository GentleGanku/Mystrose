namespace Mystrose.Network.Handlers.JSON;

public static class JHPartyInterface
{

    #region Fields
    private static readonly Dictionary<string, Action<JSONMessage>> _handlers = new()
    {
        ["pi"] = HandleInvite,
        ["pa"] = HandleAccept,
        ["pr"] = HandleRemove,
        ["pp"] = HandlePromote,
        ["ps"] = HandleSummon,
        ["pd"] = HandleDecline,
        ["pc"] = HandleDisband
    };
    #endregion

    #region Methods: Invoker
    public static void Invoke(JSONMessage message)
    {
        if (!_handlers.TryGetValue(message.Command, out var handler))
        {
            return;
        }

        try
        {
            handler.Invoke(message);
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnException($"({nameof(message)} - {message.Command}) {ex.ToString()}");
        }
    }
    #endregion

    #region Handlers
    private static void HandleInvite(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;
        party.Status = PartyProcessType.Inviting;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, party);
    }

    private static void HandleAccept(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;
        party.Status = PartyProcessType.Joining;

        if (message.World.Party is null)
        {
            party.Owner = party.Owner.ToLower();
            message.World.Party = party;
        }
        else
        {
            message.World.Party.Members = party.Members;
            message.World.Party.Status = party.Status;
        }

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Party);
    }

    private static void HandleRemove(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;

        string removedMember = message.DataObject["unm"].Deserialize<string>()!;

        message.World.Party.Owner = party.Owner;
        message.World.Party.Status = PartyProcessType.Removing;
        message.World.Party.Members.Remove(removedMember);

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Party);
    }

    private static void HandlePromote(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;

        message.World.Party.Owner = party.Owner;
        message.World.Party.Status = PartyProcessType.Promoting;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Party);
    }

    private static void HandleSummon(JSONMessage message)
    {
        string summoner = message.DataObject["unm"].Deserialize<string>()!.ToLower();

        message.World.Party.Status = PartyProcessType.Summoning;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, message.World.Party);
    }

    private static void HandleDecline(JSONMessage message)
    {
        string target = message.DataObject["unm"].Deserialize<string>()!.ToLower();

        Party party = new()
        {
            Owner = target,
            Status = PartyProcessType.Declining
        };

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, party);
    }

    private static void HandleDisband(JSONMessage message)
    {
        int id = message.DataObject["pid"].Deserialize<int>();

        if (message.World.Party.ID != id)
        {
            return;
        }

        Party party = message.World.Party;
        party.Status = PartyProcessType.Disbanding;

        message.World.Party = null;

        SVCScriptManager.InvokeTrigger(message.Identifier.Codename, party);
    }
    #endregion

}
