using Mystrose.ScriptMachine.Objects;

namespace Mystrose.ScriptMachine.Interfaces;

public interface IVariableCommand
{

    #region Inputs & Outputs
    ScriptVariable Variable
    {
        get;
        set;
    }
    #endregion

}
