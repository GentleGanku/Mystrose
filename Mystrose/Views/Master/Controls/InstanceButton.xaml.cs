using UserControl = System.Windows.Controls.UserControl;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Master.Controls;

public partial class InstanceButton : UserControl
{

    #region Constructor
    public InstanceButton()
    {
        InitializeComponent();
        DataContext = this;
    }
    #endregion

    #region (Private) Fields
    private string _nameText;
    #endregion

    #region Fields
    public Button Button
    {
        get => BTN_Item;
    }
    #endregion

    #region Properties
    public string NameText
    {
        get => _nameText;
        set
        {
            _nameText = value;
            TB_Name.Text = value;
        }
    }
    #endregion

    #region Handlers: Events
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;

        switch (button.Name)
        {
            case "BTN_Item":
                if (button.Appearance is ControlAppearance.Transparent)
                {
                    SVCGameManager.Select(NameText);
                    button.Appearance = ControlAppearance.Primary;
                }
                break;
            case "BTN_Remove":
                SVCGameManager.Deactivate(NameText);
                break;
        }
    }
    #endregion

}
