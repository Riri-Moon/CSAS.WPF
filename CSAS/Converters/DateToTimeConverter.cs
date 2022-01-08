using System;
using System.Globalization;
using System.Windows.Data;

namespace CSAS.Converters
{
	public class DateToTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var date = (DateTime)value;
			return date.TimeOfDay.ToString().Remove(5);

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
