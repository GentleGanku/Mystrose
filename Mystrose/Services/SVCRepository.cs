namespace Mystrose.Services;

public class SVCRepository
{

    #region Delegates & Handlers
    public delegate void ModelHandler<T>(T modelItem);
    public static event ModelHandler<Server> ServerEvent;
    public static event ModelHandler<MapFormat> MapEvent;
    #endregion

    #region Fields
    private static string _pathToFolder => "repository";
    private static JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };
    public static readonly Dictionary<string, RepositoryModel<GameObject>> Models = new()
    {
        [nameof(Server)] = new(),
        [nameof(MapFormat)] = new()
    };
    #endregion

    #region Methods: Filing
    public static void Checkup()
    {
        try
        {
            if (!Directory.Exists(_pathToFolder))
            {
                Directory.CreateDirectory(_pathToFolder);
            }

            foreach (var model in Models)
            {
                if (!File.Exists(_pathToFolder + $"\\{model.Key}.json"))
                {
                    SaveModel(model.Key);
                }

                LoadModel(model.Key);
            }

            SVCLogger.LogOnConsole("Repository checkup has been completed.", "SVCRepository", "Checkup");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCRepository", "Checkup");
        }
    }

    public static void Flush()
    {
        try
        {
            foreach (var model in Models)
            {
                SaveModel(model.Key);
            }

            SVCLogger.LogOnConsole("Repository has been flushed.", "SVCRepository", "Flush");
        }
        catch (Exception ex)
        {
            SVCLogger.LogOnConsole(ex.ToString(), "SVCRepository", "Flush");
        }
    }
    #endregion

    #region Methods: Read/Write
    public static Response<RepositoryModel<GameObject>?> LoadModel(string typeName)
    {
        string jsonDictionary = File.ReadAllText(_pathToFolder + $"\\{typeName}.json");

        switch (typeName)
        {       
            case nameof(Server):
                RepositoryModel<Server> serverModel = JsonSerializer.Deserialize<RepositoryModel<Server>>(jsonDictionary, _serializerOptions);
                Models[typeName] = new()
                {
                    LastUpdatedDate = serverModel.LastUpdatedDate,
                    List = serverModel.Get<GameObject>()
                };
                break;

            case nameof(MapFormat):
                RepositoryModel<MapFormat> mapModel = JsonSerializer.Deserialize<RepositoryModel<MapFormat>>(jsonDictionary, _serializerOptions);
                Models[typeName] = new()
                {
                    LastUpdatedDate = mapModel.LastUpdatedDate,
                    List = mapModel.Get<GameObject>()
                };
                break;

            default:
                return new(false,
                    "The model type is not supported.",
                    null);
        }

        return new(true,
            $"Loaded the {typeName} model from the repository.",
            Models[typeName]);
    }

    public static Response<RepositoryModel<GameObject>?> AddModel(List<Server> items)
    {
        RepositoryModel<GameObject> repositoryModel = Models[nameof(Server)];
        foreach (var item in items)
        {
            Server? existingItem = repositoryModel.Get<Server>().Find(i => i.Name.Equals(item.Name));

            if (existingItem is null)
            {
                repositoryModel.List.Add(item);
            }
            else
            {
                existingItem = item;
            }

            ServerEvent?.Invoke(item);
        }

        repositoryModel.LastUpdatedDate = DateTime.Now;

        return new(true,
            $"Added the {items.Count} servers to the repository.",
            repositoryModel);
    }

    public static Response<RepositoryModel<GameObject>?> AddModel(List<MapFormat> items)
    {
        RepositoryModel<GameObject> repositoryModel = Models[nameof(MapFormat)];
        foreach (var item in items)
        {
            MapFormat? existingItem = repositoryModel.Get<MapFormat>().Find(i => i.Name.Equals(item.Name));

            if (existingItem is null)
            {
                repositoryModel.List.Add(item);
            }
            else
            {
                existingItem = item;
            }

            MapEvent?.Invoke(item);
        }

        repositoryModel.LastUpdatedDate = DateTime.Now;

        return new(true,
            $"Added the {items.Count} maps to the repository.",
            repositoryModel);
    }

    public static Response<RepositoryModel<GameObject>?> SaveModel(string typeName)
    {
        if (!Models.TryGetValue(typeName, out RepositoryModel<GameObject> repositoryModel))
        {
            return new(false,
                "The model type is not supported.",
                null);
        }

        string jsonDictionary = string.Empty;
        switch (typeName)
        {
            case nameof(Server):
                RepositoryModel<Server> serverModel = new()
                {
                    LastUpdatedDate = repositoryModel.LastUpdatedDate,
                    List = repositoryModel.Get<Server>()
                };
                jsonDictionary = JsonSerializer.Serialize(serverModel, _serializerOptions);
                break;

            case nameof(MapFormat):
                RepositoryModel<MapFormat> mapModel = new()
                {
                    LastUpdatedDate = repositoryModel.LastUpdatedDate,
                    List = repositoryModel.Get<MapFormat>()
                };
                jsonDictionary = JsonSerializer.Serialize(mapModel, _serializerOptions);
                break;
        }

        File.WriteAllText(_pathToFolder + $"\\{typeName}.json", jsonDictionary);

        return new(true,
            $"Saved the {typeName} model to the repository.",
            repositoryModel);
    }
    #endregion

}
