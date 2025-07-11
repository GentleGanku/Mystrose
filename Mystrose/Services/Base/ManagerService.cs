namespace Mystrose.Services.Base;

public abstract class ManagerService<T>(string name = "Default") : Service($"[Manager Service: {name}]")
{

    #region Fields
    public (string, T?) this[string codename]
    {
        get
        {
            if (Items.TryGetValue(codename, out T? instance) && instance is null)
            {
                return default;
            }

            return (codename, instance);
        }
        set
        {
            if (Items.TryGetValue(codename, out T? instance) && instance is null)
            {
                return;
            }

            instance = value.Item2;
        }
    }

    public Dictionary<string, T?> Collection
    {
        get => Items;
    }

    public Dictionary<string, T?> ActiveCollection
    {
        get => Items.Where(i => i.Value is not null).ToDictionary(i => i.Key, i => i.Value);
    }

    public Dictionary<string, T?> InactiveCollection
    {
        get => Items.Where(i => i.Value is null).ToDictionary(i => i.Key, i => i.Value);
    }
    #endregion

    #region Properties
    protected Dictionary<string, T?> Items
    {
        get;
        set;
    } = new()
    {
        ["Avernus"] = default,
        ["Beatrix"] = default,
        ["Cassiopeia"] = default,
        ["Durandal"] = default,
        ["Eligos"] = default,
        ["Fenrir"] = default,
        ["Gwyndell"] = default,
        ["Harbinger"] = default,
    };
    #endregion

    #region Methods: Overrides
    public override void Construct()
    {
        try
        {
            Log($"{Name} has been constructed.", "Construct");
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

            Log($"{Name} has been deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

}