using System;
using Button = Wpf.Ui.Controls.Button;
using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.ScriptManager.Features;

public partial class ScriptTabMenu : UserControl
{

    #region Constructors
    public ScriptTabMenu()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    #endregion

    #region Private Fields
    private TabMenuButton? _selectedTab;
    #endregion
    
    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }

    public List<TabMenuButton> Tabs
    {
        get => [.. SPNL_Tabs.Children.OfType<TabMenuButton>()];
    }
    #endregion
    
    #region Properties
    public TabMenuButton? SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab is not null)
            {
                _selectedTab.IsSelected = false;
            }
            
            _selectedTab = value;
            _selectedTab!.IsSelected = true;
        }
    }

    public Action<int>? SelectionAction
    {
        get;
        set;
    } = null;
    #endregion

    #region Methods: Actions
    public void SelectTab(int index)
    {
        if (Tabs.Count <= 0)
        {
            HSVCLogger.Instance.LogOnTrace($"No tabs currently exist.");
            return;
        }

        Response<Action> response = ParentWindow.Invoke(() =>
        {
            if (index < 0 || index >= Tabs.Count)
            {
                return;
            }

            SelectedTab = Tabs[index];
        });
    }
    #endregion

    #region Events: Interface
    private void TabButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        int tabIndex = Tabs.IndexOf((TabMenuButton)btn.Parent);
        
        SelectTab(tabIndex);
        SelectionAction?.Invoke(tabIndex);
    }
    #endregion
    
    #region Handlers: Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        foreach (var tab in Tabs)
        {
            tab.Button.Click += TabButton_Click;
        }

        SelectedTab = Tabs[0];
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        foreach (var tab in Tabs)
        {
            tab.Button.Click -= TabButton_Click;
        }

        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }
    #endregion
    
}