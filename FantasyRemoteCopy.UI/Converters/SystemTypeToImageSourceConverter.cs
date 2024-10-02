using System.Globalization;
using FantasyRemoteCopy.UI.Enums;

namespace FantasyRemoteCopy.UI.Converters;

public class SystemTypeToImageSourceConverter:ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SystemType type)
        {
            return Binding.DoNothing;
        }
        
        return type switch
        {
            SystemType.None => null,
            SystemType.Windows => ImageSource.FromFile("windows.png"),
            SystemType.MacOS => ImageSource.FromFile("mac.png"),
            SystemType.IOS => ImageSource.FromFile("iphone.png"),
            SystemType.Android => ImageSource.FromFile("android.png"),
            SystemType.Linux => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}