namespace Mystrose.DataRecords.Game.Base;

public class AuraDictionary : Dictionary<EntityType, Dictionary<string, List<Aura>>>
{

    #region Constructor
    public AuraDictionary()
    {
        base[EntityType.Player] = [];
        base[EntityType.Monster] = [];
        base[EntityType.Unknown] = [];
    }
    #endregion

    #region Fields
    public new Dictionary<string, List<Aura>> this[EntityType type]
    {
        get => base[type];
    }

    public List<Aura>? this[EntityType type, string id]
    {
        get
        {
            if (!this[type].TryGetValue(id, out List<Aura>? value))
            {
                this[type].Add(id, new());
                return this[type][id];
            }

            return value;
        }
    }

    public Aura? this[EntityType type, string id, string name]
    {
        get
        {
            List<Aura>? list = this[type, id];

            return list!.Find(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
    #endregion

    #region Methods
    public void Add(Aura aura)
    {
        List<Aura>? list = this[aura.TargetType, aura.TargetID];

        if (list.Find(a => a.Name == aura.Name) is not null)
        {
            aura.Refresh();
        }
        else
        {
            aura.IsAdded = true;
            list.Add(aura);
        }
    }

    public void Remove(Aura aura)
    {
        List<Aura>? list = this[aura.TargetType, aura.TargetID];

        if (list.Find(a => a.Name == aura.Name) is null)
        {
            return;
        }
        else
        {
            aura.IsAdded = false;
            list.Remove(aura);
        }
    }
    #endregion

}
