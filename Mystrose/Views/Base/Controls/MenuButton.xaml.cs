using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TextBlock = System.Windows.Controls.TextBlock;

namespace Mystrose.Views.Base.Controls;

public partial class MenuButton : UserControl
{

    #region Constructor
    public MenuButton()
    {
        InitializeComponent();
        DataContext = this;

        BTN_Item.MouseEnter += Button_MouseEnter;
        BTN_Item.MouseLeave += Button_MouseLeave;
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
        }
    }
    #endregion

    #region Events: Interface
    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
        DoubleAnimation widthAnimation = new DoubleAnimation();
        widthAnimation.From = 32;
        widthAnimation.To = FullButtonWidth;
        widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

        DoubleAnimation opacityAnimation = new DoubleAnimation();
        opacityAnimation.From = 0;
        opacityAnimation.To = 1;
        opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(widthAnimation);
        storyboard.Children.Add(opacityAnimation);

        Storyboard.SetTargetName(widthAnimation, "BTN_Item");
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Button.WidthProperty));

        Storyboard.SetTargetName(opacityAnimation, "TB_Caption");
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(TextBlock.OpacityProperty));

        BTN_Item.BeginStoryboard(storyboard);
    }

    private void Button_MouseLeave(object sender, MouseEventArgs e)
    {
        DoubleAnimation widthAnimation = new DoubleAnimation();
        widthAnimation.From = FullButtonWidth;
        widthAnimation.To = 32;
        widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

        DoubleAnimation opacityAnimation = new DoubleAnimation();
        opacityAnimation.From = 1;
        opacityAnimation.To = 0;
        opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(widthAnimation);
        storyboard.Children.Add(opacityAnimation);

        Storyboard.SetTargetName(widthAnimation, "BTN_Item");
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Button.WidthProperty));

        Storyboard.SetTargetName(opacityAnimation, "TB_Caption");
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(TextBlock.OpacityProperty));

        BTN_Item.BeginStoryboard(storyboard);
    }
    #endregion

}
