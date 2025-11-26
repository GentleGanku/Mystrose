namespace Mystrose.DataRecords.ReadableModels;

public class RMCell(Cell? model = null, World? world = null) : ReadableModel(model ?? new Cell(), world ?? new World())
{

    #region Properties: I/O
    public new Cell Model
    {
        get => (Cell)base.Model;
    }
    #endregion

    #region Properties: Attributes
    public string Name => Model.Name;
    public string Pads => string.Join("|", Model.Pads);
    public string Map_Items => string.Join("|", Model.MapItems.Select(m => $"{m.ID}:{m.Name}"));
    #endregion

    #region Methods: Conversion
    public new Cell ToObject()
    {
        return Model;
    }

    public override string ToString()
    {
        return $"{Name} | {Model.MapItems.Count} map items";
    }
    #endregion

}