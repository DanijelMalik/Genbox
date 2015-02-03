using System;
using System.Globalization;
using System.Windows.Data;

namespace Genbox.Presentation.Windows.Converters
{
    public class InverseValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return d * -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return d * -1;
        }
    }
}