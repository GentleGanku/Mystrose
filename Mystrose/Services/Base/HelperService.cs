namespace Mystrose.Services.Base;

public abstract class HelperService(string name = "Default") : Service($"[Helper Service: {name}]")
{

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
