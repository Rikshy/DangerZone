using System;
using System.Windows;
using System.Windows.Data;

namespace DangerZone.Helper
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => targetType != typeof(Visibility) ? throw new InvalidOperationException("The target must be a boolean") : (Visibility)value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotSupportedException();

        #endregion
    }
}