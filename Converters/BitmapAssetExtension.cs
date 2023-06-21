using System;
using System.IO;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace ToucanUI.Converters
{
    public class BitmapAssetExtension : MarkupExtension
    {
        public string AssetPath { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrEmpty(AssetPath))
            {
                string currentAssemblyPath = AppContext.BaseDirectory;
                string fullPath = Path.Combine(currentAssemblyPath, AssetPath.Replace('/', Path.DirectorySeparatorChar));
                return new Bitmap(fullPath);
            }

            return null;
        }
    }

}
