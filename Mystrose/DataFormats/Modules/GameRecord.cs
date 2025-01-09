namespace Mystrose.DataFormats.Modules;

public class GameRecord<T> where T : GameObject
{

    #region Constructor
    public GameRecord(bool isTemporary, params T[] objects)
    {
        IsTemporary = isTemporary;
        Objects = new List<T>(objects);
    }
    #endregion

    #region Properties
    public bool IsTemporary
    {
        get;
        set;
    }

    public List<T> Objects
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public void Add(T obj)
    {
        Objects.Add(obj);
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
