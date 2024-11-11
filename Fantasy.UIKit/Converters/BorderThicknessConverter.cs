using System.ComponentModel;
using System.Globalization;

namespace Fantasy.UIKit.Converters;

public class BorderThicknessConverter : TypeConverter
{

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(Thickness);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text) return null;
        string[] split = text.Split(",");

        if (split.Length is < 1 or > 4 or 3)
            throw new InvalidOperationException();

        if (split.Length == 1)
        {

            return int.TryParse(text, out int v) ? (object)new Thickness(v) : throw new FormatException();
        }
        else if (split.Length == 2)
        {
            return int.TryParse(split[0], out int v) && int.TryParse(split[1], out int v2) ? (object)new Thickness(v, v2) : throw new FormatException();
        }
        else
        {
            return split.Length == 4
                ? int.TryParse(split[0], out int v) && int.TryParse(split[1], out int v2) && int.TryParse(split[2], out int v3) && int.TryParse(split[3], out int v4)
                            ? (object)new Thickness(v, v2, v3, v4)
                            : throw new FormatException()
                : throw new InvalidCastException();
        }



    }
}