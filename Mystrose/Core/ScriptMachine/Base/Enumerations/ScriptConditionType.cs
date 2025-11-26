namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Conditional.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptConditionType
{
    [EnumMember(Value = "=  Equal to")]
    Equal = 001,
    [EnumMember(Value = "≠  Not equal to")]
    NotEqual = 002,

    [EnumMember(Value = "∋  To contain")]
    Contains = 101,
    [EnumMember(Value = "∌  To not contain")]
    NotContains = 102,
    [EnumMember(Value = "↦  Starts with")]
    StartsWith = 103,
    [EnumMember(Value = "↤  Ends with")]
    EndsWith = 104,

    [EnumMember(Value = "<  Less than")]
    LessThan = 201,
    [EnumMember(Value = "<= Less than or equal to")]
    LessThanOrEqual = 202,
    [EnumMember(Value = ">  More than")]
    MoreThan = 203,
    [EnumMember(Value = ">= More than or equal to")]
    MoreThanOrEqual = 204
}
