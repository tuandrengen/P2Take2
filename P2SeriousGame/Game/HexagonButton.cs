using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2SeriousGame
{
    public class HexagonButton : Button
    {
       
        /// <summary>
        /// Takes posistion in a coordinate system and if hexagun button is an edge button.        
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="isEdgeTile"></param>
        public HexagonButton(int xCoordinate, int yCoordinate, bool isEdgeTile)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
			_isEdgeTile = isEdgeTile;
        }

		private bool _visited = false;
		public bool Visited
		{
			get { return _visited; }
			set { _visited = value; }
		}


		private bool _passable = true;
        public bool Passable
        {
            get { return _passable; }
            set
            {
                if (value == true)
                {
                    BackColor = System.Drawing.Color.LightGray;
                    _passable = value;
                }
                else
                {
                    BackColor = System.Drawing.Color.Red;
                    _passable = value;
                }
            }
        }

        private int _xCoordinate;
        public int XCoordinate
        {
            get { return _xCoordinate; }
            private set
            {
                if (value >= 0)
                    _xCoordinate = value;
                else
                    throw new MustBePositiveException("XCoordinate value is not valid");
            }
        }

        private int _yCoordinate;
        public int YCoordinate
        {
            get { return _yCoordinate; }
            private set
            {
                if (value >= 0)
                    _yCoordinate = value;
                else
                    throw new MustBePositiveException("YCoordinate value is not valid");
            }
        }

		private bool _isEdgeTile;

		public bool IsEdgeTile
		{
			get { return _isEdgeTile; }
		}

        public void HexClicked(object sender, MouseEventArgs e)
        {
			//Console.WriteLine($"You pressed on tile: ({XCoordinate}, {YCoordinate}) {IsEdgeTile}");
            HexagonButton sender_Button = sender as HexagonButton;
            sender_Button.Enabled = false;
            sender_Button.Passable = false;
            //PrintNeighbours();
        }

		public HexagonButton parent;
		public List<HexagonButton> neighbourList = new List<HexagonButton>();

        public int CostToStart;

        private void PrintNeighbours()
        {
            foreach (HexagonButton hex in neighbourList)
            {
                Console.WriteLine($"{ hex.XCoordinate}, { hex.YCoordinate} { hex.IsEdgeTile}");
            }
        }
    }
}