﻿using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class GameFormTester
    {
        Initializer initializer = new Initializer();

        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(15, 15)]
        public void ButtonPainter_ValidMapSize_CreatesCorrectHexagon(int x, int y)
        {
            initializer.InitializeMap(x);
            HexagonButton hexagonButton = new HexagonButton(x, y, false);
            System.Drawing.Drawing2D.GraphicsPath buttonPath;
            PointF[] expectedPoints = P2SeriousGame.Math.GetPoints(x, y);
            
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
        public void ResetButtonClick_ValidMapSize_ResetsButtonCorrectly(int size)
        {
            initializer.InitializeMouseEventArgs();
            HexagonButton hex = new HexagonButton(1, 1, false);
            GameForm window = new GameForm(size);
            window.ResetButtonClick(hex, initializer.mouseArg);

            Assert.AreEqual(false, hex.Visited);
            Assert.AreEqual(true, hex.Passable);
            Assert.AreEqual(true, hex.Enabled);
        }
    }
}
