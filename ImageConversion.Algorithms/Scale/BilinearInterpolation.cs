using System.Drawing;

namespace ImageConversion.Algorithms.Scale
{
    public class BilinearInterpolation : ScaleAlgorithm
    {
        public override Bitmap Scale(Bitmap oldBitmap, float scaleX, float scaleY)
        {
            var newBitmap = new Bitmap(CeilingF(oldBitmap.Width * scaleX), CeilingF(oldBitmap.Height * scaleY));

            for (var x = 0; x < newBitmap.Width - 1; ++x)
            {
                var originalXValue = x / ((float)(newBitmap.Width - 1) / (oldBitmap.Width - 1));
                var originalLeftX = FloorF(originalXValue);
                var originalRightX = originalLeftX + 1;

                for (var y = 0; y < newBitmap.Height - 1; ++y)
                {
                    var originalYValue = y / ((float)(newBitmap.Height - 1) / (oldBitmap.Height - 1));
                    var originalUpY = FloorF(originalYValue);
                    var originalDownY = originalUpY + 1;

                    var leftUpColor = oldBitmap.GetPixel(originalLeftX, originalUpY);
                    var leftDownColor = oldBitmap.GetPixel(originalLeftX, originalDownY);
                    var rightUpColor = oldBitmap.GetPixel(originalRightX, originalUpY);
                    var rightDownColor = oldBitmap.GetPixel(originalRightX, originalDownY);

                    var leftUpFactor = (originalXValue - originalLeftX) * (originalYValue - originalUpY);
                    var leftDownFactor = (originalXValue - originalLeftX) * (originalDownY - originalYValue);
                    var rightUpFactor = (originalRightX - originalXValue) * (originalYValue - originalUpY);
                    var rightDownFactor = (originalRightX - originalXValue) * (originalDownY - originalYValue);
                    

                    newBitmap.SetPixel(x, y, Color.FromArgb(
                        (byte)(leftUpColor.A * rightDownFactor + leftDownColor.A * rightUpFactor + rightUpColor.A * leftDownFactor + rightDownColor.A * leftUpFactor),
                        (byte)(leftUpColor.R * rightDownFactor + leftDownColor.R * rightUpFactor + rightUpColor.R * leftDownFactor + rightDownColor.R * leftUpFactor),
                        (byte)(leftUpColor.G * rightDownFactor + leftDownColor.G * rightUpFactor + rightUpColor.G * leftDownFactor + rightDownColor.G * leftUpFactor),
                        (byte)(leftUpColor.B * rightDownFactor + leftDownColor.B * rightUpFactor + rightUpColor.B * leftDownFactor + rightDownColor.B * leftUpFactor)
                    ));
                }

                var edgeFactorX = (originalXValue - originalLeftX) / (originalRightX - originalLeftX);

                var leftColor = oldBitmap.GetPixel(originalLeftX, oldBitmap.Height - 1);
                var rightColor = oldBitmap.GetPixel(originalRightX, oldBitmap.Height - 1);

                newBitmap.SetPixel(x, newBitmap.Height - 1, Color.FromArgb(
                    (byte)(leftColor.A + edgeFactorX * (rightColor.A - leftColor.A)),
                    (byte)(leftColor.R + edgeFactorX * (rightColor.R - leftColor.R)),
                    (byte)(leftColor.G + edgeFactorX * (rightColor.G - leftColor.G)),
                    (byte)(leftColor.B + edgeFactorX * (rightColor.B - leftColor.B))
                ));
            }

            for (var y = 0; y < newBitmap.Height - 1; ++y)
            {
                var originalYValue = y / ((float)(newBitmap.Height - 1) / (oldBitmap.Height - 1));
                var originalUpY = FloorF(originalYValue);
                var originalDownY = originalUpY + 1;

                var edgeFactorY = (originalYValue - originalUpY) / (originalDownY - originalUpY);

                var upColor = oldBitmap.GetPixel(oldBitmap.Width - 1, originalUpY);
                var downColor = oldBitmap.GetPixel(oldBitmap.Width - 1, originalDownY);

                newBitmap.SetPixel(newBitmap.Width - 1, y, Color.FromArgb(
                    (byte)(upColor.A + edgeFactorY * ((float)downColor.A - upColor.A)),
                    (byte)(upColor.R + edgeFactorY * ((float)downColor.R - upColor.R)),
                    (byte)(upColor.G + edgeFactorY * ((float)downColor.G - upColor.G)),
                    (byte)(upColor.B + edgeFactorY * ((float)downColor.B - upColor.B))
                ));
            }

            newBitmap.SetPixel(newBitmap.Width - 1, newBitmap.Height - 1, oldBitmap.GetPixel(oldBitmap.Width - 1, oldBitmap.Height - 1));

            return newBitmap;
        }
    }
}
