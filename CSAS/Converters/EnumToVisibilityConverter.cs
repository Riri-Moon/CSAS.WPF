using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CSAS.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            if (value != null && parameter != null)
            {
                result = value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
            }

            //if (value.GetType().IsEnum)
            //{
            //	var values = value.GetType().GetEnumValues();
            //	if (Array.IndexOf(values, parameter) >= 0)
            //	{
            //		result = Visibility.Visible;
            //	}
            //}

            return result;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
