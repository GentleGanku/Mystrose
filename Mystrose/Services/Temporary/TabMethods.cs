namespace Mystrose.Services.Temporary;

public static class TabMethods
{

    #region Boolean - Tab Group
    public static bool IsTabGroupsFull()
    {
        return MainWindow.Instance.TitleBar.TabGroups.Children.Count >= GetTabGroupsMax();
    }

    public static bool IsTabGroupNew()
    {
        return MainWindow.Instance.TitleBar.TabGroups.Children.Count == 0 || IsGameTabsFull();
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
        return 4;
    }

    public static int GetGameTabsMax()
    {
        return 6;
    }
    #endregion


}
