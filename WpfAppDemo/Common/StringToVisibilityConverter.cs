using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfAppDemo.Common
{
    /// <summary>
    /// 字符串可见性转换器，当字符串为空或 null 时折叠元素
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}