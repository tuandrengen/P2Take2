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
        [TestCase(11)]
        public void BtnRightFormat_None_CorrectlySetsButtonValues(int size)
        {
            GameForm window = new GameForm(size);
            Formatting format = new Formatting(window);
            Control control = new Control();
            Button btn = new Button();
            string btnText = "Empty";
            Color color = Color.Red;
            Size expectedSize = new Size(150, 60);
            Point expectedPoint = new Point((window.Bounds.Right) - (btn.Width) - 30, window.Bounds.Top + (format.BtnCount * btn.Height));
            format.BtnRightFormat(btn, btnText, color);

           

            Assert.AreEqual(expectedSize, btn.Size);
            Assert.AreEqual(false, btn.TabStop);
            Assert.AreEqual(FlatStyle.Flat, btn.FlatStyle);
            Assert.AreEqual(0, btn.FlatAppearance.BorderSize);
            Assert.AreEqual(color, btn.BackColor);
            Assert.AreEqual(expectedPoint, btn.Location);
            Assert.AreEqual(btnText, btn.Text);
            Assert.AreEqual(ContentAlignment.MiddleCenter, btn.TextAlign);
            

        }
    }
}
