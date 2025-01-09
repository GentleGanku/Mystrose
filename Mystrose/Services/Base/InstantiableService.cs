namespace Mystrose.Services.Base;

public abstract class InstantiableService(ClientInstanceIdentifier identifier, string name = "Default") : Service($"[Instantiable Service: {name}]")
{

    #region Properties
    public ClientInstanceIdentifier Identifier
    {
        get;
        private init;
    } = identifier;
    #endregion

    #region Methods: Overrides
    public override void Construct()
    {
        try
        {
            Log($"{Name} has been constructed.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            Log($"{Name} has been deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

}
