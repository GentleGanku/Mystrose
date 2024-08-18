using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Panels.General
{
    
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class FrozenPanel : UserControl
    {
        public FrozenPanel()
        {
            InitializeComponent();

            Name = "FrozenPanel";
            SetValue(Grid.RowProperty, 0);
            SetValue(Grid.ColumnProperty, 0);
        }
    }

}
