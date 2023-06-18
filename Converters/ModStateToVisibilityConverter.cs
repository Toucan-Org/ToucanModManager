using Avalonia.Data.Converters;
using System;
using System.Globalization;
using static ToucanUI.Models.KSP2.ModViewModel;

namespace ToucanUI.Converters
{
    public class ModStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModStateEnum modState)
            {
                return modState == ModStateEnum.Installed;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
