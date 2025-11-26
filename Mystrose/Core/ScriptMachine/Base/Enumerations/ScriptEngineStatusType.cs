namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script engine status type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptEngineStatusType
{
    [EnumMember(Value = "Idle")]
    Idle = 001,

    [EnumMember(Value = "Running")]
    Running = 101,

    [EnumMember(Value = "Paused")]
    Paused = 201,
    [EnumMember(Value = "Stopped")]
    Stopped = 202,

    [EnumMember(Value = "Crash")]
    Crash = 303
}
