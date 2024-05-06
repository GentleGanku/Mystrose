using MahApps.Metro.IconPacks;
using Mystrose.Global;
using Mystrose.Systems;
using Mystrose.Panels.MainWindow;
using Mystrose.Utilities.Enumerations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

using UserControl = System.Windows.Controls.UserControl;
using ColumnDefinition = System.Windows.Controls.ColumnDefinition;
using Grid = System.Windows.Controls.Grid;

namespace Mystrose.Controls.Main;

public partial class GameTabBar : UserControl
{

    #region Contructor
    public GameTabBar()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }
    #endregion

    #region Properties
    protected internal MainWindow Window
    {
        get;
        set;
    }

    protected internal TabItem SelectedItem
    {
        get;
        set;
    }

    protected internal TabGroup SelectedGroup
    {
        get;
        set;
    }
    #endregion

    #region Methods: Controller
    private void ApplySettings()
    {
        SkipHomeSetting();
    }

    public void AddNewTab()
    {
        if (TabMethods.IsTabGroupsFull() && TabMethods.IsGameTabsFull())
        {
            return;
        }

        if (TabMethods.IsTabGroupNew())
        {
            TabGroup tabGroup = CreateGameTabGroup(TabGroups.Children.Count);
            Window.NavigationBar.NotifsFlyoutContent.AddGroup(TabGroups.Children.Count);

            TabGroups.Children.Add(tabGroup);

            OnTabGroupClick(tabGroup, null);
        }

        TabItem item = CreateTabItem(GameTabs.Children.Count);

        Client client = new();
        ClientMaster.Clients.Add(item, client);
        ClientMaster.Profiles.Add(client.Profile);

        client.Profile.InstanceTab = item;

        GameTabs.Children.Add(client.Profile.InstanceTab);
        SelectedGroup.Tabs.Add(client.Profile.InstanceTab);

        client.GameHost.GroupIndex = TabGroups.Children.IndexOf(SelectedGroup);

        client.Profile.InstanceTab.ContentPanel.Content = client.GameHost;

        OnTabItemClick(client.Profile.InstanceTab, null);
    }

    public async void AddAllNewTabs()
    {
        if (TabMethods.IsGameTabsFull())
        {
            return;
        }

        int emptyTabCount = TabMethods.GetGameTabsMax() - GameTabs.Children.Count;
        for (int i = 0; i < emptyTabCount; i++)
        {
            AddNewTab();
            await Task.Delay(10);
        }
    }

    public void SwitchWindowMode(GameWindowType mode)
    {
        //switch (mode)
        //{
        //    case GameWindowType.Single:
        //        Window.GameWindowMode = GameWindowType.Multi;
        //        ModeMenu.Header = "Single-Game Mode";

        //        Window.ContentPanel.Children.Clear();

        //        Grid grid = new Grid();

        //        for (int i = 0; i < 3; i++)
        //        {
        //            grid.ColumnDefinitions.Add(new ColumnDefinition()
        //            {
        //                Width = new GridLength(1, GridUnitType.Star),
        //                MinWidth = 320,
        //                MaxWidth = 900
        //            });
        //        }
        //        for (int i = 0; i < 2; i++)
        //        {
        //            grid.RowDefinitions.Add(new RowDefinition()
        //            {
        //                Height = new GridLength(1, GridUnitType.Star),
        //                MinHeight = 183,
        //                MaxHeight = 600
        //            });
        //        }

        //        int emptyRow = 0;
        //        int emptyColumn = 0;

        //        for (int i1 = 0; i1 < TabGroups.Children.Count; i1++)
        //        {
        //            TabGroup tabGroup = TabGroups.Children[i1] as TabGroup;
        //            for (int i2 = 0; i2 < tabGroup.Tabs.Count; i2++)
        //            {
        //                TabItem tabItem = tabGroup.Tabs[i2];
        //                GameHost host = tabItem.ContentPanel.Content as GameHost;

        //                //host.Margin = new Thickness(15, 45, 15, 45);

        //                Grid.SetRow(host, emptyRow);
        //                Grid.SetColumn(host, emptyColumn);

        //                emptyColumn++;
        //                if (emptyColumn >= 3)
        //                {
        //                    emptyColumn = 0;
        //                    emptyRow++;
        //                }

        //                grid.Children.Add(host);
        //            }
        //        }

        //        Window.ContentPanel.Children.Add(grid);
        //        break;
        //    case GameWindowType.Multi:
        //        Window.GameWindowMode = GameWindowType.Single;
        //        ModeMenu.Header = "Multi-Game Mode";

        //        Window.ContentPanel.Children.Clear();

        //        for (int i1 = 0; i1 < TabGroups.Children.Count; i1++)
        //        {
        //            TabGroup tabGroup = TabGroups.Children[i1] as TabGroup;
        //            for (int i2 = 0; i2 < tabGroup.Tabs.Count; i2++)
        //            {
        //                TabItem tabItem = tabGroup.Tabs[i2];
        //                GameHost host = tabItem.ContentPanel.Content as GameHost;

        //                host.Margin = new Thickness(0);
        //                host.Padding = new Thickness(0);

        //                Grid.SetRow(host, 0);
        //                Grid.SetColumn(host, 0);

        //                (host.Parent as Grid).Children.Remove(host);
        //            }
        //        }

        //        Window.ContentPanel.Children.Add(SelectedItem.ContentPanel.Content as GameHost);
        //        break;
        //}
    }

    public void MinimizeWindow()
    {
        Window.WindowState = WindowState.Minimized;
    }

    public void MaximizeRestoreWindow()
    {
        Window.WindowState = Window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    public void CloseWindow()
    {
        Window.Close();
    }
    #endregion

    #region Methods: Setter
    private void InitializeStartup()
    {
        SelectedItem = new TabItem();
        SelectedGroup = new TabGroup();

        HomeBtn.ContentPanel.Content = new HomePanel();
        (HomeBtn.ContentPanel.Content as HomePanel).Parent = this;

        HomeBtn.ClickEvent += OnTabItemClick;

        AddTabBtn.Click += OnAddTabClick;

        TitleBar.MouseWheel += OnTitleBarMouseWheel;
        TitleBar.MouseRightButtonDown += OnTitleBarMouseRight;

        OnTabItemClick(HomeBtn, null);
    }

    private void SetColumnDefs()
    {
        GameTabs.ColumnDefinitions.Clear();
        for (int i = 0; i < TabMethods.GetGameTabsMax(); i++) 
        {
            GameTabs.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }

        TabGroups.ColumnDefinitions.Clear();
        for (int i = 0; i < TabMethods.GetTabGroupsMax(); i++)
        {
            TabGroups.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }
    }
    #endregion

    #region Methods: Setting
    private void SkipHomeSetting()
    {
        if (ClientMaster.Settings.IsHomeSkip)
        {
            AddNewTab();
        }
    }
    #endregion

    #region Methods: Tab Item
    private TabItem CreateTabItem(int index, RoleType role = RoleType.Unknown)
    {
        TabItem tabItem = new()
        {
            Name = "Tab" + index,
            Height = 26,
            Margin = new Thickness(1.5, 1, 1.5, 0),
            Padding = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            IconControlContent = GetRoleIcon(role),
            Content = new TextBlock()
            {
                Text = "New Tab",
                Height = 16,
                FontSize = 11,
                TextTrimming = TextTrimming.CharacterEllipsis,
            },
            MiscControlContent = new MenuButton()
            {
                Width = 16,
                Height = 16,
                Padding = new Thickness(0),
                IconControlContent = new PackIconEvaIcons()
                {
                    Width = 8,
                    Height = 8,
                    Kind = PackIconEvaIconsKind.ArrowIosDownward
                }
            }
        };

        tabItem.ClickEvent += OnTabItemClick;

        (tabItem.MiscControlContent as MenuButton).Menu = new();
        (tabItem.MiscControlContent as MenuButton)?.Menu.Items.Add(new MenuItem()
        {
            Name = "CloseTabMenu",
            Header = "Close tab",
            Tag = index
        });
        (tabItem.MiscControlContent as MenuButton)?.Menu.Items.Add(new MenuItem()
        {
            Name = "CloseTabsMenu",
            Header = "Close other tabs",
            Tag = index
        });

        ((tabItem.MiscControlContent as MenuButton)?.Menu.Items[0] as MenuItem).Click += OnMenuItemClick;
        ((tabItem.MiscControlContent as MenuButton)?.Menu.Items[1] as MenuItem).Click += OnMenuItemClick;

        tabItem.SetValue(Grid.ColumnProperty, index);

        return tabItem;
    }

    private object GetRoleIcon(RoleType role)
    {
        object icon;
        switch (role)
        {
            case RoleType.Shield:
                icon = new PackIconMaterial()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconMaterialKind.ShieldSword
                };
                break;
            case RoleType.Support:
                icon = new PackIconBootstrapIcons()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconBootstrapIconsKind.Stars
                };
                break;
            case RoleType.Warrior:
                icon = new PackIconRPGAwesome()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconRPGAwesomeKind.SpinningSword
                };
                break;
            case RoleType.Mage:
                return new PackIconSimpleIcons()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconSimpleIconsKind.Codemagic
                };
            case RoleType.Rogue:
                icon = new PackIconRPGAwesome()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconRPGAwesomeKind.DrippingKnife
                };
                break;
            case RoleType.Healer:
                icon = new PackIconFontAwesome()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconFontAwesomeKind.HandSparklesSolid
                };
                break;
            default:
                icon = new PackIconRPGAwesome()
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(1, 0, 0, 0),
                    Kind = PackIconRPGAwesomeKind.Helmet
                };
                break;
        }
        return icon;
    }

    private void CloseTab(object sender, RoutedEventArgs e)
    {
        int index = int.Parse((sender as MenuItem).Tag.ToString());

        TabItem? tabItem = GameTabs.Children[index] as TabItem;
        Profile profile = ProfileMethods.GetProfile(tabItem);

        GameTabs.Children.Remove(tabItem);
        SelectedGroup.Tabs.Remove(tabItem);
        tabItem = null;

        ClientMaster.Profiles.Remove(profile);
        profile = null;

        for (int tabIndex = 0; tabIndex < GameTabs.Children.Count; tabIndex++)
        {
            TabItem? gameTab = GameTabs.Children[tabIndex] as TabItem;
            gameTab.Name = "Tab" + tabIndex;
            gameTab.SetValue(Grid.ColumnProperty, tabIndex);

            for (int menuIndex = 0; menuIndex < (gameTab.MiscControlContent as MenuButton)?.Menu.Items.Count; menuIndex++)
            {
                MenuItem? menuItem = (gameTab.MiscControlContent as MenuButton)?.Menu.Items[menuIndex] as MenuItem;
                menuItem.Tag = tabIndex;
            }
        }

        if (SelectedGroup.Tabs.Count > 0)
        {
            OnTabItemClick(GameTabs.Children[0], null);
        }
        else
        {
            TabGroups.Children.RemoveAt(int.Parse(SelectedGroup.Tag.ToString()));

            for (int i = 0; i < TabGroups.Children.Count; i++)
            {
                TabGroup tabGroup = TabGroups.Children[i] as TabGroup;
                tabGroup.Name = "TabGroup" + i;
                tabGroup.Tag = i;
                tabGroup.SetValue(Grid.ColumnProperty, i);
            }

            if (TabGroups.Children.Count > 0)
            {
                OnTabGroupClick(TabGroups.Children[0], null);
            }
            else
            {
                OnTabItemClick(HomeBtn, null);
            }
        }
    }

    private void CloseOtherTabs(object sender, RoutedEventArgs e)
    {
        TabItem targetedTabItem = GameTabs.Children[int.Parse((sender as MenuItem).Tag.ToString())] as TabItem;

        int i = 0;
        while (i < GameTabs.Children.Count)
        {
            if (GameTabs.Children[i] != targetedTabItem)
            {
                CloseTab(new MenuItem()
                {
                    Tag = i
                }, null);
            }
            else
            {
                i++;
            }
        }
    }
    #endregion

    #region Methods: Tab Group
    private TabGroup CreateGameTabGroup(int index)
    {
        TabGroup tabGroup = new()
        {
            Name = "TabGroup" + index,
            Height = 5,
            Margin = new Thickness(1, 0, 1, 0),
            Padding = new Thickness(0),
            CornerRadius = new CornerRadius(2.5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Tag = index
        };

        tabGroup.ClickEvent += OnTabGroupClick;

        tabGroup.SetValue(Grid.ColumnProperty, index);

        return tabGroup;
    }
    #endregion

    #region Events: Controller
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        InitializeStartup();
        SetColumnDefs();
        ApplySettings();
    }

    private void OnTitleBarMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (TabGroups.Children.Count <= 1)
        {
            return;
        }

        int currentIndex = TabGroups.Children.IndexOf(SelectedGroup);
        int newIndex = e.Delta > 0 ? (currentIndex + 1) : (currentIndex - 1);
        if (newIndex >= TabGroups.Children.Count)
        {
            newIndex = 0;
        }
        else if (newIndex < 0)
        {
            newIndex = TabGroups.Children.Count - 1;
        }

        OnTabGroupClick(TabGroups.Children[newIndex], null);
    }

    private void OnTitleBarMouseRight(object sender, RoutedEventArgs e)
    {
        TitleBar.ContextMenu.IsOpen = true;
    }

    private void OnAddTabClick(object sender, RoutedEventArgs e)
    {
        AddNewTab();
    }
    #endregion

    #region Events: Tab Item
    private void OnTabItemClick(object sender, RoutedEventArgs e)
    {
        var tabItem = sender as TabItem;

        SelectedItem.IsSelected = false;
        SelectedItem = tabItem;
        SelectedItem.IsSelected = true;

        if (tabItem.ContentPanel.Content != null)
        {
            if (Window.ContentPanel.Children.Count > 0)
            {
                Window.ContentPanel.Children.RemoveRange(0, Window.ContentPanel.Children.Count);
            }

            Window.NavigationBar.Visibility = Visibility.Visible;
            if (tabItem.ContentPanel.Content is not GameHost)
            {
                Window.NavigationBar.Visibility = Visibility.Collapsed;
            }
            else if (((GameHost)tabItem.ContentPanel.Content).Buffer != null)
            {
                Window.ContentPanel.Children.Add(((GameHost)tabItem.ContentPanel.Content).Buffer);
            }

            Window.ContentPanel.Children.Insert(0, (UIElement)tabItem.ContentPanel.Content);
        }
    }
    #endregion

    #region Events: Tab Group
    private void OnTabGroupClick(object sender, RoutedEventArgs e)
    {
        var tabGroup = sender as TabGroup;

        SelectedGroup.IsSelected = false;
        SelectedGroup = tabGroup;
        SelectedGroup.IsSelected = true;

        Window.NavigationBar.NotifsFlyoutContent.SetGroup(TabGroups.Children.IndexOf(SelectedGroup));

        GameTabs.Children.Clear();
        foreach (TabItem item in tabGroup.Tabs)
        {
            GameTabs.Children.Add(item);
            if (item.IsSelected)
            {
                OnTabItemClick(item, null);
            }
        }
    }
    #endregion

    #region Events: Menu Item
    private void OnMenuItemClick(object sender, RoutedEventArgs e)
    {
        switch (((MenuItem)sender).Name)
        {
            case "NewTabMenu":
                AddNewTab();
                break;
            case "AllTabsMenu":
                AddAllNewTabs();
                break;

            case "ModeMenu":
                SwitchWindowMode(Window.GameWindowMode);
                break;
            case "MinimizeMenu":
                MinimizeWindow();
                break;
            case "MaximizeRestoreMenu":
                MaximizeRestoreWindow();
                break;
            case "CloseMenu":
                CloseWindow();
                break;

            case "CloseTabMenu":
                CloseTab(sender, e);
                break;
            case "CloseTabsMenu":
                CloseOtherTabs(sender, e);
                break;
        }
    }
    #endregion

}
