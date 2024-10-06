using System.Globalization;
using FantasyRemoteCopy.UI.Enums;

namespace FantasyRemoteCopy.UI.Converters;

public sealed class SendTypeFileToVisibleConverter:ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SendType sendType)
            return Binding.DoNothing;
        return sendType == SendType.File ? true : false;
    }
}