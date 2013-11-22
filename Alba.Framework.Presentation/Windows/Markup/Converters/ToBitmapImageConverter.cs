using System;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Alba.Framework.Windows.Markup
{
    public class ToBitmapImageConverter : IValueConverter
    {
        public RequestCachePolicy UriCachePolicy { get; set; }
        public BitmapCreateOptions CreateOptions { get; set; }
        public BitmapCacheOption CacheOption { get; set; }
        public int DecodePixelWidth { get; set; }
        public int DecodePixelHeight { get; set; }
        public Int32Rect SourceRect { get; set; }
        public Rotation Rotation { get; set; }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
                return DependencyProperty.UnsetValue;
            
            Uri uriSource = null;
            Stream streamSource = null;
            var valueUri = value as Uri;
            if (valueUri != null)
                uriSource = valueUri;
            if (uriSource == null) {
                var valueStr = value as string;
                if (valueStr != null)
                    uriSource = new Uri(valueStr, UriKind.RelativeOrAbsolute);
                if (uriSource == null) {
                    var valueStream = value as Stream;
                    if (valueStream != null)
                        streamSource = valueStream;
                }
            }
            if (uriSource == null && streamSource == null)
                throw new ArgumentException("Value must be Uri, Stream or string.");

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = uriSource;
            img.StreamSource = streamSource;
            img.UriCachePolicy = UriCachePolicy;
            img.CreateOptions = CreateOptions;
            img.CacheOption = CacheOption;
            img.DecodePixelWidth = DecodePixelWidth;
            img.DecodePixelHeight = DecodePixelHeight;
            img.SourceRect = SourceRect;
            img.Rotation = Rotation;
            img.EndInit();
            return img;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}