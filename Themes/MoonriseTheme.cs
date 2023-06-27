using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.Themes
{
    public class MoonriseTheme : Theme
    {
        public static MoonriseTheme Instance { get; } = new MoonriseTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.White);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#000000"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#CCCECE"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#CBE2E8"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#C7F4FF"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#eef2f3"), 0),
            new GradientStop(Color.Parse("#8e9eab"), 1)
        }
        };
    }
}
