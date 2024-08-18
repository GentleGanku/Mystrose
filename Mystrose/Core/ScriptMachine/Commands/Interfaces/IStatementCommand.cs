namespace Mystrose.Core.ScriptMachine.Commands.Interfaces;

public interface IStatementCommand
{

    #region Inputs & Outputs
    ScriptStatementType? StatementType
    {
        get;
    }

    bool IsReverseChecked
    {
        get;
    }
    #endregion

}
