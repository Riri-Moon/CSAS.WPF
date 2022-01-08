using System;
using System.Globalization;
using System.Windows.Data;

namespace CSAS.Converters
{
	public class ObjectToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}

			if (value is string input && string.IsNullOrWhiteSpace(input))
			{
				return false;
			}

			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
