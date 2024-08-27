using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Base.Controls;

public partial class MenuButton : UserControl
{

    #region Constructor
    public MenuButton()
    {
        InitializeComponent();
        DataContext = this;
    }
    #endregion

    #region (Private) Fields
    private object _iconContent;
    private string _captionText;
    #endregion

    #region Fields
    public Button Button
    {
        get => BTN_Item;
    }

    public double FullButtonWidth
    {
        get => 35 + TB_Caption.ActualWidth;
    }
    #endregion

    #region Properties
    public object IconContent
    {
        get => _iconContent;
        set
        {
            _iconContent = value;
            CPST_Icon.Content = value;
        }
    }

    public string CaptionText
    {
        get => _captionText;
        set
        {
            _captionText = value;
            TB_Caption.Text = value;
        }
    }
    #endregion

}
