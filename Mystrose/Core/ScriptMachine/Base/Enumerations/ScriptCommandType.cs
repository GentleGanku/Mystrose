namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script command type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptCommandType
{
    [EnumMember(Value = "Filler")]
    Filler,
    [EnumMember(Value = "Action")]
    Action,
    [EnumMember(Value = "Statement")]
    Statement,
    [EnumMember(Value = "Trigger")]
    Trigger,
    [EnumMember(Value = "Stack")]
    Stack,
    [EnumMember(Value = "Variable")]
    Variable
}
