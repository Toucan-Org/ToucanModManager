using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.Themes
{
    public class GreenMintTheme : Theme
    {
        public static GreenMintTheme Instance { get; } = new GreenMintTheme();

        // Title
        public override SolidColorBrush TitleForeground => new SolidColorBrush(Colors.Black);

        // Button
        public override SolidColorBrush ButtonForeground => new SolidColorBrush(Color.Parse("#2C4C5A"));
        public override SolidColorBrush ButtonBorderBrush => new SolidColorBrush(Colors.Transparent);
        public override SolidColorBrush ButtonBackground => new SolidColorBrush(Color.Parse("#47A66C"));
        public override SolidColorBrush ButtonBackgroundPointerOver => new SolidColorBrush(Color.Parse("#68C083"));
        public override SolidColorBrush ButtonBackgroundPressed => new SolidColorBrush(Color.Parse("#3D7E5A"));

        // Background gradient
        public override SolidColorBrush GridBackground => new SolidColorBrush(Color.Parse("#101010"));

        public override LinearGradientBrush BackgroundGradientBrush => new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0.4, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#0034281E"), 0),
            new GradientStop(Color.Parse("#FF3C6E47"), 0.5),
            new GradientStop(Color.Parse("#FF5A9C6E"), 0.7),
            new GradientStop(Color.Parse("#FF78CA95"), 0.9),
            new GradientStop(Color.Parse("#FF96F8BC"), 1)
        }
        };
    }
}
