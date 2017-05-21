using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class FormattingTests
    {
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void Formatting_RightValues_ContructedRigth(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Assert.AreEqual(window, format.control);
        }
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void Constants_None_RightValueOfConstants(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);


            //    public int ButtonWidth = 0;
            //public int ButtonHeight = 0;
            //public int ButtonHeightOffset => (3 * (ButtonHeight / 4));

            //public int SmallBtnSpacing = 30;
            //public int LargeBtnSpacing = 60;
            //public int BtnCount = 1;

            //public int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            //public int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            ////These constants declare the amount of reserved space or margins, where 0.05 equals 5%
            //public const double _leftWidthReserved = 0.05;
            //public const double _endWidthReserved = 0.12;
            //public const double _topHeightReserved = 0.05;
            //public const double _bottomHeightReserved = 0.03;

            ////The gamescreen variables sets the height and width of the area on the screen where hexagonbutton can be drawn
            //public double _gameScreenWidth = Screen.PrimaryScreen.Bounds.Width * (1 - (_leftWidthReserved + _endWidthReserved));
            //public double _gameScreenHeight = Screen.PrimaryScreen.Bounds.Height * (1 - (_topHeightReserved + _bottomHeightReserved));

            ////Centers the hexagonmap starting placement, if the hexagonmap doesnt fill out the entire gamescreen width
            //public double WidthCentering => (_gameScreenWidth - (ButtonWidth * Map.TotalHexagonColumns)) / 2;

            ////WidthStart and heightStart sets the starting place for the hexagonmap
            //public int WidthStart => (int)((_leftWidthReserved * Screen.PrimaryScreen.Bounds.Width) + WidthCentering);


            //public int _heightStart = (int)(_topHeightReserved * Screen.PrimaryScreen.Bounds.Height);

            Assert.AreEqual(0, format.ButtonWidth);
            Assert.AreEqual(0, format.ButtonHeight);
            Assert.AreEqual(0, format.ButtonHeightOffset);
            Assert.True(false);
            //Lave færdig hvis på variablerne som forbliver public.
            


    }



        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnRightFormat_None_CorrectlySetsButtonValues(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Button btn = new Button();
            string btnText = "Empty";
            Color color = Color.Red;
            Size expectedSize = new Size(150, 60);

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                format.BtnRightFormat(btn, btnText, color);
                int numberOfButtons = format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point(format.control.Bounds.Right - btn.Width - 30, format.control.Bounds.Top + (numberOfButtons * btn.Height));

                Assert.AreEqual(expectedSize, btn.Size);
                Assert.AreEqual(false, btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, btn.FlatStyle);
                Assert.AreEqual(0, btn.FlatAppearance.BorderSize);
                Assert.AreEqual(color, btn.BackColor);
                Assert.AreEqual(expectedPoint, btn.Location);
                Assert.AreEqual(btnText, btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, format.BtnCount);
            }
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnCenterFormat_None_CorrectlySetsButtonValues(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Button btn = new Button();
            string btnText = "Empty";
            Color color = Color.Red;
            Size expectedSize = new Size(300, 100);

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                format.BtnCenterFormat(btn, btnText, color);
                int numberOfButtons = format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point((format.control.Bounds.Right / 2) - (btn.Width / 2), format.control.Bounds.Top + (numberOfButtons * 60));

                Assert.AreEqual(expectedSize, btn.Size);
                Assert.AreEqual(false, btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, btn.FlatStyle);
                Assert.AreEqual(0, btn.FlatAppearance.BorderSize);
                Assert.AreEqual(color, btn.BackColor);
                Assert.AreEqual(expectedPoint, btn.Location);
                Assert.AreEqual(btnText, btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, format.BtnCount);

            }
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnLeftFormat_None_CorrectlySetsButtonValues(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Button btn = new Button();
            string btnText = "Empty";
            Color color = Color.Red;
            Size expectedSize = new Size(200, 75);
            

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                format.BtnLeftFormat(btn, btnText, color);
                int numberOfButtons = format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point(30, format.control.Bounds.Top + (numberOfButtons * 30));

                Assert.AreEqual(expectedSize, btn.Size);
                Assert.AreEqual(false, btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, btn.FlatStyle);
                Assert.AreEqual(0, btn.FlatAppearance.BorderSize);
                Assert.AreEqual(color, btn.BackColor);
                Assert.AreEqual(expectedPoint, btn.Location);
                Assert.AreEqual(btnText, btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, format.BtnCount);
            }
        }
    }
}
