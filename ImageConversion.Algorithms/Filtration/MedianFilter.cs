using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageConversion.Algorithms.Filtration
{
    public class MedianFilter : FilterAlgorithm
    {
        public override Bitmap Filtrate(Bitmap oldBitmap, double[,] mask)
        {
            var bufferBitmap = GetBufferBitmap(oldBitmap);

            var filterWindow = new List<Color>(9);

            for (var x = 0; x < bufferBitmap.Width; ++x)
            {
                for (var y = 0; y < bufferBitmap.Height; ++y)
                {
                    for (var filterX = 0; filterX < 3; ++filterX)
                    {
                        for (var filterY = 0; filterY < 3; ++filterY)
                        {
                            var filterOnBitmapX = (x - 3 / 2 + filterX + bufferBitmap.Width) % bufferBitmap.Width;
                            var filterOnBitmapY = (y - 3 / 2 + filterY + bufferBitmap.Height) % bufferBitmap.Height;

                            filterWindow.Add(bufferBitmap.GetPixel(filterOnBitmapX, filterOnBitmapY));
                        }
                    }

                    var filteredColor = Color.FromArgb(
                        filterWindow.OrderBy(color => color.A).ElementAt(4).A,
                        filterWindow.OrderBy(color => color.R).ElementAt(4).R,
                        filterWindow.OrderBy(color => color.G).ElementAt(4).G,
                        filterWindow.OrderBy(color => color.B).ElementAt(4).B);

                    bufferBitmap.SetPixel(x, y, filteredColor);

                    filterWindow.Clear();
                }
            }

            return ExtractBitmap(bufferBitmap);
        }
    }
}
