namespace Mystrose.DataFormats.Modules;

public class GameRecord
{

    #region Constructor
    public GameRecord(bool isTemporary, params GameObject[] objects)
    {
        IsTemporary = isTemporary;
        Objects = new List<(DateTime, GameObject)>(objects.Select(obj => (DateTime.Now, obj)));
    }
    #endregion

    #region Properties
    public bool IsTemporary
    {
        get;
        init;
    }

    public List<(DateTime, GameObject)> Objects
    {
        get;
        init;
    }
    #endregion

    #region Fields
    public (DateTime, GameObject) this[int index]
    {
        get => Objects[index];
    }
    #endregion

    #region Getters
    public (DateTime, T)[] GetObjects<T>() where T : GameObject
    {
        return Objects
            .Where(o => o.Item2 is T)
            .Select(o => (o.Item1, (T)o.Item2))
            .ToArray();
    }
    #endregion
    
    #region Methods
    public void Add(GameObject obj)
    {
        Objects.Add(new (DateTime.Now, obj));
    }

    public void Expire()
    {
        if (!IsTemporary)
        {
            return;
        }

        Objects.Clear();
    }
    #endregion

}
