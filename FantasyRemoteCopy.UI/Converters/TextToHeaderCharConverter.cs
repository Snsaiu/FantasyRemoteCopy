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
        public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is string s ? s.First().ToString().ToUpper() : "";
    }
}