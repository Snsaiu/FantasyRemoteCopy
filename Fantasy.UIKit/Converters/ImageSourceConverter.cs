using System.ComponentModel;
using System.Globalization;

namespace Fantasy.UIKit.Converters
{
    public sealed class ImageSourceConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // IMPORTANT! Update ImageSourceDesignTypeConverter.IsValid if making changes here
            string? strValue = value?.ToString();
            return strValue != null
                ? (object)(Uri.TryCreate(strValue, UriKind.Absolute, out Uri uri) && uri.Scheme != "file"
                    ? ImageSource.FromUri(uri)
                    : ImageSource.FromFile(strValue))
                : throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", strValue,
                    typeof(ImageSource)));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            return value is FileImageSource fis
                ? fis.File
                : value is UriImageSource uis ? (object)uis.Uri.ToString() : throw new NotSupportedException();
        }
    }
}