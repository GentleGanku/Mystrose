namespace Mystrose.Services.Base;

public abstract class Subservice<T>(T service) where T : Service
{

    #region Fields
    protected readonly T Service = service;
    #endregion

    #region Abstract Methods
    protected abstract void Log(string message);
    #endregion

    #region Virtual Methods
    protected virtual void Execute(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    protected virtual ReturnValue Execute<ReturnValue>(Func<ReturnValue> func)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }

        return default;
    }
    #endregion
    
}
