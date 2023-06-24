using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using ToucanUI.Themes;

namespace ToucanUI.Services
{
    public class ThemeManager
    {
        public static void LoadTheme(Theme theme)
        {
            var app = (App)Application.Current;
            app.Styles.Clear();
            app.Styles.Add(new Style(x => x.OfType<Button>())
            {
                Setters =
            {
                new Setter(Button.ForegroundProperty, theme.ButtonForeground),
                new Setter(Button.BackgroundProperty, theme.ButtonBackground),
            }
            });
            app.Resources["BackgroundGradientBrush"] = theme.BackgroundGradientBrush;
        }
    }
}
