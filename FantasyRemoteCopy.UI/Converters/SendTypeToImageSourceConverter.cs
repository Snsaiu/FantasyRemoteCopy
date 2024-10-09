using System.Globalization;
using FantasyRemoteCopy.UI.Enums;

namespace FantasyRemoteCopy.UI.Converters;

public sealed class SendTypeToImageSourceConverter:ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SendType type)
        {
            return Binding.DoNothing;
        }
        
        return type switch
        {
            SendType.Text => ImageSource.FromFile("texticon.png"),
            SendType.File => ImageSource.FromFile("fileicon.png"),
            SendType.Folder=>ImageSource.FromFile("foldericon.png"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}