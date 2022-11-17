using System.Drawing;

namespace ImageConversion.Algorithms.Scale
{
    public class IncreaseInKTimes : ScaleAlgorithm
    {
        private static Color GetDeltaColor(Color left, Color right, int countOfNewPixels)
        {
            return Color.FromArgb(
                (byte)((0.0f - left.A + right.A) / countOfNewPixels),
                (byte)((0.0f - left.R + right.R) / countOfNewPixels),
                (byte)((0.0f - left.G + right.G) / countOfNewPixels),
                (byte)((0.0f - left.B + right.B) / countOfNewPixels));
        }

        private static Color GetPixelColor(Color left, Color delta, int pixelPlace)
        {
            return Color.FromArgb(
                (byte)(left.A + delta.A * pixelPlace),
                (byte)(left.R + delta.R * pixelPlace),
                (byte)(left.G + delta.G * pixelPlace),
                (byte)(left.B + delta.B * pixelPlace));
        }

        public override Bitmap Scale(Bitmap oldBitmap, float scaleX, float scaleY)
        {
            var newBitmap = new Bitmap(CeilingF(oldBitmap.Width * scaleX), CeilingF(oldBitmap.Height * scaleY));

            for (var x = 0; x < oldBitmap.Width; ++x)
            {
                for (var y = 0; y < oldBitmap.Height; ++y)
                {
                    newBitmap.SetPixel(RoundF(x * scaleX), RoundF(y * scaleY), oldBitmap.GetPixel(x, y));
                }
            }

            if (scaleX > 1.0f)
            {
                for (var x = 0; x < oldBitmap.Width - 1; ++x)
                {
                    var countOfNewPixels = RoundF((x + 1) * scaleX) - RoundF(x * scaleX) - 1;

                    for (var y = 0; y < oldBitmap.Height; ++y)
                    {
                        if (countOfNewPixels > 0)
                        {
                            var deltaColor = GetDeltaColor(oldBitmap.GetPixel(x, y), oldBitmap.GetPixel(x + 1, y), countOfNewPixels);

                            for (var i = 1; i <= countOfNewPixels; ++i)
                            {
                                newBitmap.SetPixel(RoundF(x * scaleX) + i, RoundF(y * scaleY), GetPixelColor(oldBitmap.GetPixel(x, y), deltaColor, i));
                            }
                        }
                    }
                }

                for (var x = RoundF((oldBitmap.Width - 1) * scaleX); x < newBitmap.Width; ++x)
                {
                    for (var y = 0; y < RoundF((oldBitmap.Height - 1) * scaleY) + 1; ++y)
                    {
                        newBitmap.SetPixel(x, y, newBitmap.GetPixel(x - 1, y));
                    }
                }

            }

            if (scaleY > 1.0f)
            {
                for (var y = 0; y < oldBitmap.Height - 1; ++y)
                {
                    var countOfNewPixels = RoundF((y + 1) * scaleY) - RoundF(y * scaleY) - 1;

                    for (var x = 0; x < RoundF(oldBitmap.Width * scaleX); ++x)
                    {
                        if (countOfNewPixels > 0)
                        {
                            var deltaColor = GetDeltaColor(newBitmap.GetPixel(x, RoundF(y * scaleY)), newBitmap.GetPixel(x, RoundF((y + 1) * scaleY)), countOfNewPixels);

                            for (var i = 1; i <= countOfNewPixels; ++i)
                            {
                                newBitmap.SetPixel(x, RoundF(y * scaleY) + i, GetPixelColor(newBitmap.GetPixel(x, RoundF(y * scaleY)), deltaColor, i));
                            }
                        }
                    }
                }

                for (var x = 0; x < newBitmap.Width; ++x)
                {
                    for (var y = RoundF((oldBitmap.Height - 1) * scaleY) + 1; y < newBitmap.Height; ++y)
                    {
                        newBitmap.SetPixel(x, y, newBitmap.GetPixel(x, y - 1));
                    }
                }

            }

            return newBitmap;
        }
    }
}
