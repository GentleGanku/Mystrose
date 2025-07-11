namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMMonster : ReadableModel<Monster>
{

    #region Constructor
    public RMMonster(Monster? model = null, World? world = null) 
        : base(model ?? new Monster(), world ?? new World())
    {
        KeyProperties = new()
        {
            [nameof(Monster_Map_ID)] = Monster_Map_ID
        };
        Format = World!.Area.Format.MonsterFormats.Find(mf => mf.ID == ID) ?? new MonsterFormat();
    }
    #endregion

    #region Private Fields
    [JsonIgnore]
    private MonsterFormat Format
    {
        get;
        init;
    }
    #endregion

    #region Properties
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

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | MMID {Monster_Map_ID} / In {Cell}";
    }
    #endregion

}