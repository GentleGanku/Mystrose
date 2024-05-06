using Mystrose.Systems;
using Wpf.Ui.Controls;

namespace Mystrose.Global;

public static class ProfileMethods
{

    #region Methods - Getter
    public static Profile GetProfile(TabItem tabItem)
    {
        Profile profile = null;

        foreach (Profile clientProfile in ClientMaster.Profiles)
        {
            if (clientProfile.InstanceTab == tabItem)
            {
                profile = clientProfile;
                break;
            }
        }

        return profile;
    }
    #endregion

}
