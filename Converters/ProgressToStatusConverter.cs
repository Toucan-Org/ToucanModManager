using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using ToucanUI.Models.KSP2;

namespace ToucanUI.Converters
{
    public class ProgressToStatusConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int progress && values[1] is ModViewModel.ModStateEnum modState && values[2] is bool isUpdateAvailable)
            {
                if (parameter is string iconName)
                {
                    return GetIconVisibility(progress, modState, iconName, isUpdateAvailable);
                }
            }

            return false;
        }

        private bool GetIconVisibility(int progress, ModViewModel.ModStateEnum modState, string iconName, bool isUpdateAvailable)
        {
            switch (iconName)
            {
                case "Download":
                    return progress == 0 && modState == ModViewModel.ModStateEnum.NotInstalled && !isUpdateAvailable;
                case "Downloading":
                    return modState == ModViewModel.ModStateEnum.Downloading;
                case "Downloaded":
                    return progress >= 100 && (modState == ModViewModel.ModStateEnum.Installed && !isUpdateAvailable);

                default:
                    return false;
            }
        }

    }
}
