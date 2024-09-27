using System.Globalization;

namespace FantasyRemoteCopy.UI.Converters;

public abstract class ValueConverterBase : IValueConverter, IMarkupExtension
{
    public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
    public virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
    public object ProvideValue(IServiceProvider serviceProvider) => this;
}