namespace Mystrose.DataFormats.Modules;

public struct ClientInstanceIdentifier
{

    #region Constructor
    public ClientInstanceIdentifier(string codename)
    {
        Codename = codename;
    }
    #endregion

    #region Properties
    public string Codename
    {
        get;
        init;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return Codename;
    }
    #endregion

}
