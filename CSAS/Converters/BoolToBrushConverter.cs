using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CSAS.Converters
{
	public class BoolToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e53935"));
			}
			else
			{
				return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
