using System;
using System.Windows;
using System.Windows.Data;

namespace DangerZone.Helper
{
    public class QuestionActiveConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => targetType != typeof(GridLength) ? throw new InvalidOperationException("The target must be Visibility") : (bool)value ? 0 : 150;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotSupportedException();

        #endregion
    }
}
