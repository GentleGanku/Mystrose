using Mystrose.ScriptMachine.Enumerations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// An inheritor class that represents an optional object.
/// </summary>
public class ScriptOptions : ScriptParameter
{

    #region Constructors
    public ScriptOptions()
    {
        // Empty constructor for serialization and deserialization.
    }

    public ScriptOptions(object value, string? hint = null) : base(value, hint)
    {
        InputType = ScriptParameterInputType.Options;
        Options = value.ToString()!;
    }
    #endregion

    #region Private Fields
    private string _options;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's list of values.
    /// </summary>
    /// <returns>
    /// A list representing the parameter's specific values.
    /// </returns>
    public string Options
    {
        get => _options;
        set
        {
            _options = value;
            List<string> list = GetOptionsList();
            if (list.Count > 0)
            {
                SetValue(list[0]);
            }
        }
    }
    #endregion

    #region Methods
    public List<string> GetOptionsList()
    {
        return [.. Options.Split(" / ")];
    }

    public void Empty()
    {
        Options = string.Empty;

        base.Empty();
    }
    #endregion

}
