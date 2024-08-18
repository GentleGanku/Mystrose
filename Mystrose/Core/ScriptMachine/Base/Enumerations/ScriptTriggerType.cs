namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script command type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptTriggerType
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
    Drop,
    [EnumMember(Value = "Combat Message")]
    CombatMessage,
    [EnumMember(Value = "Event Message")]
    EventMessage,
}
