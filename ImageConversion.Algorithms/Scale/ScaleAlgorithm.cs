using System.Drawing;

namespace ImageConversion.Algorithms.Scale
{
    public abstract class ScaleAlgorithm : Algorithm
    {
        public abstract Bitmap Scale(Bitmap oldBitmap, float scaleX, float scaleY);
    }
}
