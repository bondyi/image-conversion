using System.Drawing;

namespace ImageConversion.Algorithms.Scale
{
    public class NearestNeighbor : ScaleAlgorithm
    {
        public override Bitmap Scale(Bitmap oldBitmap, float scaleX, float scaleY)
        {
            var newBitmap = new Bitmap(CeilingF(oldBitmap.Width * scaleX), CeilingF(oldBitmap.Height * scaleY));

            for (var x = 0; x < newBitmap.Width; ++x)
            {
                for (var y = 0; y < newBitmap.Height; ++y)
                {
                    newBitmap.SetPixel(x, y, oldBitmap.GetPixel(FloorF(x / scaleX), FloorF(y / scaleY)));
                }
            }

            return newBitmap;
        }
    }
}
