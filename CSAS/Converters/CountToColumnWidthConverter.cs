using System;
using System.Globalization;
using System.Windows.Data;

namespace CSAS.Converters
{
	public class CountToColumnWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Int32)value > 0)
			{
				return Double.Parse(parameter.ToString());
			}
			else
			{
				return 0d;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
