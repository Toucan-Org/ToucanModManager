using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using ToucanUI.ViewModels;


// This class is used to convert the progress of a mod download to a color for the ProgressBar

namespace ToucanUI.Converters
{
    public class ProgressToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int progress)
            {
                if (progress >= 100)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 195, 195, 195)); // Gray color
                }
                double relativeProgress = progress / 100.0;
                Color color = InterpolateColor(relativeProgress);
                return new SolidColorBrush(color);
            }

            return Brushes.Transparent;
        }

        private Color InterpolateColor(double progress)
        {
            var color1 = Color.FromRgb(255, 0, 0); // Red
            var color2 = Color.FromRgb(0, 255, 0); // Green

            var hsl1 = RgbToHsl(color1);
            var hsl2 = RgbToHsl(color2);

            float h = hsl1.H + (hsl2.H - hsl1.H) * (float)progress;
            float s = hsl1.S + (hsl2.S - hsl1.S) * (float)progress;
            float l = hsl1.L + (hsl2.L - hsl1.L) * (float)progress;

            return HslToRgb(h, s, l);
        }

        private (float H, float S, float L) RgbToHsl(Color color)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float max = Math.Max(Math.Max(r, g), b);
            float min = Math.Min(Math.Min(r, g), b);

            float h, s, l;
            l = (max + min) / 2;

            if (max == min)
            {
                h = s = 0;
            }
            else
            {
                float d = max - min;
                s = l > 0.5f ? d / (2 - max - min) : d / (max + min);

                if (max == r)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / d + 2;
                }
                else
                {
                    h = (r - g) / d + 4;
                }

                h /= 6;
            }

            return (h, s, l);
        }

        private Color HslToRgb(float h, float s, float l)
        {
            float r, g, b;

            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
                float p = 2 * l - q;

                r = HueToRgb(p, q, h + 1 / 3f);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1 / 3f);
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        private float HueToRgb(float p, float q, float t)
        {
            if (t < 0)
                t += 1;
            if (t > 1)
                t -= 1;

            if (t < 1 / 6f)
                return p + (q - p) * 6 * t;
            if (t < 1 / 2f)
                return q;
            if (t < 2 / 3f)
                return p + (q - p) * (2 / 3f - t) * 6;

            return p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}