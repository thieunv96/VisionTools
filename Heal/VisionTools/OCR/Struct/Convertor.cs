using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Heal.VisionTools.OCR.Struct
{
    public class BrushColorCvt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Drawing.Color? color = value as System.Drawing.Color?;
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(color.Value.R, color.Value.G, color.Value.B));
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            return System.Drawing.Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);
        }

    }
    class Convertor
    {
        public static BitmapSource Bitmap2BitmapSource(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            var pixelFormat = bitmap.PixelFormat;
            PixelFormat format = PixelFormats.Bgr24;
            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    format = PixelFormats.Gray8;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    format = PixelFormats.Bgr24;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    format = PixelFormats.Bgra32;
                    break;
                default:
                    break;
            }
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                format, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
