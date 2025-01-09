namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCDrop(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{
    
    #region Methods: Service
    public bool Accept(BaseItem item)
    {
        return Execute(() =>
        {
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%getDrop%{roomId}%{item.ID}%");
        });
    }
    
    public void RejectAll()
    {
        Execute(() =>
        {
            Service.Call("rejectExcept");
        });
    }
    
    public void RejectExcept(params string[] itemNames)
    {
        Execute(() =>
        {
            Service.Call("rejectExcept", itemNames.Select(n => n.ToLower()));
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCDrop");
    }
    #endregion

}