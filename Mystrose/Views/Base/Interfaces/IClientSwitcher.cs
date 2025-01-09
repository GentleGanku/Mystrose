namespace Mystrose.Views.Base.Interfaces;

public interface IClientSwitcher
{

    #region Properties
    ClientSwitchButton SwitchButton
    {
        get;
    }
    #endregion

    #region Methods
    void SwitchButton_SelectionChanged(object sender, SelectionChangedEventArgs e);
    #endregion

}
