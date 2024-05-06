using NAudio.Wave;
using System.IO;
using System.Threading.Tasks;
using Mystrose.Modifiers;

namespace Mystrose.Systems;

public class SoundController
{

    #region Constructor
    public SoundController()
    {
        MusicReader = null;
        OutputDevice = new WaveOutEvent
        {
            Volume = 0.5f
        };
        OutputDevice.PlaybackStopped += OnOutputDeviceStopped;
    }
    #endregion

    #region Destructor
    ~SoundController()
    {
        MusicReader.Dispose();
        OutputDevice.Dispose();
    }
    #endregion

    #region Properties
    protected internal WaveFileReader MusicReader
    {
        get;
        private set;
    }

    protected internal WaveOutEvent OutputDevice
    {
        get;
        private set;
    }
    #endregion

    #region Methods
    public async void SwitchMusic(int code)
    {
        if (MusicReader != null)
        {
            OutputDevice.Stop();
            await Task.Delay(1000);
        }

        switch (code)
        {
            case 0:
                MusicReader = new WaveFileReader(Properties.Resources.BattleHaven);
                break;
        }

        PlayMusic();
    }

    public async void PlayMusic()
    {
        OutputDevice.Init(MusicReader);
    }
    #endregion

    #region Events
    private void OnOutputDeviceStopped(object sender, StoppedEventArgs e)
    {
        MusicReader.Position = 1;
        OutputDevice.Play();
    }
    #endregion

}
