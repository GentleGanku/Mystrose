using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every statement object type in the script machine.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptStatementType
{
    [EnumMember(Value = "Variable")]
    Variable,
    [EnumMember(Value = "Self")]
    Self,
    [EnumMember(Value = "Player")]
    Player,
    [EnumMember(Value = "Monster")]
    Monster,
    [EnumMember(Value = "Skill")]
    Skill,
    [EnumMember(Value = "Aura")]
    Aura,
    [EnumMember(Value = "Map")]
    Map,
    [EnumMember(Value = "Faction")]
    Faction,
    [EnumMember(Value = "Quest")]
    Quest,
    [EnumMember(Value = "Item")]
    Item,
    [EnumMember(Value = "Drop")]
    Drop
}
