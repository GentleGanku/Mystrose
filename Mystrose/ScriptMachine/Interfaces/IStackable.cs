using Mystrose.ScriptMachine.Objects;
using System.Collections.Generic;

namespace Mystrose.ScriptMachine.Interfaces;

public interface IStackable
{

    #region Inputs & Outputs
    string LabelName
    {
        get;
    }

    int StackLimit
    {
        get;
    }

    List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }
    #endregion

    #region Methods
    bool IsInputValid(ScriptCommand cmd);
    #endregion

}
