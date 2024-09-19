namespace Mystrose.DataFormats.Modules;

public struct RepositoryModel<T>
{

    #region Constructor
    public RepositoryModel()
    {
        LastUpdatedDate = DateTime.Now;
        List = [];
    }

    public RepositoryModel(List<T> list)
    {
        LastUpdatedDate = DateTime.Now;
        List = list;
    }
    #endregion

    #region Properties
    public DateTime LastUpdatedDate
    {
        get;
        set;
    }

    public List<T> List
    {
        get;
        set;
    }
    #endregion

    #region Methods
    public List<LT> Get<LT>()
    {
        return List.Cast<LT>().ToList();
    }
    #endregion

}
