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
                if (parameter is string iconName)
                {
                    return GetIconVisibility(progress, iconName);
                }
                else
                {
                    if (progress == 0)
                    {
                        return "Not Installed";
                    }
                    if (progress < 100)
                    {
                        return "Installing...";
                    }
                    return "Installed";
                }
            }

            return "Not Installed";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //This function then tells the UI what Icon should currently be visible
        private bool GetIconVisibility(int progress, string iconName)
        {
            switch (iconName)
            {
                case "Download":
                    return progress == 0;
                case "Downloading":
                    return progress > 0 && progress < 100;
                case "Downloaded":
                    return progress >= 100;

                default:
                    return false;
            }
        }

    }
}
