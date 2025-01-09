namespace Mystrose.Services.Base;

public abstract class Service(string name)
{
    
    #region Properties
    protected string Name
    {
        get;
        init;
    } = name;
    #endregion

    #region Abstract Methods
    public abstract void Construct();
    public abstract void Deconstruct();
    #endregion

    #region Methods
    protected void Log(string message = "An unknown error has occured.", string caller = "Service")
    {
        HSVCLogger.Instance.LogOnConsole(message, Name, caller);
    }
    #endregion

}
