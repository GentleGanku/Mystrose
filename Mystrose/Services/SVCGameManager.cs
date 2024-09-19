namespace Mystrose.Services;

public class SVCGameManager
{

    #region Delegates & Handlers
    public delegate void GameHandler(string codename, HSTGame? game);
    public static event GameHandler ActivatedGameEvent;
    public static event GameHandler DeactivatedGameEvent;
    public static event GameHandler RenderedGameEvent;
    public static event GameHandler SelectedGameEvent;
    #endregion

    #region Fields
    public static string SelectedCodename = string.Empty;
    private static readonly Dictionary<string, HSTGame?> _games = new()
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

    #region Methods: Filing
    public static void Checkup()
    {
        try
        {
            Response<Option?> response = SVCSettings.Get("skippableHome");
            bool isHomeSkippable = response.Output!.Get<bool>();

            if (!isHomeSkippable)
            {
                Deselect();

                SVCLogger.LogOnConsole("Game Manager checkup completed; Loaded the Home panel.", "SVCGameManager", "Checkup");
                return;
            }

            Activate();

            SVCLogger.LogOnConsole("Game Manager checkup completed; Loaded the Game panel.", "SVCGameManager", "Checkup");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCGameManager", "Checkup");
        }
    }
    #endregion

    #region Methods: Read/Write
    public static Response<HSTGame?> Activate()
    {
        KeyValuePair<string, HSTGame?>? game = _games.First(g => g.Value is null);

        if (game is null)
        {
            return new(false, 
                "No available game client found.", 
                null);
        }

        string gameCodename = game.Value.Key;
        ClientUseIdentifier identifier = new(gameCodename);

        _games[gameCodename] = new(identifier);

        ActivatedGameEvent?.Invoke(gameCodename, _games[gameCodename]!);

        Render(gameCodename);

        return new(true, 
            $"Game client with the codename {gameCodename} has been activated.", 
            _games[gameCodename]);
    }

    public static Response<HSTGame?> Activate(string codename)
    {
        if (_games.TryGetValue(codename, out HSTGame? game) && game is not null)
        {
            return new(false,
                $"Game client with the codename {codename} is already activated.",
                game);
        }

        ClientUseIdentifier identifier = new(codename);

        game = new(identifier);

        ActivatedGameEvent?.Invoke(codename, game);

        Render(codename);

        return new(true,
            $"Game client with the codename {codename} has been activated.",
            game);
    }

    public static Response<HSTGame?> Deactivate()
    {
        KeyValuePair<string, HSTGame?>? game = _games.Last(g => g.Value is not null);

        if (game is null)
        {
            return new(false,
                "No activated game client found.",
                null);
        }

        string gameCodename = game.Value.Key;
        _games[gameCodename]!.Destruct();
        _games[gameCodename] = null;

        DeactivatedGameEvent?.Invoke(gameCodename, null);

        //Deselect();

        return new(true,
            $"Game client with the codename {gameCodename} has been deactivated.",
            null);
    }

    public static Response<HSTGame?> Deactivate(string codename)
    {
        if (_games.TryGetValue(codename, out HSTGame? game) && game is null)
        {
            return new(false,
                $"Game client with the codename {codename} is already deactivated.",
                null);
        }

        game!.Destruct();
        _games[codename] = null;

        DeactivatedGameEvent?.Invoke(codename, null);

        //Deselect();

        return new(true, 
            $"Game client with the codename {codename} has been deactivated.", 
            null);
    }

    public static Response<Dictionary<string, HSTGame?>> ActivateAll()
    {
        Dictionary<string, HSTGame?> emptyGames = _games.Where(g => g.Value is null).ToDictionary()!;

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

    public static Response<Dictionary<string, HSTGame?>> DeactivateAll()
    {
        Dictionary<string, HSTGame?> activatedGames = _games.Where(g => g.Value is not null).ToDictionary()!;

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

    public static Response<Dictionary<string, HSTGame?>> DeactivateAll(string exceptedCodename)
    {
        Dictionary<string, HSTGame?> activatedGames = _games.Where(g => g.Value is not null && !g.Key.Equals(exceptedCodename)).ToDictionary()!;

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
    public static async Task<Response<HSTGame?>> Render(string codename)
    {
        if (_games.TryGetValue(codename, out HSTGame? game) && game!.Child is not null)
        {
            return new(false,
                $"Game client with the codename {codename} is already rendered.",
                null);
        }

        await Task.Delay(100);
        game!.Refresh();

        RenderedGameEvent?.Invoke(codename, game);

        Select(codename);

        return new(true,
            $"Game client with the codename {codename} has been rendered.",
            game);
    }
    #endregion

    #region Methods: Selection
    public static Response<HSTGame?> Select(string codename)
    {
        if (_games.TryGetValue(codename, out HSTGame? game) && game is null)
        {
            return new(false,
                $"Game client with the codename {codename} is not activated yet.",
                null);
        }

        _selectedCodename = codename;

        SelectedGameEvent?.Invoke(codename, game);

        return new(true,
            $"Game client with the codename {codename} has been loaded to the interface.",
            game);
    }

    public static Response<HSTGame?> Deselect()
    {
        _selectedCodename = string.Empty;

        SelectedGameEvent?.Invoke(string.Empty, null);

        return new(true,
            $"Deselected active game client from the interface.",
            null);
    }
    #endregion

    #region Methods: Dictionary
    public static Response<Dictionary<string, HSTGame?>> GetGameDict()
    {
        Dictionary<string, HSTGame?> gameDict = _games;

        return new(true,
            "Successfully copied the codenames list.",
            gameDict);
    }

    public static Response<string[]> GetCodenames()
    {
        string[] codenames = [.. _games.Keys];

        return new(true,
            "Successfully copied the codenames list.",
            codenames);
    }
    #endregion

}
