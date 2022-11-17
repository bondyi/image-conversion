using System;
using System.Drawing;

namespace ImageConversion.Algorithms.Rotation
{
    public class FastRotation : RotationAlgorithm
    {
        public override Bitmap Rotate(Bitmap oldBitmap, int angle)
        {
            var angleInRad = 0.0D - Math.PI * angle / 180;

            var newWidth = (float)(oldBitmap.Width * Math.Abs(Math.Cos(angleInRad)) + oldBitmap.Height * Math.Abs(Math.Sin(angleInRad)));
            var newHeight = (float)(oldBitmap.Width * Math.Abs(Math.Sin(angleInRad)) + oldBitmap.Height * Math.Abs(Math.Cos(angleInRad)));

            var newBitmap = new Bitmap(CeilingF(newWidth), CeilingF(newHeight));

            for (var x = 0; x < newWidth; ++x)
            {
                for (var y = 0; y < newHeight; ++y)
                {
                    var newX = FloorF((float)((x - newWidth / 2) * Math.Cos(angleInRad) - (y - newHeight / 2) * Math.Sin(angleInRad) + oldBitmap.Width / 2));
                    var newY = FloorF((float)((x - newWidth / 2) * Math.Sin(angleInRad) + (y - newHeight / 2) * Math.Cos(angleInRad) + oldBitmap.Height / 2));

                    if (newX >= 0 && newX < oldBitmap.Width && newY >= 0 && newY < oldBitmap.Height)
                    {
                        newBitmap.SetPixel(x, y, oldBitmap.GetPixel(newX, newY));
                    }
                }
            }

            return newBitmap;
        }
    }
}
