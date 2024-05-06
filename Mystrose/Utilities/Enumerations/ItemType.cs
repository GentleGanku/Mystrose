using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Mystrose.Utilities.Enumerations;

/// <summary>
/// An enumeration that represents every Item Type in the game.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Unknown)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ItemType
{
    [EnumMember(Value = "Sword")]
    Sword,
    [EnumMember(Value = "Axe")]
    Axe,
    [EnumMember(Value = "Dagger")]
    Dagger,
    [EnumMember(Value = "Gauntlet")]
    Gauntlet,
    [EnumMember(Value = "Gun")]
    Gun,
    [EnumMember(Value = "HandGun")]
    HandGun,
    [EnumMember(Value = "Bow")]
    Bow,
    [EnumMember(Value = "Crossbow")]
    Mace,
    [EnumMember(Value = "Polearm")]
    Polearm,
    [EnumMember(Value = "Rifle")]
    Rifle,
    [EnumMember(Value = "Staff")]
    Staff,
    [EnumMember(Value = "Wand")]
    Wand,
    [EnumMember(Value = "Whip")]
    Whip,
    [EnumMember(Value = "Class")]
    Class,
    [EnumMember(Value = "Armor")]
    Armor,
    [EnumMember(Value = "Helm")]
    Helm,
    [EnumMember(Value = "Cape")]
    Cape,
    [EnumMember(Value = "Pet")]
    Pet,
    [EnumMember(Value = "Amulet")]
    Amulet,
    [EnumMember(Value = "Necklace")]
    Necklace,
    [EnumMember(Value = "Note")]
    Note,
    [EnumMember(Value = "Resource")]
    Resource,
    [EnumMember(Value = "Item")]
    Item,
    [EnumMember(Value = "Misc")]
    Misc,
    [EnumMember(Value = "Quest Item")]
    QuestItem,
    [EnumMember(Value = "Server Use")]
    ServerUse,
    [EnumMember(Value = "House")]
    House,
    [EnumMember(Value = "Wall Item")]
    WallItem,
    [EnumMember(Value = "Floor Item")]
    FloorItem,
    [EnumMember(Value = "Enhancement")]
    Enhancement,

    Unknown
}