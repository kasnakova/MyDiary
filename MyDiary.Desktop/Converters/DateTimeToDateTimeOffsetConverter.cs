//using System;
//using System.Globalization;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Data;

//namespace MyDiary.Mobile.Common
//{
//    public class DateTimeToDateTimeOffsetConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, string language)
//        {
//            try
//            {
//                DateTime date = (DateTime)value;
//                return new DateTimeOffset(date);
//            }
//            catch (Exception ex)
//            {
//                return DateTimeOffset.MinValue;
//            }
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, string language)
//        {
//            try
//            {
//                DateTimeOffset dto = (DateTimeOffset)value;
//                return dto.DateTime;
//            }
//            catch (Exception ex)
//            {
//                return DateTime.MinValue;
//            }
//        }
//    }
//}
