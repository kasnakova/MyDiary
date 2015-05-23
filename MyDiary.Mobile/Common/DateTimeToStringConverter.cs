using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyDiary.Mobile.Common
{
    public class DateTimeToStringConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime date = (DateTime)value;
            return date.ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.ParseExact((string)value, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
