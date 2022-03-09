using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.ValueConverters
{
    public enum VisibleWhen
    {
        True,
        False
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VisibleWhen visibleWhen = parameter != null
                ? Enum.Parse<VisibleWhen>(parameter.ToString())
                : VisibleWhen.True;


            bool boolValue = bool.Parse(value.ToString());

            if (boolValue)
                return visibleWhen == VisibleWhen.True
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            else
                return visibleWhen == VisibleWhen.True
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
