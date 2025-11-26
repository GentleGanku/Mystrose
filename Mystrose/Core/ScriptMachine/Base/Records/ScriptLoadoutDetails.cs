namespace Mystrose.Core.ScriptMachine.Base.Records;

public class ScriptLoadoutDetails
{

    #region Properties: Loadout
    public Guid UUID
    {
        get;
        protected set;
    } = Guid.NewGuid();

    public string Name
    {
        get;
        protected set;
    } = "New Loadout - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

    public bool IsEnabled
    {
        get;
        protected set;
    } = true;

    public string Author
    {
        get;
        protected set;
    } = "Unknown";

    public string Documentation
    {
        get;
        protected set;
    } = "No documentation available.";

    [JsonIgnore]
    public string FilePath
    {
        get;
        protected set;
    } = string.Empty;

    [JsonIgnore]
    public DateTime CreationDate
    {
        get;
        protected set;
    } = DateTime.Now;

    [JsonIgnore]
    public DateTime LastModifiedDate
    {
        get;
        protected set;
    } = DateTime.Now;
    #endregion

    #region Methods: Details Retrieval
    public void RetrieveFileDetails(FileInfo info)
    {
        FilePath = info.FullName;
        CreationDate = info.CreationTime;
        LastModifiedDate = info.LastWriteTime;
    }
    #endregion

}
