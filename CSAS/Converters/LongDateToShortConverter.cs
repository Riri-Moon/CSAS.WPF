using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace CSAS.Converters
{
    public class LongDateToShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            culture = new CultureInfo("sk-SK");
            var date = (DateTime)value;
            string format = string.Format(culture.DateTimeFormat.ShortDatePattern, date);
            return date.ToString(format);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
