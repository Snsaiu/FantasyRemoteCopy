#region

using Fantasy.UIKit.Primitives;

using System.ComponentModel;
using System.Globalization;

#endregion

namespace Fantasy.UIKit.Converters;

internal class CornerRadiusShapeConverter : TypeConverter
{
    private static readonly string[] values =
    [
        "None",
        "ExtraSmall",
        "ExtraSmallTop",
        "Small",
        "Medium",
        "Large",
        "LargeTop",
        "LargeEnd",
        "ExtraLarge",
        "ExtraLargeTop",
        "Full"
    ];

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text) return null;
        switch (text)
        {
            case "None":
                return CornerRadiusShape.None;
            case "ExtraSmall":
                return CornerRadiusShape.ExtraSmall;
            case "Small":
                return CornerRadiusShape.Small;
            case "Medium":
                return CornerRadiusShape.Medium;
            case "Large":
                return CornerRadiusShape.Large;
            case "LargeTop":
                return CornerRadiusShape.LargeTop;
            case "LargeEnd":
                return CornerRadiusShape.LargeEnd;
            case "ExtraLarge":
                return CornerRadiusShape.ExtraLarge;
            case "ExtraLargeTop":
                return CornerRadiusShape.ExtraLargeTop;
            case "Full":
                return CornerRadiusShape.Full;
            default:
                {
                    string[] arr = text.Split(',');
                    switch (arr.Length)
                    {
                        case 1 when int.TryParse(arr[0], out int i32):
                            return new CornerRadiusShape(i32);
                        case 4 when int.TryParse(arr[0], out int topLeft)
                                    && int.TryParse(arr[1], out int topRight)
                                    && int.TryParse(arr[2], out int bottomLeft)
                                    && int.TryParse(arr[3], out int bottomRight):
                            return new CornerRadiusShape(topLeft, topRight, bottomLeft, bottomRight);
                    }

                    break;
                }
        }

        return null;
    }

    public override StandardValuesCollection? GetStandardValues(ITypeDescriptorContext? context)
    {
        return new(values);
    }
}