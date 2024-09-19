namespace Mystrose.DataFormats.Modules;

public struct ClientUseIdentifier
{

    #region Constructor
    public ClientUseIdentifier(string codename)
    {
        Codename = codename;
    }
    #endregion

    #region Properties
    public string Codename
    {
        get;
        set;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return Codename;
    }
    #endregion

}
