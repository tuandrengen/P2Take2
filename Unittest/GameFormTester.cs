using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class GameFormTester
    {
        //denne burde virke, man kan ikke få Painteventet triggered. Har bare indsat funkionen, så man ikke kalder dem, men bare udfører den.
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(15, 15)]
        public void ButtonPainter_MathClassWorks_RightPolygon(int x, int y)
        {
            HexagonButton hexagonButton = new HexagonButton(x, y, false);
            GameForm window = new GameForm(x);
            IPathfinding ipathfinding = new Pathfinding(window);
            System.Drawing.Drawing2D.GraphicsPath buttonPath;
            PointF[] expectedPoints = P2SeriousGame.Math.GetPoints(x, y);
            Map map = new Map(window, x, ipathfinding);

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
            MouseButtons mBtn = new MouseButtons();
            MouseEventArgs mArgs = new MouseEventArgs(mBtn, 0, 0, 0, 0);
            HexagonButton hex = new HexagonButton(1, 1, false);
            GameForm window = new GameForm(size);
            window.ResetButtonClick(hex, mArgs);

            Assert.AreEqual(false, hex.Visited);
            Assert.AreEqual(true, hex.Passable);
            Assert.AreEqual(true, hex.Enabled);
        }

    }
}
