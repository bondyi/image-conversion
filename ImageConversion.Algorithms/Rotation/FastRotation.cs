using System;
using System.Drawing;

namespace ImageConversion.Algorithms.Rotation
{
    public class FastRotation : RotationAlgorithm
    {
        public override Bitmap Rotate(Bitmap oldBitmap, int angle)
        {
            var angleInRad = 0.0D - Math.PI * angle / 180;

            var sinAngleInRad = Math.Sin(angleInRad);
            var cosAngleInRad = Math.Cos(angleInRad);

            var newWidth = CeilingF((float)(
                oldBitmap.Width * Math.Abs(cosAngleInRad) + 
                oldBitmap.Height * Math.Abs(sinAngleInRad)));

            var newHeight = CeilingF((float)(
                oldBitmap.Width * Math.Abs(sinAngleInRad) + 
                oldBitmap.Height * Math.Abs(cosAngleInRad)));

            var newBitmap = new Bitmap(newWidth, newHeight);

            var newBitmapCenter = new Point(newBitmap.Width / 2, newBitmap.Height / 2);

            var unOffsetX = oldBitmap.Width / 2;
            var unOffsetY = oldBitmap.Height / 2;

            for (var x = 0; x < newWidth; ++x)
            {
                var offsetX = x - newBitmapCenter.X;

                for (var y = 0; y < newHeight; ++y)
                {
                    var offsetY = y - newBitmapCenter.Y;

                    var xToRotate = FloorF((float)(
                        offsetX * cosAngleInRad -
                        offsetY * sinAngleInRad +
                        unOffsetX));

                    var yToRotate = FloorF((float)(
                        offsetX * sinAngleInRad + 
                        offsetY * cosAngleInRad + 
                        unOffsetY));

                    if (xToRotate >= 0 && xToRotate < oldBitmap.Width && 
                        yToRotate >= 0 && yToRotate < oldBitmap.Height)
                    {
                        newBitmap.SetPixel(x, y, oldBitmap.GetPixel(xToRotate, yToRotate));
                    }
                }
            }

            return newBitmap;
        }
    }
}
