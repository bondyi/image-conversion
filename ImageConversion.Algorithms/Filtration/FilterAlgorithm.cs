using System.Drawing;

namespace ImageConversion.Algorithms.Filtration
{
    public abstract class FilterAlgorithm : Algorithm
    {
        protected static Bitmap GetBufferBitmap(Bitmap oldBitmap)
        {
            var bufferBitmap = new Bitmap(oldBitmap.Width + 2, oldBitmap.Height + 2);

            for (var x = 0; x < oldBitmap.Width; ++x)
            {
                for (var y = 0; y < oldBitmap.Height; ++y)
                {
                    bufferBitmap.SetPixel(x + 1, y + 1, oldBitmap.GetPixel(x, y));
                }
            }

            for (var x = 1; x < bufferBitmap.Width - 1; ++x)
            {
                bufferBitmap.SetPixel(x, 0, bufferBitmap.GetPixel(x, 1));
                bufferBitmap.SetPixel(x, bufferBitmap.Height - 1, bufferBitmap.GetPixel(x, bufferBitmap.Height - 2));
            }

            for (var y = 1; y < bufferBitmap.Height - 1; ++y)
            {
                bufferBitmap.SetPixel(0, y, bufferBitmap.GetPixel(1, y));
                bufferBitmap.SetPixel(bufferBitmap.Width - 1, y, bufferBitmap.GetPixel(bufferBitmap.Width - 2, y));
            }

            bufferBitmap.SetPixel(0, 0, bufferBitmap.GetPixel(1, 1));
            bufferBitmap.SetPixel(bufferBitmap.Width - 1, 0, bufferBitmap.GetPixel(bufferBitmap.Width - 2, 1));
            bufferBitmap.SetPixel(0, bufferBitmap.Height - 1, bufferBitmap.GetPixel(1, bufferBitmap.Height - 2));
            bufferBitmap.SetPixel(bufferBitmap.Width - 1, bufferBitmap.Height - 1, bufferBitmap.GetPixel(bufferBitmap.Width - 2, bufferBitmap.Height - 2));

            return bufferBitmap;
        }

        protected static Bitmap ExtractBitmap(Bitmap bufferBitmap, int fromX, int fromY, int width, int height)
        {
            var newBitmap = new Bitmap(width, height);

            for (var x = 0; x < newBitmap.Width; ++x)
            {
                for (var y = 0; y < newBitmap.Height; ++y)
                {
                    newBitmap.SetPixel(x, y, bufferBitmap.GetPixel(x + fromX, y + fromY));
                }
            }

            return newBitmap;
        }

        protected static double GetNormalizingFactor(double[,] mask)
        {
            var sum = 0.0D;

            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 3; ++j)
                {
                    sum += mask[i, j];
                }
            }

            return 1.0D / sum;
        }

        public abstract Bitmap Filtrate(Bitmap oldBitmap, double[,] mask);
    }
}
