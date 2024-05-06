using Mystrose.ScriptMachine.Enumerations;

namespace Mystrose.ScriptMachine.Interfaces;

public interface IStatementCommand
{

    #region Inputs & Outputs
    ScriptStatementType? StatementType
    {
        get;
    }
    #endregion

}
