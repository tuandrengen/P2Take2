using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class DatabaseTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void WinOrLoss_None_SetsRightValues(bool won)
        {
            Database database = new Database();
            Pathfinding.gameRoundWin = won;
            database.WinOrLose();
            if (won)
            {
                Assert.AreEqual(0, database.RoundLoss);
                Assert.AreEqual(1, database.RoundWin);
                Assert.AreEqual(1, database.WinOrLose());
            }
            else if(!won)
            {
                Assert.AreEqual(1, database.RoundLoss);
                Assert.AreEqual(0, database.RoundWin);
                Assert.AreEqual(0, database.WinOrLose());
            }
        }


    }
}
