using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CSAS.Converters
{
    public class InvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value;

            if (value is Visibility.Visible)
            {
                result = Visibility.Collapsed;
            }
            else if (value is Visibility.Collapsed)
            {
                result = Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
