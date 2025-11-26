namespace Mystrose.DataRecords.ReadableModels;

public class RMMonster(Monster? model = null, World? world = null) : ReadableModel(model ?? new Monster(), world ?? new World())
{

    #region Fields
    [JsonIgnore]
    public MonsterFormat Format
    {
        get => World!.Area.Format.MonsterFormats.Find(mf => mf.ID == ID) ?? new MonsterFormat();
    }
    #endregion

    #region Properties: I/O
    public new Monster Model
    {
        get => (Monster)base.Model;
    }

    public override Dictionary<string, object> KeyProperties
    {
        get => new()
        {
            [nameof(Monster_Map_ID)] = Monster_Map_ID
        };
    }
    #endregion

    #region Properties: Attributes
    public int ID => Model.ID;
    public string Name => Format.Name;
    public int Monster_Map_ID => Model.MonMapID;
    public int State => (int)Model.State;
    public string Cell => Model.Cell;
    public string Targets => string.Join("|", Model.Targets);
    public double Max_HP => Model.MaxHP;
    public double HP => Model.HP;
    public int Max_MP => Model.MaxMP;
    public int MP => Model.MP;
    public double DPS => Model.DPS;
    public bool Is_Aggressive => Model.IsAggressive;
    #endregion

    #region Methods: Conversion
    public new Monster ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | MMID {Monster_Map_ID} / In {Cell}";
    }
    #endregion

}