using Avalonia;
using Avalonia.Media;

namespace ToucanUI.Themes
{
    public class CelestialTheme : Theme
    {
        public static CelestialTheme Instance { get; } = new CelestialTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.White);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#2C4C5A"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#C33764"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#E94057"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#AB2553"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0.4, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#FFC33764"), 0),
            new GradientStop(Color.Parse("#FF1D2671"), 1)
        }
        };
    }

}
