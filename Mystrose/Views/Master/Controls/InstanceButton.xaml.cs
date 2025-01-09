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
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }

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

    #region Methods
    private void Deactivate()
    {
        ParentWindow.ShowActionMessageBox($"Deactivating {NameText}",
            $"Are you sure you want to deactivate {NameText}? This will stop any related processes for it.",
            "Yes",
            "No",
            () =>
            {
                MSVCGame.Instance.Deactivate(NameText);
            },
            () =>
            {
                return;
            },
            "Cancel",
            () =>
            {
                return;
            });
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
                    MSVCGame.Instance.Select(NameText);
                    button.Appearance = ControlAppearance.Primary;
                }
                break;
            case "BTN_Remove":
                Deactivate();
                break;
        }
    }
    #endregion

}
