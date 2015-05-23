using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyDiary.Mobile.Common
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime dt = (DateTime)value;
                //Get the timespan from subtracting the date from the original DateTime
                //this returns a timespan representing the time component of the DateTime
                TimeSpan ts = dt - dt.Date;
                return ts;
            }
            catch (Exception ex)
            {
                return TimeSpan.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                TimeSpan timeSpan = (TimeSpan)value;
                DateTime date = DateTime.MinValue + timeSpan;
                return date;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
    }

}
