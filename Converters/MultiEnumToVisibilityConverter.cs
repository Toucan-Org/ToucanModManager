using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using ToucanUI.ViewModels;

namespace ToucanUI.Converters
{
    public class MultiEnumToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 2 || !(values[0] is ModlistViewModel.FetchStateEnum) || !(values[1] is ModlistViewModel.ViewStateEnum))
                return AvaloniaProperty.UnsetValue;

            var fetchState = (ModlistViewModel.FetchStateEnum)values[0];
            var viewState = (ModlistViewModel.ViewStateEnum)values[1];
            var viewStateToMatch = Enum.Parse<ModlistViewModel.ViewStateEnum>((string)parameter);

            return fetchState == ModlistViewModel.FetchStateEnum.Success && viewState == viewStateToMatch;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
