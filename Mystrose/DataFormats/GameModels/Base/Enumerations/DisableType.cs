namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Disable Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum DisableType
{
    [EnumMember(Value = null)]
    None,
    [EnumMember(Value = "stun")]
    Stun,
    [EnumMember(Value = "stone")]
    Petrify,
    [EnumMember(Value = "disabled")]
    Disable,

    Unknown
}
