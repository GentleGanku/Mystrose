using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.WorldVisualizer.Controls;

public partial class VisualizerItem : UserControl
{
    
    #region Constructor
    public VisualizerItem(IReadableModel? readableModel, string groupType)
    {
        InitializeComponent();

        ReadableModel = readableModel;
        Label = readableModel is not null ? readableModel.ToString() : "No data available.";
        GroupType = groupType;
    }
    #endregion

    #region (Private) Fields
    private IReadableModel? _readableModel;
    private string _label;
    private string _context;
    private string _groupType;
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }

    #endregion

    #region Properties
    public IReadableModel? ReadableModel
    {
        get => _readableModel;
        set { _readableModel = value; }
    }

    public string Label
    {
        get => _label;
        set
        {
            string[] modelString = value.Split(" | ");
            
            _label = modelString[0];
            TB_Label.Text = _label;
            TB_Label.Opacity = _label == "No data available." ? 0.5 : 1;

            Context = modelString.Length > 1 ? value.Split(" | ")[1] : "";
        }
    }

    public string Context
    {
        get => _context;
        set
        {
            _context = value;
            TB_Context.Text = _context;
            TB_Context.Visibility = string.IsNullOrEmpty(_context) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
    
    public string GroupType
    {
        get => _groupType;
        set
        {
            _groupType = value;
            TB_Label.Tag = value;
        }
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
    }
    #endregion
    
}