using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace MyDiary.Desktop.Common
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            return isVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((Visibility)value == Visibility.Visible);
        }
    }
}
