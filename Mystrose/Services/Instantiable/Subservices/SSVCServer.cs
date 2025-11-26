using Mystrose.DataRecords.Game;

namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCServer(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void SetLoginInfo(string username, string password)
    {
        Execute(() =>
        {
            Service.SetGameObject("TempLoginName", username);
            Service.SetGameObject("TempLoginPass", password);
        });
    }
    
    public void Login()
    {
        Execute(() =>
        {
            string username = Service.GetGameObject<string>("TempLoginName");
            string password = Service.GetGameObject<string>("TempLoginPass");
            Service.CallGameFunction("login", username, password);
        });
    }
    
    public void Logout()
    {
        Execute(() =>
        {
            Service.CallGameFunction("logout");
        });
    }

    public void ConnectTo(Server server)
    {
        Execute(() =>
        {
            string serverString = JSONParser.Serialize(server);
            int chatRestrictionInt = int.Parse(JSONParser.Serialize(server.IsChatRestricted));
            
            Service.SetGameObject("objServerInfo", serverString);
            Service.SetGameObject("chatF.iChat", chatRestrictionInt);
            Service.CallGameFunction("connectTo", server.IP, server.Port);
        });
    }

    public void ShowLoginFrame()
    {
        Execute(() =>
        {
            bool isConnected = Service.GetGameObject<bool>("sfc.isConnected");

            if (isConnected)
            {
                Service.CallGameFunction("sfc.disconnect");
            }
            
            Service.CallGameFunction("gotoAndPlay", "Login");
            Service.CallGameFunction("removeAllChildren");
        });
    }
    
    public void ShowServersFrame()
    {
        Execute(() =>
        {
            Service.CallGameFunction("showServerList");
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCServer");
    }
    #endregion

}
