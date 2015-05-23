using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace MyDiary.Mobile.Common
{
    public class BooleanToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool hasPassword = (bool)value;
            return hasPassword ? "../Assets/lock.png" : "../Assets/unlock.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (string)value == "../Assets/lock.png";
        }
    }
}
