using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.Themes
{
    public class BlueOceanTheme : Theme
    {
        public static BlueOceanTheme Instance { get; } = new BlueOceanTheme();

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
            StartPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops
        {
            new GradientStop(Color.Parse("#00c6ff"), 0),
            new GradientStop(Color.Parse("#0072ff"), 1)
        }
        };
    }

}
