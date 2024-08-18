namespace Mystrose.Modifiers;

/// SOUND VOLUME MODIFIER
public class MSoundVolume
{

    /// <summary>
    /// Gets the current volume
    /// </summary>
    [DllImport("ChangeVolumeWindows")]
    public static extern float GetSystemVolume();
    /// <summary>
    /// sets the current volume
    /// </summary>
    /// <param name="newVolume">The new volume to set</param>
    [DllImport("ChangeVolumeWindows")]
    public static extern void SetSystemVolume(double newVolume);

}
