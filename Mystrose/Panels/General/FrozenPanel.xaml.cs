using System.Windows.Controls;

namespace Mystrose.Panels.MainWindow
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
