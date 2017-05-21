using System.Drawing;

namespace P2SeriousGame
{
    public class Math
    {
        /// <summary>
        /// Calculates the 6 points in a hexagon from a rectangle.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static PointF[] GetPoints(int height, int width)
        {
            float x, y, r, h;

            float side = height / 2;
            x = width / 2;
            y = 0;
            r = width / 2;
            h = (height - side) / 2;

            PointF[] points = new PointF[6];
            points[0] = new PointF(x, y);
            points[1] = new PointF(x + r, y + h);
            points[2] = new PointF(x + r, y + side + h);
            points[3] = new PointF(x, y + height);
            points[4] = new PointF(x - r, y + side + h);
            points[5] = new PointF(x - r, y + h);
            return points;
        }
    }
}
