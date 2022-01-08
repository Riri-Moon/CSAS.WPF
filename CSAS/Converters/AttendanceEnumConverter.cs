using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static CSAS.Enums.Enums;

namespace CSAS.Converters
{
	[ValueConversion(typeof(String), typeof(ImageSource))]
	public class AttendanceEnumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((AttendanceEnums)value == AttendanceEnums.IsPresent)
			{
				return "Prítomný";
			}
			else if ((AttendanceEnums)value == AttendanceEnums.NotPresent)
			{
				return "Neprítomný";
			}
			else if ((AttendanceEnums)value == AttendanceEnums.Excused)
			{
				return "Ospravedlnené";
			}
			else return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((string)value == "Prítomný")
			{
				return AttendanceEnums.IsPresent;
			}
			else if ((string)value == "Neprítomný")
			{
				return AttendanceEnums.NotPresent;
			}
			else if ((string)value == "Ospravedlnené")
			{
				return AttendanceEnums.Excused;
			}
			else return AttendanceEnums.New;
		}
	}
}
