using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfAppDemo.Common
{
    /// <summary>
    /// 资源键转字符串转换器，尝试从应用程序资源中查找对应的本地化字符串
    /// </summary>
    public class ResourceStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                return Application.Current.TryFindResource(key) ?? key;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}