using System;
using System.Windows.Data;

namespace DangerZone.Helper
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => targetType != typeof(bool) ? throw new InvalidOperationException("The target must be a boolean") : !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotSupportedException();

        #endregion
    }
}