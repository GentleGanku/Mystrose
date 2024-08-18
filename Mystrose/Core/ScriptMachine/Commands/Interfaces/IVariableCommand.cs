namespace Mystrose.Core.ScriptMachine.Commands.Interfaces;

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
