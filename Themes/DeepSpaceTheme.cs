using Avalonia.Media;
using Avalonia;

namespace ToucanUI.Themes
{
    public class DeepSpaceTheme : Theme
    {
        public static DeepSpaceTheme Instance { get; } = new DeepSpaceTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.White);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#000000"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#1B3753"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#387EC5"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#48A4FF"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#000000"), 0),
            new GradientStop(Color.Parse("#434343"), 1)
        }
        };
    }

}
