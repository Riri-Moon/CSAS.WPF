using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CSAS.Converters
{
    public class IsGreaterToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) > int.Parse(parameter.ToString()))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
