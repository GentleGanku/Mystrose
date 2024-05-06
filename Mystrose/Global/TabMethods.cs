using MahApps.Metro.IconPacks;
using Mystrose.Controls.Main;
using Mystrose.Systems;
using Mystrose.Utilities.Enumerations;
using System.Windows;
using Wpf.Ui.Controls;

namespace Mystrose.Global;

public static class TabMethods
{

    #region Boolean - Tab Group
    public static bool IsTabGroupsFull()
    {
        return MainWindow.Instance.TitleBar.TabGroups.Children.Count >= GetTabGroupsMax();
    }

    public static bool IsTabGroupNew()
    {
        return (MainWindow.Instance.TitleBar.TabGroups.Children.Count == 0) || (IsGameTabsFull());
    }
    #endregion

    #region Boolean - Tab Item
    public static bool IsGameTabsFull()
    {
        return MainWindow.Instance.TitleBar.GameTabs.Children.Count >= GetGameTabsMax();
    }
    #endregion

    #region Methods - Maximum
    public static int GetTabGroupsMax()
    {
        int max = 0;
        switch (ClientMaster.Settings.GroupType)
        {
            case GroupType.Default:
                max = 4;
                break;
            case GroupType.Minimal:
                max = 6;
                break;
            case GroupType.Intermediate:
                max = 3;
                break;
            case GroupType.Ultra:
                max = 2;
                break;
        }

        return max;
    }

    public static int GetGameTabsMax()
    {
        int max = 0;
        switch (ClientMaster.Settings.GroupType)
        {
            case GroupType.Default:
                max = 6;
                break;
            case GroupType.Minimal:
                max = 4;
                break;
            case GroupType.Intermediate:
                max = 8;
                break;
            case GroupType.Ultra:
                max = 12;
                break;
        }

        return max;
    }
    #endregion


}
