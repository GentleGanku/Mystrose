namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMSelf : ReadableModel<MainAvatar>
{

    #region Constructor
    public RMSelf(MainAvatar? model = null, World? world = null) 
        : base(model ?? new MainAvatar(), world ?? new World())
    {
        KeyProperties = new();
    }
    #endregion

    #region Properties
    public int State => (int)Model.State;
    public string Cell => Model.Cell;
    public string Pad => Model.Pad;
    public double X_Coordinate => Model.X;
    public double Y_Coordinate => Model.Y;
    public string Gender => Model.Gender.ToString();
    public int Level => Model.Level;
    public string Class => Model.Class;
    public int Class_Points => Model.ClassPoints;
    public int Class_Rank => Model.ClassRank;
    public string Equipments => string.Join("|", Model.Equipments.Values.Select(i => i.ID));
    public bool Is_AFK => Model.IsAFK;
    public bool Is_Resting => Model.IsResting;
    public bool Is_Member => Model.IsMember;
    public int Max_HP => Model.MaxHP;
    public int HP => Model.HP;
    public int Max_MP => Model.MaxMP;
    public int MP => Model.MP;
    public int Max_SP => Model.MaxSP;
    public int SP => Model.SP;
    public string Targets => string.Join("|", Model.Targets);
    public int Available_Inventory_Slots => World?.Inventories[InventoryType.Base].FreeSlots ?? 0;
    public int Available_House_Inventory_Slots => World?.Inventories[InventoryType.House].FreeSlots ?? 0;
    public int Available_Bank_Slots => World?.Inventories[InventoryType.Bank].FreeSlots ?? 0;
    public int Gold => Model.Gold;
    public int Adventure_Coins => Model.Coins;
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Model.Name}";
    }
    #endregion

}