using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Base.Controls;

public partial class TitleBar : UserControl
{

    #region Constructor
    public TitleBar()
    {
        InitializeComponent();
    }
    #endregion

    #region (Private) Fields
    private SymbolRegular _titleIcon;
    private string _titleText;
    private object _additionalContent;
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }
    #endregion

    #region Properties
    public SymbolRegular TitleIcon
    {
        get => _titleIcon;
        set
        {
            _titleIcon = value;
            SI_Title.Symbol = value;
        }
    }

    public string TitleText
    {
        get => _titleText;
        set
        {
            _titleText = value;
            TTLB_Item.Title = value;
        }
    }

    public object AdditionalContent
    {
        get => _additionalContent;
        set
        {
            _additionalContent = value;
            TTLB_Item.Header = value;
        }
    }
    #endregion

}
