namespace Mystrose.DataRecords.ReadableModels;

public class RMQuest(Quest? model = null, World? world = null) : ReadableModel(model ?? new Quest(), world ?? new World())
{

    #region Properties: I/O
    public new Quest Model
    {
        get => (Quest)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(ID)] = ID
        };
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public string Name => Model.Name;
    public bool Is_One_Time => Model.IsOneTime;
    public string Status => Model.StatusType.ToString();
    #endregion

    #region Methods: Conversion
    public new Quest ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | ID {ID}";
    }
    #endregion

}