using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.Converters
{
    public class TextToHeaderCharConverter : ValueConverterBase
    {
        public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return Binding.DoNothing;
            return string.IsNullOrEmpty(value.ToString()) ? string.Empty : value.ToString()!.First().ToString().ToUpper();
        }
    }
}