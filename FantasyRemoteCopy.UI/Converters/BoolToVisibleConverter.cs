using System.Globalization;

namespace FantasyRemoteCopy.UI.Converters;

public sealed class BoolToVisibleConverter:ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b)
            return Binding.DoNothing;
        return b? Visibility.Visible : Visibility.Collapsed;
    }
}