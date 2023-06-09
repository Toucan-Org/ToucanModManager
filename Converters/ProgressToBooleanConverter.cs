using Avalonia.Data.Converters;
using System;

namespace ToucanUI.Converters
{
    public class ProgressToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int progress)
            {
                if (progress == 0)
                {
                    return true; // button should be enabled if not installed
                }
                if (progress < 100)
                {
                    return false; // button should be disabled if downloading
                }
                return false; // button should be disabled if installed
            }

            return true; // button should be enabled if progress is not an int
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

