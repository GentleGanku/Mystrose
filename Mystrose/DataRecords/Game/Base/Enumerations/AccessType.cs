namespace Mystrose.DataRecords.Game.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Access Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Player)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum AccessType
{
    Player = 0,
    Staff = 30,
    Tester = 40,
    Developer = 50,
    Moderator = 60,
    GameLead = 100,
}
