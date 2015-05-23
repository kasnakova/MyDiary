using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace MyDiary.Mobile.Common
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVisible = (bool)value;
            return isVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !((Visibility)value == Visibility.Visible);
        }
    }
}
