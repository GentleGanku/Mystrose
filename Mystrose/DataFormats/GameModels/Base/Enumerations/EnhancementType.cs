namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Enhancement Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum EnhancementType
{
    Adventurer = 1,
    Fighter = 2,
    Thief = 3,
    Armsman = 4,
    Hybrid = 5,
    Wizard = 6,
    Healer = 7,
    Spellbreaker = 8,
    Lucky = 9,

    Unknown = 999
}
