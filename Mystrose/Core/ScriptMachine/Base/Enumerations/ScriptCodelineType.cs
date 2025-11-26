namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script command type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptCodelineType
{
    [EnumMember(Value = "Action")]
    Action,
    [EnumMember(Value = "Filler")]
    Filler,
    [EnumMember(Value = "Stack")]
    Stack,
    [EnumMember(Value = "Statement")]
    Statement,
    [EnumMember(Value = "Trigger")]
    Trigger,
    [EnumMember(Value = "Variable")]
    Variable,
    [EnumMember(Value = "Option")]
    Option
}
