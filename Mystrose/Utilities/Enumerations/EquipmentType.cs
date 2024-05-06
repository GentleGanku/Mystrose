using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Equipment Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum EquipmentType
{
    [EnumMember(Value = "Weapon")]
    Weapon,
    [EnumMember(Value = "ar")]
    Class,
    [EnumMember(Value = "co")]
    Armor,
    [EnumMember(Value = "he")]
    Helm,
    [EnumMember(Value = "ba")]
    Cape,
    [EnumMember(Value = "pe")]
    Pet,
    [EnumMember(Value = "am")]
    Amulet,
    [EnumMember(Value = "mi")]
    Ground,
    [EnumMember(Value = "ho")]
    House,

    Unknown
}