namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCMap(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{
    
    #region Methods: Service
    public bool WalkTo(double xCoordinate, double yCoordinate)
    {
        return Execute(() =>
        {
            int walkSpeed = Service.GetGameObject<int>("world.WALKSPEED");
            int roomId = GetRoomID();

            Service.CallGameFunction("world.myAvatar.pMC.walkTo", xCoordinate, yCoordinate, walkSpeed);
            return Service.SendToServer($"%xt%zm%mv%{roomId}%{xCoordinate}%{yCoordinate}%{walkSpeed}%");
        });
    }
    
    public void MoveTo(string cell = "Enter", string pad = "Spawn", bool avoidNotify = false)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.moveToCell", cell, pad, avoidNotify);
        });
    }
    
    public void Goto(string playerName)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.goto", playerName);
        });
    }

    public void Join(string mapName, string instanceNumber = "-1", string cell = "Enter", string pad = "Spawn")
    {
        Execute(() =>
        {
            string mapInstance = instanceNumber switch
            {
                "-1" => mapName,
                "1e9" => $"{mapName}-{new Random().Next(1001, 100000)}",
                _ => $"{mapName}-{instanceNumber}"
            };
            
            Service.CallGameFunction("world.gotoTown", mapInstance, cell, pad);
        });
    }

    public void SetSpawnPoint(string cell = "Enter", string pad = "Spawn")
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.setSpawnPoint", cell, pad);
        });
    }
    
    public void Load(string mapFilePath)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.loadMap", mapFilePath);
        });
    }
    
    public void Reload()
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.reloadCurrentMap");
        });
    }
    
    public void GetMapItem(int mapItemId)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.getMapItem", mapItemId);
        });
    }
    
    public int GetRoomID()
    {
        return Execute(() => GetRoomID());
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCMap");
    }
    #endregion

}