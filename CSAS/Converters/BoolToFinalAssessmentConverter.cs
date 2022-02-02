using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CSAS.Converters
{
	public class BoolToFinalAssessmentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value || value == null)
			{
				return "Navrhované hodnotenie: ";
			}
			else
			{
				return "Udelené hodnotenie: ";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
