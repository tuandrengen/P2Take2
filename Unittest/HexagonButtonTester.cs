using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class HexagonButtonTester
    {
        Initializer initializer = new Initializer();

        //Burde man gører sådan, at man ikke kan sætte koordinaterne en negativværdi.
        [TestCase(-1, -1, false)]
        [TestCase(0, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(10, 10, true)]
        //[ExpectedException(typeof(Exception))]
        public void HexagonButton_PositiveValues_ConstructedRight(int x, int y, bool edge)
        {
            HexagonButton n = new HexagonButton(x, y, edge);
            Assert.AreEqual(x, n.XCoordinate);
            Assert.AreEqual(y, n.YCoordinate);
            Assert.AreEqual(edge, n.IsEdgeTile);
        }

        //Burde ikke kunne lave en buttom med negative koordinater
        [TestCase(-1, -3)]
        [TestCase(0, 0)]
        [TestCase(3, 1)]
        [TestCase(10, 10)]
        public void HexClicked_PositiveValues_ValuesChangedRight(int buttomX, int buttomY)
        {
            HexagonButton hex = new HexagonButton(buttomX, buttomY, false);
            initializer.InitializeMouseEventArgs();
            hex.HexClicked(hex, initializer.mouseArg);
            Assert.AreEqual(false, hex.Enabled);
            Assert.AreEqual(false, hex.Passable);
        }
    }
}
