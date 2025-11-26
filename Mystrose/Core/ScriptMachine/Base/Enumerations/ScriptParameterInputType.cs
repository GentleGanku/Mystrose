namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every input type available for Script Parameter.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptParameterInputType
{
    [EnumMember(Value = "INPUT_TYPE.001")]
    Parameter = 001,
    [EnumMember(Value = "INPUT_TYPE.002")]
    Conditional = 002,
    [EnumMember(Value = "INPUT_TYPE.003")]
    Options = 003,
    [EnumMember(Value = "INPUT_TYPE.004")]
    KeyValuePair = 004,
}
