namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every value type available in a Script Variable.
/// </summary>
[JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Assignation)]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptOperatorType
{
    [EnumMember(Value = "=")]
    Assignation,

    [EnumMember(Value = "+")]
    Addition,
    [EnumMember(Value = "-")]
    Subtraction,
    [EnumMember(Value = "*")]
    Multiplication,
    [EnumMember(Value = ":")]
    Division,
    [EnumMember(Value = "%")]
    Modulo,
}
