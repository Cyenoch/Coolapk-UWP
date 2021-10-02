using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace Coolapk.WinUI.Converters
{
    /// <summary>
    /// 如果不是空或null则返回True
    /// </summary>
    public class TrueIfNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                switch (value)
                {
                    case string v:
                        return v.Length == 0 ? false : (object)true;
                    case ICollection<object> v:
                        return v.Count == 0 ? false : (object)true;
                    case bool v:
                        return v;
                    default:
                        return true;
                }
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 取反
    /// </summary>
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return true;
            }

            switch (value)
            {
                case bool v: // 取反
                    return !v;
                case string v: // 如果字符串是空的则返回true
                    return v.Equals(string.Empty);
                case int v: // 如果==0返回true
                    return v == 0;
                default: // 
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Visibile
    /// </summary>
    public class VisibileIfTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
            switch (value)
            {
                case string v: // 不是空字符串则显示
                    return v.Equals(string.Empty) ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
                case ICollection<object> v: // 不是空集则显示
                    return v.Count == 0 ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
                case bool v: // bool to visibile
                    return !v ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
                default:
                    return Windows.UI.Xaml.Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
