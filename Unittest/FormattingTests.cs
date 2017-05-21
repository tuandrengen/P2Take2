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
        Initializer initializer = new Initializer();

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void Formatting_ValidMapSize_ContructedRigth(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Assert.AreEqual(window, format.control);
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnRightFormat_ValidMapSize_CorrectlySetsButtonValues(int size)
        {
            initializer.InitializeToFormatting(size, 150, 60);

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                initializer.format.BtnRightFormat(initializer.btn, initializer.btnText, initializer.color);
                int numberOfButtons = initializer.format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point(initializer.format.control.Bounds.Right - initializer.btn.Width - 30,
                                                initializer.format.control.Bounds.Top + (numberOfButtons * initializer.btn.Height));

                Assert.AreEqual(expectedPoint, initializer.btn.Location);
                Assert.AreEqual(initializer.expectedSize, initializer.btn.Size);
                Assert.AreEqual(false, initializer.btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, initializer.btn.FlatStyle);
                Assert.AreEqual(0, initializer.btn.FlatAppearance.BorderSize);
                Assert.AreEqual(initializer.color, initializer.btn.BackColor);
                Assert.AreEqual(initializer.btnText, initializer.btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, initializer.btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, initializer.format.BtnCount);
            }
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnCenterFormat_ValidMapSize_CorrectlySetsButtonValues(int size)
        {
            initializer.InitializeToFormatting(size, 300, 100);

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                initializer.format.BtnCenterFormat(initializer.btn, initializer.btnText, initializer.color);
                int numberOfButtons = initializer.format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point((initializer.format.control.Bounds.Right / 2) - (initializer.btn.Width / 2),
                                                initializer.format.control.Bounds.Top + (numberOfButtons * 60));

                Assert.AreEqual(expectedPoint, initializer.btn.Location);
                Assert.AreEqual(initializer.expectedSize, initializer.btn.Size);
                Assert.AreEqual(false, initializer.btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, initializer.btn.FlatStyle);
                Assert.AreEqual(0, initializer.btn.FlatAppearance.BorderSize);
                Assert.AreEqual(initializer.color, initializer.btn.BackColor);
                Assert.AreEqual(initializer.btnText, initializer.btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, initializer.btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, initializer.format.BtnCount);
            }
        }

        [TestCase(9)]
        [TestCase(11)]
        [TestCase(13)]
        public void BtnLeftFormat_ValidMapSize_CorrectlySetsButtonValues(int size)
        {
            initializer.InitializeToFormatting(size, 200, 75);

            for (int i = 0; i < size * size; i++)
            {
                //number of buttons gets counted up by one when function is called, 
                //therefore -1 after call to get right amount of buttons.
                initializer.format.BtnLeftFormat(initializer.btn, initializer.btnText, initializer.color);
                int numberOfButtons = initializer.format.BtnCount - 1;

                //calculates expected locationpoint.
                Point expectedPoint = new Point(30, initializer.format.control.Bounds.Top + (numberOfButtons * 30));

                Assert.AreEqual(expectedPoint, initializer.btn.Location);
                Assert.AreEqual(initializer.expectedSize, initializer.btn.Size);
                Assert.AreEqual(false, initializer.btn.TabStop);
                Assert.AreEqual(FlatStyle.Flat, initializer.btn.FlatStyle);
                Assert.AreEqual(0, initializer.btn.FlatAppearance.BorderSize);
                Assert.AreEqual(initializer.color, initializer.btn.BackColor);
                Assert.AreEqual(initializer.btnText, initializer.btn.Text);
                Assert.AreEqual(ContentAlignment.MiddleCenter, initializer.btn.TextAlign);
                Assert.AreEqual(numberOfButtons + 1, initializer.format.BtnCount);
            }
        }
    }
}
