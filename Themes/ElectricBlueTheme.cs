using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.Themes
{
    public class ElectricBlueTheme : Theme
    {
        public static ElectricBlueTheme Instance { get; } = new ElectricBlueTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.White);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#2C4C5A"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#4494A6"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#5BB0C8"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#3E7990"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0.4, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#001A5656"), 0),
            new GradientStop(Color.Parse("#FF1A4D73"), 0.5),
            new GradientStop(Color.Parse("#FF2C3F90"), 0.7),
            new GradientStop(Color.Parse("#FF3D31AD"), 0.9),
            new GradientStop(Color.Parse("#FF4E23CA"), 1)
        }
        };
    }

}
