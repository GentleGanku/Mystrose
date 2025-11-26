namespace Mystrose.DataRecords.Game.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Target Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum TargetType
{
    [EnumMember(Value = "s")]
    Self,
    [EnumMember(Value = "h")]
    Enemy,
    [EnumMember(Value = "f")]
    Ally,

    Unknown
}
