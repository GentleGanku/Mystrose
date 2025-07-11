namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMQuest : ReadableModel<Quest>
{

    #region Constructor
    public RMQuest(Quest? model = null, World? world = null) 
        : base(model ?? new Quest(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(ID)] = ID
        };
    }
    #endregion

    #region Properties
    public int ID => Model.ID;
    public string Name => Model.Name;
    public bool Is_One_Time => Model.IsOneTime;
    public string Status => Model.StatusType.ToString();
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | ID {ID}";
    }
    #endregion

}