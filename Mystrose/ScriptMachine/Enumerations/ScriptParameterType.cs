using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Enumerations;

/// <summary>
/// An enumeration that represents every Script Parameter Type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScriptParameterType
{
    [EnumMember(Value = "Parameter")]
    Parameter,
    [EnumMember(Value = "SecondaryParameter")]
    SecondaryParameter,
}
