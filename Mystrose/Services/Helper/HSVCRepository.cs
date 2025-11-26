using Mystrose.DataRecords.Game;

namespace Mystrose.Services.Helper;

public class HSVCRepository() : HelperService(nameof(HSVCRepository))
{

    #region Delegates & Handlers
    public delegate void ModelHandler<T>(T modelItem);
    public event ModelHandler<Server> ServerEvent;
    public event ModelHandler<MapFormat> MapEvent;
    #endregion

    #region (Static) Fields
    public static HSVCRepository Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new HSVCRepository();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static HSVCRepository? _instance;
    #endregion

    #region Fields
    private readonly string _pathToFolder = "repository";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };
    #endregion

    #region Properties
    public Dictionary<string, RepositoryModel<GameObject>> Models
    {
        get;
        init;
    } = new()
    {
        [nameof(Server)] = new(),
        [nameof(MapFormat)] = new()
    };
    #endregion
    
    #region Methods: Builder
    public override void Construct()
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

            Log("Repository constructed successfully.", "Construct");
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
            foreach (var model in Models)
            {
                model.Value.List.Clear();
            }

            Log("Repository deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }

    public void Flush()
    {
        try
        {
            foreach (var model in Models)
            {
                SaveModel(model.Key);
            }

            Log("Repository flushed successfully.", "Flush");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Flush");
        }
    }
    #endregion

    #region Methods: Read/Write
    public Response<RepositoryModel<GameObject>?> LoadModel(string typeName)
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

    public Response<RepositoryModel<GameObject>?> AddModel(List<Server> items)
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

    public Response<RepositoryModel<GameObject>?> AddModel(List<MapFormat> items)
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

    public Response<RepositoryModel<GameObject>?> SaveModel(string typeName)
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
