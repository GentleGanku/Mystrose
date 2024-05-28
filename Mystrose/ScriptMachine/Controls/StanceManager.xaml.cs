using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Contracts;
using WpfControls = Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace Mystrose.ScriptMachine.Controls;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class StanceManager : UserControl
{

    public StanceManager()
    {
        InitializeComponent();
        DataContext = this;
        ContentDialogService = new ContentDialogService();
    }

    private IContentDialogService ContentDialogService
    {
        get;
        set;
    }

    private async void Prompt_Click(object sender, RoutedEventArgs e)
    {
        WpfControls.ContentDialogResult result = await ContentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
        {
            Title = "Manage the Stances",
            Content = ((WpfControls.Button)sender).CommandParameter,
            PrimaryButtonText = "Go",
            CloseButtonText = "Cancel",
        });
    }

}
