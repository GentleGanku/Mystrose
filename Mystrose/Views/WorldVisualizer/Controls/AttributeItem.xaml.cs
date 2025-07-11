using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.WorldVisualizer.Controls;

public partial class AttributeItem : UserControl
{

    #region Constructor
    public AttributeItem(string label, string value)
    {
        InitializeComponent();
        
        Label = label;
        Value = value;
    }
    #endregion

    #region (Private) Fields
    private string _label;
    #endregion
    
    #region Properties
    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            TB_Label.Text = value;
        }
    }
    
    public string Value
    {
        get => TBX_Content.Text;
        set
        {
            TBX_Content.Text = value;
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