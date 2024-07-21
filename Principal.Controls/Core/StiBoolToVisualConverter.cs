using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Principal.Controls.Core
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class StiBoolToVisualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value) ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
