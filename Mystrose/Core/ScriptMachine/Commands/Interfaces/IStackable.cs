namespace Mystrose.Core.ScriptMachine.Codelines.Interfaces;

public interface IStackable
{

    #region Properties: I/O
    ScriptCodeline[] InternalCommands
    {
        get;
        init;
    }
    #endregion

    #region Methods: Validation
    string ValidateCodelineToBeAdded(ScriptCodeline cdl);
    #endregion

}
