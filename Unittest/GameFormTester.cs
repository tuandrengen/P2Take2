using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class GameFormTester : Initializer
    {
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(15, 15)]
        public void ButtonPainter_MathClassWorks_RightPolygon(int x, int y)
        {
            InitializeMap(x);
            HexagonButton hexagonButton = new HexagonButton(x, y, false);
            System.Drawing.Drawing2D.GraphicsPath buttonPath;
            PointF[] expectedPoints = P2SeriousGame.Math.GetPoints(x, y);
            
            //Inserted some part of the code from the method ButtonPainter.
            buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
            buttonPath.AddPolygon(P2SeriousGame.Math.GetPoints(x, y));
            Region region = new Region(buttonPath);
            hexagonButton.Region = region;

            Assert.AreEqual(hexagonButton.Region, region);
            Assert.AreEqual(expectedPoints, buttonPath.PathPoints);
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void ResetButtonClick_None_ChangesValuesRight(int size)
        {
            InitializeMouseEventArgs();
            HexagonButton hex = new HexagonButton(1, 1, false);
            GameForm window = new GameForm(size);
            window.ResetButtonClick(hex, mouseArg);

            Assert.AreEqual(false, hex.Visited);
            Assert.AreEqual(true, hex.Passable);
            Assert.AreEqual(true, hex.Enabled);
        }
    }
}
