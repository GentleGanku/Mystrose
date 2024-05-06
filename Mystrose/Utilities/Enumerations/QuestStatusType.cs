using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Action Slot Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum QuestStatusType
{
    [EnumMember(Value = null)]
    Inactive,
    [EnumMember(Value = "p")]
    Active,

    Unknown
}
