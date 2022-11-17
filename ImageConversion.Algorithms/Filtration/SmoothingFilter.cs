using System;
using System.Drawing;

namespace ImageConversion.Algorithms.Filtration
{
    public class SmoothingFilter : FilterAlgorithm
    {
        public override Bitmap Filtrate(Bitmap oldBitmap, double[,] mask)
        {
            var bufferBitmap = GetBufferBitmap(oldBitmap);
            var normalizingFactor = GetNormalizingFactor(mask);

            for (var x = 0; x < oldBitmap.Width; ++x)
            {
                for (var y = 0; y < oldBitmap.Height; ++y)
                {
                    var alpha = 0.0D;
                    var red = 0.0D;
                    var green = 0.0D;
                    var blue = 0.0D;

                    for (var filterX = 0; filterX < 3; ++filterX)
                    {
                        var filterOnBitmapX = x - 1 + filterX;

                        for (var filterY = 0; filterY < 3; ++filterY)
                        {
                            var filterOnBitmapY = y - 1 + filterY;

                            var px = oldBitmap.GetPixel(Math.Clamp(filterOnBitmapX, 0, oldBitmap.Width - 1), Math.Clamp(filterOnBitmapY, 0, oldBitmap.Height - 1));
                            alpha += px.A * mask[filterX, filterY];
                            red += px.R * mask[filterX, filterY];
                            green += px.G * mask[filterX, filterY];
                            blue += px.B * mask[filterX, filterY];
                        }
                    }

                    alpha *= normalizingFactor;
                    red *= normalizingFactor;
                    green *= normalizingFactor;
                    blue *= normalizingFactor;

                    var filteredColor = Color.FromArgb(Math.Clamp((int)alpha, 0, 255), Math.Clamp((int)red, 0, 255), Math.Clamp((int)green, 0, 255), Math.Clamp((int)blue, 0, 255));

                    bufferBitmap.SetPixel(x + 1, y + 1, filteredColor);
                }
            }

            return ExtractBitmap(bufferBitmap, 1, 1, oldBitmap.Width, oldBitmap.Height);
        }
    }
}
