using System.Collections.Generic;
using System.Linq;

namespace Mystrose.ScriptMachine.Inputs;

/// <summary>
/// A base class that represents a conditional statement object.
/// </summary>
public class ScriptOptions : ScriptParameter
{

    #region Constructors
    public ScriptOptions(object value, string? hint = null) : base(value, hint)
    {
        SetList(GetList());
    }
    #endregion

    #region Private Fields
    private List<string>? _list;
    #endregion

    #region Properties
    /// <summary>
    /// The parameter's list of values.
    /// </summary>
    /// <returns>
    /// A list representing the parameter's specific values.
    /// </returns>
    public List<string>? List
    {
        get => _list;
        private set
        {
            _list = value;
            if (value?.Count > 0)
            {
                SetValue(value[0]);
            }
        }
    }
    #endregion

    #region Methods
    public List<string> GetList()
    {
        return Object switch
        {
            string stringValue => stringValue.Split(" / ").ToList(),
            string[] arrayValue => arrayValue.ToList(),
            List<string> listValue => listValue,
            _ => []
        };
    }

    public void SetList(List<string> list)
    {
        List = new(list);
    }

    public void Empty()
    {
        base.Empty();

        List = null;
    }
    #endregion

}
