using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Action Slot Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ActionType
{
    [EnumMember(Value = "p1")]
    FirstPassive,
    [EnumMember(Value = "p2")]
    SecondPassive,
    [EnumMember(Value = "p3")]
    ThirdPassive,
    [EnumMember(Value = "p4")]
    FourthPassive,

    [EnumMember(Value = "aa")]
    AutoAttack,
    [EnumMember(Value = "a1")]
    FirstActive,
    [EnumMember(Value = "a2")]
    SecondActive,
    [EnumMember(Value = "a3")]
    ThirdActive,
    [EnumMember(Value = "a4")]
    FourthActive,

    [EnumMember(Value = "i1")]
    Consumable,

    Unknown
}
