using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ToucanUI.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return AvaloniaProperty.UnsetValue;

            var values = ((string)parameter).Split(',');

            foreach (var val in values)
            {
                if (value.ToString() == val.Trim())
                    return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}


