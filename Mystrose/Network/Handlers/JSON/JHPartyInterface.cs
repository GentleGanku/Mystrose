using Mystrose.DataRecords.Game;

namespace Mystrose.Network.Handlers.JSON;

public class JHPartyInterface() : MessageHandler<JSONMessage>(new()
{
    ["pi"] = HandleInvite,
    ["pa"] = HandleAccept,
    ["pr"] = HandleRemove,
    ["pp"] = HandlePromote,
    ["ps"] = HandleSummon,
    ["pd"] = HandleDecline,
    ["pc"] = HandleDisband
})
{

    #region Methods: Handlers
    private static void HandleInvite(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;
        party.Status = PartyProcessType.Inviting;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, party);
    }

    private static void HandleAccept(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;
        party.Status = PartyProcessType.Joining;

        if (message.HostWorld.Party is null)
        {
            party.Owner = party.Owner.ToLower();
            message.HostWorld.Party = party;
        }
        else
        {
            message.HostWorld.Party.Members = party.Members;
            message.HostWorld.Party.Status = party.Status;
        }

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Party);
    }

    private static void HandleRemove(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;

        string removedMember = message.DataObject["unm"].Deserialize<string>()!;

        message.HostWorld.Party.Owner = party.Owner;
        message.HostWorld.Party.Status = PartyProcessType.Removing;
        message.HostWorld.Party.Members.Remove(removedMember);

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Party);
    }

    private static void HandlePromote(JSONMessage message)
    {
        Party party = message.DataObject.Deserialize<Party>()!;

        message.HostWorld.Party.Owner = party.Owner;
        message.HostWorld.Party.Status = PartyProcessType.Promoting;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Party);
    }

    private static void HandleSummon(JSONMessage message)
    {
        string summoner = message.DataObject["unm"].Deserialize<string>()!.ToLower();

        message.HostWorld.Party.Status = PartyProcessType.Summoning;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, message.HostWorld.Party);
    }

    private static void HandleDecline(JSONMessage message)
    {
        string target = message.DataObject["unm"].Deserialize<string>()!.ToLower();

        Party party = new()
        {
            Owner = target,
            Status = PartyProcessType.Declining
        };

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, party);
    }

    private static void HandleDisband(JSONMessage message)
    {
        int id = message.DataObject["pid"].Deserialize<int>();

        if (message.HostWorld.Party.ID != id)
        {
            return;
        }

        Party party = message.HostWorld.Party;
        party.Status = PartyProcessType.Disbanding;

        message.HostWorld.Party = null;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, party);
    }
    #endregion

}
