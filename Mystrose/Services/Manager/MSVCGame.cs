namespace Mystrose.Services.Manager;

public class MSVCGame() : ManagerService<HSTGame>(nameof(MSVCGame))
{

    #region Delegates & Handlers
    public delegate void GameHandler(string codename, HSTGame? game);
    public event GameHandler ActivatedGameEvent;
    public event GameHandler DeactivatedGameEvent;
    public event GameHandler RenderedGameEvent;
    public event GameHandler SelectedGameEvent;
    #endregion

    #region (Static) Fields
    public static MSVCGame Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCGame();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCGame? _instance;
    #endregion

    #region Properties
    public string CurrentCodename
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            Response<Option?> response = HSVCSettings.Instance.Get(SettingOption.SkippableHome);
            bool isHomeSkippable = response.Output!.Get<bool>();

            if (!isHomeSkippable)
            {
                Deselect();

                Log("Game Hosting service has been constructed.", "Construct");
                return;
            }

            Activate();

            Log("Game Hosting service has been constructed. Skipping the Home screen...", "Construct");
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
            Deselect();
            DeactivateAll();

            Items.Clear();

            Log("Game Hosting service has been deconstructed.", "Deconstruct");
       }
       catch (Exception ex)
       {
            Log(ex.ToString(), "Deconstruct");
       }
    }
    #endregion

    #region Methods: Read/Write
    public Response<HSTGame?> Activate()
    {
        KeyValuePair<string, HSTGame?>? game = InactiveCollection.First();

        if (game is null)
        {
            return new(false, 
                "No available game host found.", 
                null);
        }

        string gameCodename = game.Value.Key;
        ClientInstanceIdentifier identifier = new(gameCodename);

        Items[gameCodename] = new(identifier);

        ActivatedGameEvent?.Invoke(gameCodename, Items[gameCodename]!);

        Render(gameCodename);

        return new(true, 
            $"Game host with the codename {gameCodename} has been activated.",
            Items[gameCodename]);
    }

    public Response<HSTGame?> Activate(string codename)
    {
        if (Items.TryGetValue(codename, out HSTGame? game) && game is not null)
        {
            return new(false,
                $"Game host with the codename {codename} is already activated.",
                game);
        }

        ClientInstanceIdentifier identifier = new(codename);

        game = new(identifier);

        ActivatedGameEvent?.Invoke(codename, game);

        Render(codename);

        return new(true,
            $"Game host with the codename {codename} has been activated.",
            game);
    }

    public Response<Dictionary<string, HSTGame?>> ActivateAll()
    {
        Dictionary<string, HSTGame?> emptyGames = InactiveCollection;

        if (emptyGames.Count <= 0)
        {
            return new(false,
                "No empty game clients found.",
                emptyGames);
        }

        foreach (var game in emptyGames)
        {
            Activate(game.Key);
            Task.Delay(10);
        }

        return new(true,
            "All game clients have been activated.",
            emptyGames);
    }

    public Response<HSTGame?> Deactivate()
    {
        KeyValuePair<string, HSTGame?>? game = ActiveCollection.Last();

        if (game is null)
        {
            return new(false,
                "No activated game host found.",
                null);
        }

        string gameCodename = game.Value.Key;
        Items[gameCodename]!.Destruct();
        Items[gameCodename] = null;

        DeactivatedGameEvent?.Invoke(gameCodename, null);

        return new(true,
            $"Game host with the codename {gameCodename} has been deactivated.",
            null);
    }

    public Response<HSTGame?> Deactivate(string codename)
    {
        if (Items.TryGetValue(codename, out HSTGame? game) && game is null)
        {
            return new(false,
                $"Game host with the codename {codename} is already deactivated.",
                null);
        }

        game!.Destruct();
        Items[codename] = null;

        DeactivatedGameEvent?.Invoke(codename, null);

        return new(true, 
            $"Game host with the codename {codename} has been deactivated.", 
            null);
    }

    public Response<Dictionary<string, HSTGame?>> DeactivateAll()
    {
        Dictionary<string, HSTGame?> activatedGames = ActiveCollection;

        if (activatedGames.Count <= 0)
        {
            return new(false,
                "No activated game clients found.",
                activatedGames);
        }

        foreach (var game in activatedGames)
        {
            Deactivate(game.Key);
        }

        return new(true,
            "All game clients have been deactivated.",
            activatedGames);
    }

    public Response<Dictionary<string, HSTGame?>> DeactivateAll(string exceptedCodename)
    {
        Dictionary<string, HSTGame?> activatedGames = ActiveCollection.Where(g => !g.Key.Equals(exceptedCodename)).ToDictionary()!;

        if (activatedGames.Count <= 0)
        {
            return new(false,
                "No activated game clients found.",
                activatedGames);
        }

        foreach (var game in activatedGames)
        {
            Deactivate(game.Key);
        }

        return new(true,
            "All game clients have been deactivated.",
            activatedGames);
    }
    #endregion

    #region Methods: Rendering
    public async Task<Response<HSTGame?>> Render(string codename)
    {
        if (Items.TryGetValue(codename, out HSTGame? game) && game!.Child is not null)
        {
            return new(false,
                $"Game host with the codename {codename} is already rendered.",
                null);
        }

        await Task.Delay(100);
        game!.Refresh();

        RenderedGameEvent?.Invoke(codename, game);

        Select(codename);

        return new(true,
            $"Game host with the codename {codename} has been rendered.",
            game);
    }
    #endregion

    #region Methods: Selection
    public Response<HSTGame?> Select(string codename)
    {
        if (Items.TryGetValue(codename, out HSTGame? game) && game is null)
        {
            return new(false,
                $"Game host with the codename {codename} is not activated yet.",
                null);
        }

        CurrentCodename = codename;

        SelectedGameEvent?.Invoke(CurrentCodename, game);

        return new(true,
            $"Game host with the codename {codename} has been loaded to the interface.",
            game);
    }

    public Response<HSTGame?> Deselect()
    {
        CurrentCodename = string.Empty;

        SelectedGameEvent?.Invoke(CurrentCodename, null);

        return new(true,
            $"Deselected active game host from the interface.",
            null);
    }
    #endregion

}
