namespace Mystrose.Services;

public class SVCWorldVisualizer
{

    #region Delegates & Handlers
    public delegate void WorldHandler(string codename, World? world);
    public static event WorldHandler ActivatedWorldEvent;
    public static event WorldHandler DeactivatedWorldEvent;
    #endregion

    #region Fields
    private static readonly Dictionary<string, World?> _worlds = new()
    {
        ["Avernus"] = null,
        ["Beatrix"] = null,
        ["Cassiopeia"] = null,
        ["Durandal"] = null,
        ["Eligos"] = null,
        ["Fenrir"] = null,
        ["Gwyndell"] = null,
        ["Harbinger"] = null,
    };
    #endregion

    #region Methods: Read/Write
    public static Response<World?> Activate(string codename, string serverName)
    {
        if (_worlds.TryGetValue(codename, out World? world) && world is not null)
        {
            return new(false,
                $"World with the codename {codename} is already activated.",
                world);
        }

        ClientUseIdentifier identifier = new(codename);
        List<Server> servers = SVCRepository.Models[nameof(Server)].Get<Server>();
        Server server = servers.Find(s => s.Name.Equals(serverName))!;

        _worlds[codename] = new(identifier, server);

        ActivatedWorldEvent?.Invoke(codename, _worlds[codename]);

        return new(true,
            $"World with the codename {codename} has been activated.",
            world);
    }

    public static Response<World?> Deactivate(string codename)
    {
        if (_worlds.TryGetValue(codename, out World? world) && world is null)
        {
            return new(false,
                $"World with the codename {codename} is already deactivated.",
                world);
        }

        _worlds[codename]!.Destruct();
        _worlds[codename] = null;

        DeactivatedWorldEvent?.Invoke(codename, null);

        return new(true,
            $"World with the codename {codename} has been deactivated.",
            null);
    }
    #endregion

    #region Methods: Dictionary
    public static Response<Dictionary<string, World?>> GetWorldDict()
    {
        Dictionary<string, World?> worlds = _worlds;

        return new(true,
            "Successfully copied the worlds list.",
            worlds);
    }
    #endregion

}
