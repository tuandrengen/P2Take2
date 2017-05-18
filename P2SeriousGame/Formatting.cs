using System;
using System.Windows.Forms;
using System.Drawing;

namespace P2SeriousGame
{
    public class Formatting : Form
    {
        #region constant vars

        Control control;

        public Formatting(Control control)
        {
            this.control = control;
        }

        public int ButtonWidth;
        public int ButtonHeight;
        public int ButtonHeightOffset => (3 * (ButtonHeight / 4));

        public int SmallBtnSpacing = 30;
        public int LargeBtnSpacing = 60;
        public int BtnCount = 1;

        public int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
        public int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

        //These constants declare the amount of reserved space or margins, where 0.05 equals 5%
        public const double _leftWidthReserved = 0.05;
        public const double _endWidthReserved = 0.12;
        public const double _topHeightReserved = 0.05;
        public const double _bottomHeightReserved = 0.03;

        //The gamescreen variables sets the height and width of the area on the screen where hexagonbutton can be drawn
        public double _gameScreenWidth = Screen.PrimaryScreen.Bounds.Width * (1 - (_leftWidthReserved + _endWidthReserved));
        public double _gameScreenHeight = Screen.PrimaryScreen.Bounds.Height * (1 - (_topHeightReserved + _bottomHeightReserved));

        //Centers the hexagonmap starting placement, if the hexagonmap doesnt fill out the entire gamescreen width
        public double WidthCentering => (_gameScreenWidth - (ButtonWidth * Map.TotalHexagonColumns)) / 2;

        //WidthStart and heightStart sets the starting place for the hexagonmap
        public int WidthStart => (int)((_leftWidthReserved * Screen.PrimaryScreen.Bounds.Width) + WidthCentering);


        public int _heightStart = (int)(_topHeightReserved * Screen.PrimaryScreen.Bounds.Height);
        #endregion

        public void BtnRightFormat(Button btn, string BtnText, Color color)
        {
            btn.Size = new Size(100, 25);
            btn.TabStop = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.Location = new Point(control.Bounds.Right - btn.Width - SmallBtnSpacing, control.Bounds.Top + BtnCount * SmallBtnSpacing);
            btn.Text = BtnText;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            BtnCount++;
        }

        public void BtnCenterFormat(Button btn, string BtnText, Color color)
        {
            btn.Size = new Size(300, 100);
            btn.TabStop = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.Location = new Point(this.Bounds.Right / 2 - btn.Width / 2, this.Bounds.Top + BtnCount * LargeBtnSpacing);
            btn.Text = BtnText;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            BtnCount++;
        }

        public void BtnLeftFormat(Button btn, string BtnText, Color color)
        {
            btn.Size = new Size(200, 75);
            btn.TabStop = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.Location = new Point(SmallBtnSpacing, this.Bounds.Top + BtnCount * SmallBtnSpacing);
            btn.Text = BtnText;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            BtnCount++;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Formatting
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Formatting";
            this.Load += new System.EventHandler(this.Formatting_Load);
            this.ResumeLayout(false);

        }

        private void Formatting_Load(object sender, EventArgs e)
        {

        }
    }
}