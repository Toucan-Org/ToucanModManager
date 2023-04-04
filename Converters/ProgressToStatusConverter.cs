using Avalonia.Data.Converters;
using System;
using System.Globalization;

// This class is used to convert the progress of a mod download to a status string for the ProgressBar

namespace ToucanUI.Converters
{
    public class ProgressToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int progress)
            {
                if (progress == 0)
                {
                    return "Status: Not Installed";
                }
                if (progress < 100)
                {
                    return "Status: Installing...";
                }
                return "Status: Installed";
            }

            return "Status: Not Installed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
