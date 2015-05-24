using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyDiary.Desktop.Common
{
    class BoolToStringColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSuccessful = (bool)value;
            return isSuccessful ? "Green" : "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = value.ToString();
            return color == "Green" ? true : false;
        }
    }
}
