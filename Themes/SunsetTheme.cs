using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.Themes
{
    public class SunsetTheme : Theme
    {
        public static SunsetTheme Instance { get; } = new SunsetTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.Black);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#000000"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#CA844F"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#EF8C40"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#FA7C1C"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#ff7e5f"), 0),
            new GradientStop(Color.Parse("#feb47b"), 1)
        }
        };
    }
}
