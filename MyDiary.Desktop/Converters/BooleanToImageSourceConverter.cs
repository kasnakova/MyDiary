using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace MyDiary.Desktop.Common
{
    public class BooleanToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasPassword = (bool)value;
            return hasPassword ? "Assets/lock.png" : "../Assets/unlock.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "Assets/lock.png";
        }
    }
}
