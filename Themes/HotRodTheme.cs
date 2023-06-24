using Avalonia;
using Avalonia.Media;

namespace ToucanUI.Themes
{
    public class HotRodTheme : Theme
    {
        public static HotRodTheme Instance { get; } = new HotRodTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.Black);

        // Button 
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#800F0E"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#C66000"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#ff7b00"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#FFDD00"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0.4, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new GradientStop(Color.Parse("#005D3C1A"), 0),
                new GradientStop(Color.Parse("#FF5D3C1A"), 0.5),
                new GradientStop(Color.Parse("#FFA85430"), 0.7),
                new GradientStop(Color.Parse("#FFBF4040"), 0.9),
                new GradientStop(Color.Parse("#FFBF4040"), 1)
            }
        };
    }
}
