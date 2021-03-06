using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace CSAS.Converters
{
	public class MultiValueConverter : MarkupExtension, IMultiValueConverter
	{

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return values.ToArray();
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		private static MultiValueConverter _converter = null;

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (_converter == null) _converter = new MultiValueConverter();
			return _converter;
		}

		public MultiValueConverter()
			: base()
		{
		}
	}
}
