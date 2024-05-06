using Mystrose.ScriptMachine.Enumerations;
using Mystrose.ScriptMachine.Objects;
using System.Collections.Generic;

namespace Mystrose.ScriptMachine.Interfaces;

public interface IListCommand
{

    #region Inputs & Outputs
    ScriptListType? ListType
    {
        get;
    }

    List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }
    #endregion

}
