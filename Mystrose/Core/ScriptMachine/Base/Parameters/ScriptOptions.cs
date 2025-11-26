namespace Mystrose.Core.ScriptMachine.Base.Parameters;

/// <summary>
/// An inheritor class that represents an optional object.
/// </summary>
public class ScriptOptions : ScriptParameter
{

    #region Constructors
    public ScriptOptions(string value, string hint = "") : base(value, hint)
    {
        Options = value;
    }
    #endregion

    #region Private Fields
    private string _options = string.Empty;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's input type.
    /// </summary>
    /// <returns>
    /// An enumeration representing the parameter's input type.
    /// </returns>
    public override ScriptParameterInputType InputType
    {
        get => ScriptParameterInputType.Options;
    }

    /// <summary>
    /// The parameter's set of options in string form.
    /// </summary>
    /// <returns>
    /// A string representing the set of options.
    /// </returns>
    public string Options
    {
        get => _options;
        protected set
        {
            _options = value;

            string[] values = value.Split("/");
            if (values.Length > 0 && String.Contains('/'))
            {
                Set(values[0]);
            }
        }
    }
    #endregion

}
