namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Race Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum RaceType
{
    None,
    Chaos,
    Dragonkin,
    Drakath,
    Elemental,
    Human,
    Orc,
    Undead,

    Unknown
}