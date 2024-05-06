using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Boost Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum BoostType
{
    [EnumMember(Value = "gboost")]
    Gold,
    [EnumMember(Value = "cpboost")]
    Class,
    [EnumMember(Value = "repboost")]
    Reputation,
    [EnumMember(Value = "xpboost")]
    Experience,

    Unknown
}