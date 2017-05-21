using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class MathTester
    {
        [TestCase(-10, -10)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(10, 10)]
        public void GetPoints_NumericValues_RightPointsCreated(int height, int width)
        {
            //Calculates expected points.
            float expX = width / 2;
            float expSide = height / 2;
            float expY = 0;
            float expR = width / 2;
            float expH = (height - expSide) / 2;
            PointF[] expectedPoints = new PointF[6]
            {
                new PointF(expX, expY),
                new PointF(expX + expR, expY + expH),
                new PointF(expX + expR, expY + expSide + expH ),
                new PointF(expX, expY + height),
                new PointF(expX - expR, expY + expSide + expH),
                new PointF(expX - expR, expY + expH)
            };

            PointF[] actualPoints = P2SeriousGame.Math.GetPoints(height, width);
            
            Assert.AreEqual(expectedPoints, actualPoints);
        }

    }
}
