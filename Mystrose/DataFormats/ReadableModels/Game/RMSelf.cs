namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMSelf : ReadableModel
{

    #region Constructor
    public RMSelf(MainAvatar? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new MainAvatar();
        MandatorySearchProperties = new()
        {
            //
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private MainAvatar MainAvatar
    {
        get => (MainAvatar)Model;
    }
    #endregion

    #region Properties
    public int State
    {
        get => (int)MainAvatar.State;
    }

    public string Cell
    {
        get => MainAvatar.Cell;
    }

    public string Pad
    {
        get => MainAvatar.Pad;
    }

    public double X_Coordinate
    {
        get => MainAvatar.X;
    }

    public double Y_Coordinate
    {
        get => MainAvatar.Y;
    }

    public string Gender
    {
        get => MainAvatar.Gender.ToString();
    }

    public int Level
    {
        get => MainAvatar.Level;
    }

    public string Class
    {
        get => MainAvatar.Class;
    }

    public int Class_Points
    {
        get => MainAvatar.ClassPoints;
    }

    public int Class_Rank
    {
        get => MainAvatar.ClassRank;
    }

    public string Equipments
    {
        get => string.Join("|", MainAvatar.Equipments.Values.Select(i => i.Name));
    }

    public bool Is_AFK
    {
        get => MainAvatar.IsAFK;
    }

    public bool Is_Resting
    {
        get => MainAvatar.IsResting;
    }

    public bool Is_Member
    {
        get => MainAvatar.IsMember;
    }

    public int Max_HP
    {
        get => MainAvatar.MaxHP;
    }

    public int HP
    {
        get => MainAvatar.HP;
    }

    public int Max_MP
    {
        get => MainAvatar.MaxMP;
    }

    public int MP
    {
        get => MainAvatar.MP;
    }

    public int Max_SP
    {
        get => MainAvatar.MaxSP;
    }

    public int SP
    {
        get => MainAvatar.SP;
    }

    public string Targets
    {
        get => string.Join("|", MainAvatar.Targets);
    }

    public int Available_Inventory_Slots
    {
        get => World is not null ? World.Inventory.FreeSlots : 0;
    }

    public int Available_House_Inventory_Slots
    {
        get => World is not null ? World.HouseInventory.FreeSlots : 0;
    }

    public int Available_Bank_Slots
    {
        get => World is not null ? World.BankInventory.FreeSlots : 0;
    }

    public int Gold
    {
        get => MainAvatar.Gold;
    }

    public int Adventure_Coins
    {
        get => MainAvatar.AdventureCoins;
    }

    //public bool Has_Reputation_Boost
    //{
    //    get => MainAvatar.RepBoost;
    //}

    //public bool Has_Gold_Boost
    //{
    //    get => MainAvatar.GoldBoost;
    //}

    //public bool Has_Experience_Boost
    //{
    //    get => MainAvatar.XPBoost;
    //}

    //public bool Has_Class_Point_Boost
    //{
    //    get => MainAvatar.CPBoost;
    //}

    public int Activation_Flag
    {
        get => MainAvatar.ActivationFlag;
    }
    #endregion

}
