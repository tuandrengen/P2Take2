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
        [TestCase(1)]
        public void WinOrLoss_WinIs1_AssignsRight(int win)
        {
            //Values doesn't matter at this point.
            Round round = new Round(1, 1, 1, 1, 1, 1);
            round.WinOrLoss(win);

            Assert.AreEqual(1, round.Win);
            Assert.AreEqual(0, round.Loss);
        }

        [TestCase(0)]
        public void WinOrLoss_WinIs0_AssignsRight(int win)
        {
            //Values doesn't matter at this point.
            Round round = new Round(1, 1, 1, 1, 1, 1);
            round.WinOrLoss(win);


            Assert.AreEqual(0, round.Win);
            Assert.AreEqual(1, round.Loss);

        }

        [TestCase(1, 1, 100, 33, 1, 500)]
        [TestCase(2, 2, 200, 66, 0, 600)]
        [TestCase(3, 3, 300, 99, 1, 700)]
        [TestCase(4, 4, 400, 132, 0, 800)]
        [TestCase(5, 5, 500, 165, 1, 900)]
        public void Round_ValuesOfRightFormat_ContructedRight(int roundNumber, int roundID, float clicks, float clicksAVG, int win, float timeUsed)
        {
            Round fullRound = new Round(roundNumber, clicks, clicksAVG, win, timeUsed, roundID);
            
            fullRound.WinOrLoss(win);
            Assert.AreEqual(roundNumber, fullRound.RoundNumber);
            Assert.AreEqual(roundID, fullRound.RoundID);
            Assert.AreEqual(clicks, fullRound.NumberOfClicks);
            Assert.AreEqual(clicksAVG, fullRound.ClicksPerMinute);
            Assert.AreEqual(timeUsed, fullRound.TimeUsed);
        }
    }
}
