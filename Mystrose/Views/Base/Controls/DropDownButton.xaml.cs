using UserControl = System.Windows.Controls.UserControl;
using WpfDropDownButton = Wpf.Ui.Controls.DropDownButton;

namespace Mystrose.Views.Base.Controls;

public partial class DropDownButton : UserControl
{

    #region Constructor
    public DropDownButton()
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
    public WpfDropDownButton Button
    {
        get => BTN_Item;
    }

    public double FullButtonWidth
    {
        get => 32 + (!string.IsNullOrEmpty(CaptionText) ? (3 + TB_Caption.ActualWidth) : 0);
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

    public object Flyout
    {
        get => BTN_Item.Flyout!;
        set => BTN_Item.Flyout = value;
    }
    #endregion

}
