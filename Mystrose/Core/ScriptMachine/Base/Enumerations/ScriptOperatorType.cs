namespace Mystrose.Core.ScriptMachine.Base.Enumerations;

/// <summary>
/// An enumeration that represents every operator type in the Script Engine.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptOperatorType
{
    [EnumMember(Value = "=  Assign with")]
    Assign = 001,

    [EnumMember(Value = "=  Concatenate of")]
    Concat = 101,
    [EnumMember(Value = "=  Remove of")]
    Remove = 102,

    [EnumMember(Value = "+  Add with")]
    Add = 201,
    [EnumMember(Value = "-  Subtract with")]
    Subtract = 202,
    [EnumMember(Value = "*  Multiply by")]
    Multiply = 203,
    [EnumMember(Value = ":  Divide by")]
    Divide = 204,
    [EnumMember(Value = "%  Remainder by")]
    Modulo = 205,
}
