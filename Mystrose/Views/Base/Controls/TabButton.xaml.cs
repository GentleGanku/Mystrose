using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Base.Controls;

public partial class TabButton : UserControl
{

    #region Constructor
    public TabButton()
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

            if (string.IsNullOrEmpty(value))
            {
                TB_Caption.Opacity = 0.0;
                TB_Caption.Visibility = Visibility.Collapsed;
            }
            else
            {
                TB_Caption.Opacity = 1.0;
                TB_Caption.Visibility = Visibility.Visible;
            }
        }
    }
    #endregion

}
