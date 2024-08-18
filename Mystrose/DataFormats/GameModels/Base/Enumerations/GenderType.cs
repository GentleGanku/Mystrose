namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Gender Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum GenderType
{
    [EnumMember(Value = "M")]
    Male,
    [EnumMember(Value = "F")]
    Female,

    Unknown
}
