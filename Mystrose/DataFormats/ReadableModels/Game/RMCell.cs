namespace Mystrose.DataFormats.ReadableModels.Game;

public class RMCell : ReadableModel<Cell>
{

    #region Constructor
    public RMCell(Cell? model = null, World? world = null) 
        : base(model ?? new Cell(), world ?? new World())
    {
        KeyProperties = new();
    }
    #endregion

    #region Properties
    public string Name => Model.Name;
    public string Pads => string.Join("|", Model.Pads);
    public string Map_Items => string.Join("|", Model.MapItems.Select(m => $"{m.ID}:{m.Name}"));
    #endregion

    #region Methods: Overrides
    public override string ToString()
    {
        return $"{Name} | {Model.MapItems.Count} map items";
    }
    #endregion

}