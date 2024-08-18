namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Boost Type in the game.
/// </summary>
[DataContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoleType
{
    [EnumMember(Value = "Shield")]
    Shield,
    [EnumMember(Value = "Support")]
    Support,
    [EnumMember(Value = "Warrior")]
    Warrior,
    [EnumMember(Value = "Mage")]
    Mage,
    [EnumMember(Value = "Rogue")]
    Rogue,
    [EnumMember(Value = "Healer")]
    Healer,
    [EnumMember(Value = "Default")]
    Unknown
}