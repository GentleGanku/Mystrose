using Mystrose.DataRecords.Game;

namespace Mystrose.Network.Handlers.JSON;

public class JHArea() : MessageHandler<JSONMessage>(new()
{
    ["moveToArea"] = HandleArea
})
{

    #region Methods: Handlers
    private static void HandleArea(JSONMessage message)
    {
        Area area = message.DataObject.Deserialize<Area>()!;
        MapFormat mapFormat = message.DataObject.Deserialize<MapFormat>()!;

        area.Format = mapFormat;
        message.HostWorld.Area = area;

        MSVCScript.Instance.InvokeTrigger(message.Identifier.Codename, area);
        HSVCRepository.Instance.AddModel([mapFormat]);
    }
    #endregion

}
