namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Conditional.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Equal)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptConditionalType
{
    [EnumMember(Value = "==")]
    Equal,
    [EnumMember(Value = "!=")]
    NotEqual,
    [EnumMember(Value = "<=")]
    LessThanOrEqual,
    [EnumMember(Value = "<")]
    LessThan,
    [EnumMember(Value = ">=")]
    MoreThanOrEqual,
    [EnumMember(Value = ">")]
    MoreThan,
    [EnumMember(Value = "Include")]
    Include,
    [EnumMember(Value = "Exclude")]
    Exclude
}
