namespace Mystrose.Services;

public class MSVCWorld() : ManagerService<World>(nameof(MSVCWorld))
{

    #region Delegates & Handlers
    public delegate void WorldHandler(string codename, World? world);
    public event WorldHandler ActivatedWorldEvent;
    public event WorldHandler DeactivatedWorldEvent;
    #endregion

    #region (Static) Fields
    public static MSVCWorld Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCWorld();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCWorld? _instance;
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            Log("World Hosting has been constructed.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            Items.Clear();

            Log("World Hosting has been deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Read/Write
    public Response<World?> Activate(string codename, string serverName)
    {
        if (Items.TryGetValue(codename, out World? world) && world is not null)
        {
            return new(false,
                $"World with the codename {codename} is already activated.",
                world);
        }

        ClientInstanceIdentifier identifier = new(codename);
        List<Server> servers = HSVCRepository.Instance.Models[nameof(Server)].Get<Server>();
        Server server = servers.Find(s => s.Name.Equals(serverName))!;

        Items[codename] = new(identifier, server);

        ActivatedWorldEvent?.Invoke(codename, Items[codename]);

        return new(true,
            $"World with the codename {codename} has been activated.",
            world);
    }

    public Response<World?> Deactivate(string codename)
    {
        if (Items.TryGetValue(codename, out World? world) && world is null)
        {
            return new(false,
                $"World with the codename {codename} is already deactivated.",
                world);
        }

        Items[codename]!.Destruct();
        Items[codename] = null;

        DeactivatedWorldEvent?.Invoke(codename, null);

        return new(true,
            $"World with the codename {codename} has been deactivated.",
            null);
    }
    #endregion

}
