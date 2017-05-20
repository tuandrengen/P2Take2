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
