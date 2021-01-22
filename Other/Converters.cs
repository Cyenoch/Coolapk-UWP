using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Coolapk_UWP.Other {
    /// <summary>
    /// 如果不是空则返回True
    /// </summary>
    public class IsNotEmptyToTrueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null)
                switch (value) {
                    case string v:
                        if (v.Length == 0) return false;
                        else return true;
                    case ICollection<object> v:
                        if (v.Count == 0) return false;
                        else return true;
                    default:
                        return true;
                } else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果为空则返回True
    /// </summary>
    public class IsEmptyToTrueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value == null)
                switch (value) {
                    case string v:
                        if (v.Length == 0) return true;
                        else return false;
                    case ICollection<object> v:
                        if (v.Count == 0) return true;
                        else return false;
                    default:
                        return false;
                } else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果为False or Null则显示
    /// </summary>
    public class IsFalseOrNullToVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value == null) return Visibility.Visible;
            if (value is bool && ((bool)value) == false) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果为False则显示
    /// </summary>
    public class IsFalseToVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null && value is bool && ((bool)value) == false) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果为True则显示
    /// </summary>
    public class IsTrueToVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null && value is bool && ((bool)value) == true) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果不为空则显示
    /// </summary>
    public class IsNotEmptyToVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null)
                switch (value) {
                    case string v:
                        if (v.Length == 0) return Visibility.Collapsed;
                        else return Visibility.Visible;
                    case ICollection<object> v:
                        if (v.Count == 0) return Visibility.Collapsed;
                        else return Visibility.Visible;
                    default:
                        return Visibility.Visible;
                } else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 如果为空则显示
    /// </summary>
    public class IsEmptyToVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null)
                switch (value) {
                    case string v:
                        if (v.Length == 0) return Visibility.Visible;
                        else return Visibility.Collapsed;
                    case ICollection<object> v:
                        if (v.Count == 0) return Visibility.Visible;
                        else return Visibility.Collapsed;
                    default:
                        return Visibility.Collapsed;
                } else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
