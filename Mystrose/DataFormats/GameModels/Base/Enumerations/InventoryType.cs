namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Inventory Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum InventoryType
{
    [EnumMember(Value = "Base Inventory")]
    Base,
    [EnumMember(Value = "Temporary Inventory")]
    Temp,
    [EnumMember(Value = "House Inventory")]
    House,
    [EnumMember(Value = "Bank Inventory")]
    Bank,

    Unknown
}
