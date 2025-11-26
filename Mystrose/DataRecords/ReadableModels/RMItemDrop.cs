namespace Mystrose.DataRecords.ReadableModels;

public class RMItemDrop(BaseItem? model = null, World? world = null) : ReadableModel(model ?? new BaseItem(), world ?? new World())
{

    #region Properties: I/O
    public new BaseItem Model
    {
        get => (BaseItem)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public string Name => Model.Name;
    public string Equipment_Type => Model.EquipmentType.ToString();
    public string Category_Type => Model.Type.ToString();
    public int Level => Model.Level;
    public int Quantity => Model.Quantity;
    public int Maximum_Stack => Model.MaxStack;
    public bool Is_Temporary => Model.IsTemporary;
    public bool Is_Member_Tagged => Model.IsMemberTagged;
    public bool Is_Coin_Tagged => Model.IsCoinTagged;
    public string Metadata => Model.Metadata;
    #endregion

    #region Methods: Overrides
    public new BaseItem ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | ID {ID} - x{Quantity}";
    }
    #endregion

}