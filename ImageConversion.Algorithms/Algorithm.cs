using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace ImageConversion.Algorithms
{
    public abstract class Algorithm
    {
        protected static int RoundF(float value) => (int)MathF.Round(value);

        protected static int CeilingF(float value) => (int)MathF.Ceiling(value);

        protected static int FloorF(float value) => (int)MathF.Floor(value);

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}
