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
            if (values[0] is int progress && values[1] is ModViewModel.ModStateEnum modState)
            {
                if (parameter is string iconName)
                {
                    return GetIconVisibility(progress, modState, iconName);
                }
            }

            return AvaloniaProperty.UnsetValue;
        }

        private bool GetIconVisibility(int progress, ModViewModel.ModStateEnum modState, string iconName)
        {
            switch (iconName)
            {
                case "Download":
                    return progress == 0 && modState == ModViewModel.ModStateEnum.NotInstalled;
                case "Downloading":
                    return modState == ModViewModel.ModStateEnum.Downloading;
                case "Downloaded":
                    return progress >= 100 && modState == ModViewModel.ModStateEnum.Installed;

                default:
                    return false;
            }
        }
    }
}
