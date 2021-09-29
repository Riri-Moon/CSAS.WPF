using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace CSAS.Converters
{
    public class PathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                return Path.GetFileName((string)value);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
