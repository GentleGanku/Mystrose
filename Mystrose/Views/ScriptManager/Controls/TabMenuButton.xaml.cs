using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Application = System.Windows.Application;

namespace Mystrose.Views.ScriptManager.Controls;

public partial class TabMenuButton : UserControl
{

    #region Constructor
    public TabMenuButton()
    {
        InitializeComponent();
        DataContext = this;

        Loaded += OnLoaded;
        BTN_Item.MouseEnter += Button_MouseEnter;
        BTN_Item.MouseLeave += Button_MouseLeave;
    }
    #endregion

    #region (Private) Fields
    private object _iconContent;
    private string _captionText;
    private bool _isSelected;
    #endregion

    #region Fields
    public Button Button
    {
        get => BTN_Item;
    }
    
    public double FullButtonWidth
    {
        get => 45 + (!string.IsNullOrEmpty(CaptionText) ? TB_Caption.ActualWidth : 0);
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
    
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;

            if (value)
            {
                DrawSelectedState();
            }
            else
            {
                DrawUnselectedState();
            }
        }
    }
    #endregion

    #region Events: Interface
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        BTN_Item.Width = FullButtonWidth;
    }

    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
        if (IsSelected)
        {
            return;
        }

        DrawHoveredState();
    }
    
    private void Button_MouseLeave(object sender, MouseEventArgs e)
    {
        if (IsSelected)
        {
            return;
        }

        DrawUnhoveredState();
    }
    #endregion

    #region Methods: Interface
    private void DrawSelectedState()
    {
        CPST_Icon.Opacity = 1;
        TB_Caption.Opacity = 1;

        Button.Appearance = ControlAppearance.Primary;
    }
    
    private void DrawUnselectedState()
    {
        CPST_Icon.Opacity = 0.75;
        TB_Caption.Opacity = 0.75;

        Button.Appearance = ControlAppearance.Transparent;
    }

    private void DrawHoveredState()
    {
        CPST_Icon.Opacity = 0.75;
        TB_Caption.Opacity = 0.75;
    }

    private void DrawUnhoveredState()
    {
        CPST_Icon.Opacity = 0.75;
        TB_Caption.Opacity = 0.75;
    }
    #endregion

}
