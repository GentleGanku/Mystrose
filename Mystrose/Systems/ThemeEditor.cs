using System.Windows.Media;

namespace Mystrose.Systems;

public class ThemeEditor
{

    #region Constructor
    public ThemeEditor()
    {
        HexConverter = new BrushConverter();
    }
    #endregion

    #region Destructor
    ~ThemeEditor()
    {
        HexConverter = null;
    }
    #endregion

    #region Properties
    protected internal BrushConverter HexConverter
    {
        get;
        private set;
    }
    #endregion

    #region Methods - Getter
    public SolidColorBrush GetBrush(string key)
    {
        return (SolidColorBrush)App.Current.Resources[key];
    }

    public Color GetColor(string key)
    {
        return (Color)App.Current.Resources[key];
    }
    #endregion

    #region Methods - Setter
    public void SetBrush(string key, string hexCode)
    {
        App.Current.Resources[key] = (SolidColorBrush)HexConverter.ConvertFromString(hexCode);
    }

    public void SetColor(string key, string hexCode)
    {
        App.Current.Resources[key] = (Color)HexConverter.ConvertFromString(hexCode);
    }
    #endregion

}
