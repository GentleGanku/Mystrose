using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Entity Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum EntityType
{
    [EnumMember(Value = "p")]
    Player,
    [EnumMember(Value = "m")]
    Monster,

    Unknown
}
