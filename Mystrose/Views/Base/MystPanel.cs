using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Base;

public class MystPanel : UserControl, IDestructible
{

    #region Constructor
    public MystPanel(MystWindow parentWindow) : base()
    {
        ParentWindow = parentWindow;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    #endregion

    #region Properties
    public MystWindow ParentWindow
    {
        get;
        set;
    }
    #endregion

    #region Methods: Setup
    protected virtual void Initialize()
    {
        HSVCLogger.Instance.LogOnConsole("MystPanel is pre-initialized.", $"MystPanel-{Name}", "InitializeComponent");
    }

    public virtual void Destruct()
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    #endregion

    #region Methods: Interface Handlers
    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        Initialize();

        HSVCLogger.Instance.LogOnConsole("MystPanel is ready to go.", $"MystPanel-{Name}", "OnLoaded");
    }

    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
        HSVCLogger.Instance.LogOnConsole("MystPanel is removed.", $"MystPanel-{Name}", "OnUnloaded");

        Destruct();
        Dispose();
    }
    #endregion

}
