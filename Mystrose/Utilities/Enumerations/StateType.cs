using System.Text.Json.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every State Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Idle)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum StateType
{
    Dead,
    Idle,
    InCombat
}
