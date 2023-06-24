using Avalonia.Media;

namespace ToucanUI.Themes
{
    public abstract class Theme
    {
        // Title
        public abstract SolidColorBrush TitleForeground { get; }

        // Launch button
        public abstract SolidColorBrush ButtonForeground { get; }
        public abstract SolidColorBrush ButtonBorderBrush { get; }
        public abstract SolidColorBrush ButtonBackground { get; }
        public abstract SolidColorBrush ButtonBackgroundPointerOver { get; }
        public abstract SolidColorBrush ButtonBackgroundPressed { get; }

        // Background/Gradient
        public abstract SolidColorBrush GridBackground { get; }
        public abstract LinearGradientBrush BackgroundGradientBrush { get; }
    }

}
