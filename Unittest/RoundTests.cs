﻿using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;


namespace Unittest
{
    [TestFixture]
    public class RoundTests
    {
        [TestCase(1, 100, 33, 1, 500)]
        [TestCase(2, 200, 66, 0, 600)]
        [TestCase(3, 300, 99, 1, 700)]
        [TestCase(4, 400, 132, 0, 800)]
        [TestCase(5, 500, 165, 1, 900)]
        public void Round_ValuesOfRightFormat_ContructedRight(int roundID, float clicks, float clicksAVG, int win, float timeUsed)
        {
            Round notFullRound = new Round(clicks, clicksAVG, win, timeUsed);
            Round fullRound = new Round(roundID, clicks, clicksAVG, win, timeUsed);

            notFullRound.WinOrLoss(win);
            Assert.AreEqual(clicks, fullRound.NumberOfClicks);
            Assert.AreEqual(clicksAVG, fullRound.ClicksPerMinute);
            Assert.AreEqual(win, fullRound.Win);
            Assert.AreEqual(timeUsed, fullRound.TimeUsed);

            fullRound.WinOrLoss(win);
            Assert.AreEqual(roundID, fullRound.RoundID);
            Assert.AreEqual(clicks, fullRound.NumberOfClicks);
            Assert.AreEqual(clicksAVG, fullRound.ClicksPerMinute);
            Assert.AreEqual(win, fullRound.Win);
            Assert.AreEqual(timeUsed, fullRound.TimeUsed);


        }
    }
}
