using FantasyRemoteCopy.UI.Models;

using System.Globalization;

namespace FantasyRemoteCopy.UI.Converters;

public class WorkStateToBooleanConverter : ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is WorkState state ? state == WorkState.None ? true : (object)false : Binding.DoNothing;
}