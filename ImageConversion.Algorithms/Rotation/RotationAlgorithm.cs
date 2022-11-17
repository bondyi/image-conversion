using System.Drawing;

namespace ImageConversion.Algorithms.Rotation
{
    public abstract class RotationAlgorithm : Algorithm
    {
        public abstract Bitmap Rotate(Bitmap oldBitmap, int angle);
    }
}
