using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using ToucanUI.Models.KSP2;

namespace ToucanUI.Converters;

public class UpdateVisibilityConverter : IMultiValueConverter
{
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is bool isUpdateAvailable && values[1] is ModViewModel.ModStateEnum modState)
        {
            return isUpdateAvailable && modState == ModViewModel.ModStateEnum.Installed;
        }

        return false; // Button should be hidden if conditions are not met
    }
}
