using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.WorldVisualizer.Controls;

public partial class VisualizerItem : UserControl
{

    #region Constructor
    public VisualizerItem(IReadableModel model, string groupType)
    {
        InitializeComponent();

        Model = model;
        Label = model.ToString();
        GroupType = groupType;
    }
    #endregion

    #region (Private) Fields
    private IReadableModel _model;
    private string _label;
    private string _groupType;
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }
    #endregion

    #region Properties
    public IReadableModel Model
    {
        get => _model;
        set
        {
            _model = value;
        }
    }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            TB_Label.Text = value;
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

    #region Methods: Actions
    public void ShowContextMenu()
    {
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MouseRightButtonDown += OnMouseRightButtonDown;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MouseRightButtonDown -= OnMouseRightButtonDown;
    }
    #endregion

    #region Events: Interface
    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Pressed)
        {
        }
    }
    #endregion

}
