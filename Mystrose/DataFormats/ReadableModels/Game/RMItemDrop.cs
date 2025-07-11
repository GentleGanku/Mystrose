namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMItemDrop : ReadableModel<BaseItem>
{

    #region Constructor
    public RMItemDrop(BaseItem? model = null, World? world = null) 
        : base(model ?? new BaseItem(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Properties
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
    public override string ToString()
    {
        return $"{Name} | ID {ID} - x{Quantity}";
    }
    #endregion

}