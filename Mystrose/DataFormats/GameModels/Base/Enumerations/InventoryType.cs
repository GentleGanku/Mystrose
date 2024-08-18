namespace Mystrose.DataFormats.GameModels.Base.Enumerations;

/// <summary>
/// An enumeration that represents every Inventory Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum InventoryType
{
    [EnumMember(Value = "Inventory")]
    Inventory,
    [EnumMember(Value = "Temporary Inventory")]
    TemporaryInventory,
    [EnumMember(Value = "House Inventory")]
    HouseInventory,
    [EnumMember(Value = "Bank Inventory")]
    BankInventory,

    Unknown
}
