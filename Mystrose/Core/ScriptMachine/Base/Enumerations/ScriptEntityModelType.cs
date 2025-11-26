namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every target model type for Script Codeline.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptEntityModelType
{
    [EnumMember(Value = "Active Skill")]
    ActiveSkill = 001,
    [EnumMember(Value = "Area")]
    Area = 002,
    [EnumMember(Value = "Aura")]
    Aura = 003,
    [EnumMember(Value = "Avatar")]
    Avatar = 004,
    [EnumMember(Value = "Cell")]
    Cell = 005,
    [EnumMember(Value = "Faction")]
    Faction = 006,
    [EnumMember(Value = "Inventory Item")]
    InventoryItem = 007,
    [EnumMember(Value = "Item Drop")]
    ItemDrop = 008,
    [EnumMember(Value = "Monster")]
    Monster = 009,
    [EnumMember(Value = "Quest")]
    Quest = 010,
    [EnumMember(Value = "Self")]
    Self = 011,
    [EnumMember(Value = "Shop Item")]
    ShopItem = 012,
    [EnumMember(Value = "Script Variable")]
    ScriptVariable = 013,

    [EnumMember(Value = "Combat Message")]
    CombatMessage = 101,
    [EnumMember(Value = "Event Message")]
    EventMessage = 102
}
