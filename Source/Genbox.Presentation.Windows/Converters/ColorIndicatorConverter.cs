using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Generator4Developers.Presentation.Windows.Converters
{
    [ValueConversion(typeof(double), typeof(Brush))]
    public class ColorIndicatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percentage = (double)(value);
            var colors = new[] { Colors.Crimson, Colors.DarkOrange, Color.FromRgb(217, 170, 18), Color.FromRgb(130, 180, 26), Color.FromRgb(27, 158, 27) };
            var index = (int)Math.Min(colors.Length - 1, percentage / 0.2);

            return new SolidColorBrush(colors[index]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}