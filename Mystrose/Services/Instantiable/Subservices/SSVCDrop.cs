namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCDrop(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{
    
    #region Methods: Service
    public bool AcceptDrop(BaseItem item)
    {
        return Execute(() =>
        {
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%getDrop%{roomId}%{item.ID}%");
        });
    }
    
    public bool AcceptDrop(int itemId)
    {
        return Execute(() =>
        {
            int roomId = Service.Map.GetRoomID();
            return Service.SendToServer($"%xt%zm%getDrop%{roomId}%{itemId}%");
        });
    }
    
    public void RejectAllDrops()
    {
        Execute(() =>
        {
            Service.Call("rejectExcept");
        });
    }
    
    public void RejectDropsExcept(params string[] itemNames)
    {
        Execute(() =>
        {
            Service.Call("rejectExcept", itemNames.Select(n => n.ToLower()));
        });
    }
    
    public void RejectDrop(string itemName)
    {
        Execute(() =>
        {
            string[] exceptions = [.. MSVCWorld.Instance.ActiveCollection[Service.Identifier.Codename]!.Drops
                .Where(d => !d.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.Name.ToLower())];
            
            Service.Call("rejectExcept", exceptions);
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