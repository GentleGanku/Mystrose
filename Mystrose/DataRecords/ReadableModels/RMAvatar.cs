namespace Mystrose.DataRecords.ReadableModels;

public class RMAvatar(Avatar? model = null, World? world = null) : ReadableModel(model ?? new Avatar(), world ?? new World())
{

    #region Properties: I/O
    public new Avatar Model
    {
        get => (Avatar)base.Model;
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
    public string Name => Model.Name;
    public int Access_Level => (int)Model.AccessType;
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
    public int HP_Percentage => Model.HPPercentage;
    public int Max_MP => Model.MaxMP;
    public int MP => Model.MP;
    public int MP_Percentage => Model.MPPercentage;
    public int Max_SP => Model.MaxSP;
    public int SP => Model.SP;
    public string Targets => string.Join("|", Model.Targets);
    #endregion

    #region Methods: Conversion
    public new Avatar ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | In {Cell}, {Pad}";
    }
    #endregion

}