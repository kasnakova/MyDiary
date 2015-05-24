using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyDiary.Desktop.Common
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.ParseExact((string)value, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
