using System;
using System.Globalization;
using System.Windows.Data;

namespace CSAS.Converters
{
	public class InvertedEnumToBooleanConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}

			return !value.Equals(parameter);
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(true) ? parameter : Binding.DoNothing;
		}
	}
}
