using System.ComponentModel;
using System.Globalization;
using FantasyRemoteCopy.UI.Resources.Languages;

namespace FantasyRemoteCopy.UI.Language;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    private LocalizationResourceManager()
    {
        AppResources.Culture = new CultureInfo("en-US");
    }

    public static LocalizationResourceManager Instance { get; } = new();

    public object this[string resourceKey] =>
        AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? Array.Empty<byte>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetCulture(CultureInfo culture)
    {
        AppResources.Culture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}