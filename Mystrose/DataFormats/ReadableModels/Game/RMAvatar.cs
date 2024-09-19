namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMAvatar : ReadableModel
{

    #region Constructor
    public RMAvatar(Avatar? model = null, World? world = null) : base(model, world)
    {
        Model = model ?? new Avatar();
        MandatorySearchProperties = new()
        {
            [nameof(Name)] = Name
        };
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private Avatar Avatar
    {
        get => (Avatar)Model;
    }
    #endregion

    #region Fields
    public string Name
    {
        get => Avatar.Name;
    }

    public int Access_Level
    {
        get => (int)Avatar.AccessType;
    }

    public int State
    {
        get => (int)Avatar.State;
    }

    public string Cell
    {
        get => Avatar.Cell;
    }

    public string Pad
    {
        get => Avatar.Pad;
    }

    public double X_Coordinate
    {
        get => Avatar.X;
    }

    public double Y_Coordinate
    {
        get => Avatar.Y;
    }

    public string Gender
    {
        get => Avatar.Gender.ToString();
    }

    public int Level
    {
        get => Avatar.Level;
    }

    public string Class
    {
        get => Avatar.Class;
    }

    public int Class_Points
    {
        get => Avatar.ClassPoints;
    }

    public int Class_Rank
    {
        get => Avatar.ClassRank;
    }

    public string Equipments
    {
        get => string.Join("|", Avatar.Equipments.Values.Select(i => i.ID));
    }

    public bool Is_AFK
    {
        get => Avatar.IsAFK;
    }

    public bool Is_Resting
    {
        get => Avatar.IsResting;
    }

    public bool Is_Member
    {
        get => Avatar.IsMember;
    }

    public int Max_HP
    {
        get => Avatar.MaxHP;
    }

    public int HP
    {
        get => Avatar.HP;
    }

    public int HP_Percentage
    {
        get => Avatar.HPPercentage;
    }

    public int Max_MP
    {
        get => Avatar.MaxMP;
    }

    public int MP
    {
        get => Avatar.MP;
    }

    public int MP_Percentage
    {
        get => Avatar.MPPercentage;
    }

    public int Max_SP
    {
        get => Avatar.MaxSP;
    }

    public int SP
    {
        get => Avatar.SP;
    }

    public string Targets
    {
        get => string.Join("|", Avatar.Targets);
    }
    #endregion

}
