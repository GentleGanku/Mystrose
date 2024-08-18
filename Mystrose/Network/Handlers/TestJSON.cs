namespace Mystrose.Network.Handlers;

public class TestJSON : IJSONMessageHandler
{

    public string[] HandledCommands
    {
        get;
        set;
    } = new string[]
    {
        "friendshipStats",
        "friendshipInfo",
        "friendshipGift",
        "friendshipTalk",
        "friendshipChoice",
                        
        "addLoadout",
        "removeLoadout",
        "wearLoadout",
                        
        "acceptQuest",
        "afkGameResponse",
        "who",
        "al",
        "getinfo",
                        
        "changeColor",
        "changeArmorColor",
                        
        "friends",
                        
        "initInventory",
        "loadHouseInventory",
                        
        "house",
                        
        "callfct",

        "enhanceItemShop",
        "enhanceItemLocal",
                        
        "loadHairShop",
                        
        "buyItem",
        "sellItem",
        "removeItem",
                        
        "updateClass",
                        
        "getDrop",
        "addItems",
        "Wheel",
        "powerGem",
        "forceAddItem",
                        
        "warvalues",
                        
        "turnIn",
        "getQuest",
        "getQuests",
        "getQuests2",
        "ccqr",
        "updateQuest",
        "showQuestLink",
                        
        "dailylogin",
                        
        "initMonData",
                        
        "aura*",
        "aura+p",

        "uotls",
        "mtls",
        "cb",
        "ct",
                        
        "sar",
        "sars",
        "showAuraResult",
        "anim",
        "sAct",
        "seia",
        "stu",
                        
        "event",
                        
        "modinfo",
        "modinc",
                        
        "ia",
        "siau",
                        
        "umsg",
                        
        "gi",
        "gd",
        "ga",
        "gr",
        "guildDelete",
        "gMOTD",
        "updateGuild",
        "gc",
        "interior",
        "guildhall",
        "guildinv",
                        
        "pi",
        "pa",
        "pr",
        "pp",
        "ps",
        "pd",
        "pc",
                        
        "PVPQ",
        "PVPI",
        "PVPE",
        "PVPS",
        "PVPC",
        "pvpbreakdown",
        "di",
        "DuelEX",
                        
        "loadFactions",
        "addFaction",
                        
        "loadFriendsList",
        "requestFriend",
        "addFriend",
        "updateFriend",
        "deleteFriend",
                        
        "isModerator",
                        
        "loadWarVars",
                        
        "setAchievement",
                        
        "loadQuestStringData",
                        
        "getAdData",
        "getAdReward",

        "gettimes",
        "clockTick",
                        
        "castWait",
        "CatchResult",
                        
        "alchOnStart",
        "alchComplete",
                        
        "spellOnStart",
        "spellComplete",
        "spellWaitTimer",
                        
        "playerDeath",
                        
        "getScrolls",
        "turninscroll",
                        
        "getapop"
    };

    public void Handle(GameHost host, JSONMessage message)
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "Packets\\JSON\\" + message.Command + ".json";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonString = JsonSerializer.Serialize(message.Object, options);

        if (File.Exists(path))
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "Packets\\JSON\\temp" + message.Command + ".json";

            File.WriteAllText(tempPath, jsonString);

            if (File.ReadAllLines(path).Length < File.ReadAllLines(tempPath).Length)
            {
                File.Delete(tempPath);
                File.WriteAllText(path, jsonString);
            }
            else
            {
                File.Delete(tempPath);
            }
        }
        else
        {
            File.WriteAllText(path, jsonString);
        }
    }

}
