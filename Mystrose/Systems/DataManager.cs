using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mystrose.GameModels.Environment;
using Mystrose.GameModels.Network;

namespace Mystrose.Systems;

public class DataManager
{

    #region Variables
    public string FolderPath
    {
        get => AppDomain.CurrentDomain.BaseDirectory + "\\SavedData";
    }

    public List<string> Paths
    {
        get =>
        [
            "ScriptLoadouts",
            "Servers",
            "Maps",
            "Shops",
            "Quests"
        ];
    }
    #endregion

    #region Properties
    [JsonInclude]
    public List<Server> Servers
    {
        get;
        set;
    } = new List<Server>();

    [JsonInclude]
    public List<MapFormat> Maps
    {
        get;
        set;
    } = new List<MapFormat>();

    [JsonInclude]
    public List<Shop> Shops
    {
        get;
        set;
    } = new List<Shop>();

    [JsonInclude]
    public List<Quest> Quests
    {
        get;
        set;
    } = new List<Quest>();
    #endregion

    #region Methods - Main
    public void SaveAll()
    {
        foreach (string path in Paths)
        {
            Save(path);
        }
    }

    public void Save(string dataName)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonString = null;

        switch (dataName)
        {
            case "Servers":
                jsonString = JsonSerializer.Serialize(Servers, options);
                break;
            case "Maps":
                jsonString = JsonSerializer.Serialize(Maps, options);
                break;
            case "Shops":
                jsonString = JsonSerializer.Serialize(Shops, options);
                break;
            case "Quests":
                jsonString = JsonSerializer.Serialize(Quests, options);
                break;

            default:
                return;
        }

        File.WriteAllText(FolderPath + $"\\{dataName}.json", jsonString);
    }

    public DataManager Load()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        foreach (string path in Paths)
        {
            string filePath = FolderPath + $"\\{path}.json";
            
            if (!File.Exists(filePath))
            {
                Save(path);
            }

            string jsonString = File.ReadAllText(filePath);

            switch (path)
            {
                case "Servers":
                    Servers = JsonSerializer.Deserialize<List<Server>>(jsonString);
                    break;
                case "Maps":
                    Maps = JsonSerializer.Deserialize<List<MapFormat>>(jsonString);
                    break;
                case "Shops":
                    Shops = JsonSerializer.Deserialize<List<Shop>>(jsonString);
                    break;
                case "Quests":
                    Quests = JsonSerializer.Deserialize<List<Quest>>(jsonString);
                    break;
            }
        }

        return this;
    }
    #endregion

    #region Methods - Properties
    public void Add(object value)
    {
        string path = null;

        switch (value)
        {
            case Server server:
                path = "Servers";
                Servers.Add(server);
                break;
            case MapFormat map:
                path = "Maps";
                Maps.Add(map);
                break;
            case Shop shop:
                path = "Shops";
                Shops.Add(shop);
                break;
            case Quest quest:
                path = "Quests";
                Quests.Add(quest);
                break;

            default:
                return;
        }

        Save(path);
    }
    #endregion

}
