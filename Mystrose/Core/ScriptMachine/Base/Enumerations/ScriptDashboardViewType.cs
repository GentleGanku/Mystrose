namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every script dashboard view type.
/// </summary>
public enum ScriptDashboardViewType
{
    [EnumMember(Value = "List of commands")]
    Commands = 001,
    [EnumMember(Value = "List of triggers")]
    Triggers = 002,
    [EnumMember(Value = "List of variables")]
    Variables = 003,

    [EnumMember(Value = "Enlisted commands inside")]
    Listed = 101,
}
