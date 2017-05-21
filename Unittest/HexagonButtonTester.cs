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
        [TestCase(0, 0, true)]
        [TestCase(0, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(1, 1, false)]
        [TestCase(10, 10, true)]
        [TestCase(10, 10, false)]

        public void HexagonButton_PositiveValues_ConstructedRight(int x, int y, bool edge)
        {
            HexagonButton n = new HexagonButton(x, y, edge);
            Assert.AreEqual(x, n.XCoordinate);
            Assert.AreEqual(y, n.YCoordinate);
            Assert.AreEqual(edge, n.IsEdgeTile);
        }

        [TestCase(-1, -1, false)]
        [TestCase(-1, -1, true)]
        [TestCase(-10, -10, false)]
        public void HexagonButton_NegativeValues_ThrowsException(int x, int y, bool edge)
        {
            bool threwException = false;

            try
            {
                HexagonButton n = new HexagonButton(x, y, edge);
            }
            catch (MustBePositiveException)
            {
                threwException = true;
            }

            Assert.IsTrue(threwException);
        }

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
