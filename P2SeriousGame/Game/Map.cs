using System.Windows.Forms;

namespace P2SeriousGame
{
    /// <summary>
    /// Class to contain a grid of HexagonButtons.
    /// </summary>
	public class Map
    {
        //
        /// <summary>
        /// Creates a HexagonButton grid in xSize * ySize, needs a reference to the handler window.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        public Map(GameForm game, int size, IPathfinding path)
        {
            TotalHexagonRows = size;
            TotalHexagonColumns = size;
            this.path = path;
            hexMap = new HexagonButton[TotalHexagonColumns, TotalHexagonRows];
            CreateMap(game);
            FindNeighbours();
        }

        private static int _totalHexagonRows = 0;
        public static int TotalHexagonRows
        {
            get
            {
                return _totalHexagonRows;
            }
            set
            {
                if (value % 2 == 1 && value >= 5)
                {
                    _totalHexagonRows = value;
                }
                else if (value % 2 == 0 && value >= 5)
                {
                    throw new MapDimensionsMustBeOddException(value, "Dimension must be odd");
                }
                else if (value < 5)
                {
                    throw new MapDimensionsMustBeHigherException(value, "Dimension must atleast be 5");
                }
            }
        }

        private static int _totalHexagonColumns = 0;

        public static int TotalHexagonColumns
        {
            get
            {
                return _totalHexagonColumns;
            }
            private set
            {
                if (value % 2 == 1 && value >= 5)
                {
                    _totalHexagonColumns = value;
                }
                else if (value % 2 == 0 && value >= 5)
                {
                    throw new MapDimensionsMustBeOddException(value, "Dimension must be odd");
                }
                else if (value < 5)
                {
                    throw new MapDimensionsMustBeHigherException(value, "Dimension must atleast be 5");
                }
            }
        }

        private HexagonButton _firstButtonInPath;
        static public HexagonButton[,] hexMap;
        
        static public bool newGame = true;

        private int StartMouseXCoordinate => TotalHexagonColumns / 2;
        private int StartMouseYCoordinate => TotalHexagonRows / 2;

        private int mouseXCoordinate;
        public int MouseXCoordinate
        {
            get { return mouseXCoordinate; }
            set { mouseXCoordinate = value; }
        }

        private int mouseYCoordinate;
        public int MouseYCoordinate
        {
            get { return mouseYCoordinate; }
            set { mouseYCoordinate = value; }
        }

        IPathfinding path;

        /// <summary>
        /// Initialises the HexagonButton grid. Flags edge buttons.
        /// </summary>
        /// <param name="game"></param>
        public void CreateMap(GameForm game)
        {
            game.CalculateButtonDimension();
            for (int i = 0; i < _totalHexagonColumns; i++)
            {
                for (int j = 0; j < _totalHexagonRows; j++)
                {
                    bool isEdge = false;
                    if (i == 0 || i == _totalHexagonColumns - 1 || j == 0 || j == _totalHexagonRows - 1)
                    {
                        isEdge = true;
                    }
                    hexMap[i, j] = new HexagonButton(i, j, isEdge);
                    game.DrawButton(hexMap[i, j], this);
                    game.PlaceHexagonButton(hexMap[i, j]);
                }
            }
        }

        /// <summary>
        /// Calculates new route when HexagonButton is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MousePositioner(object sender, MouseEventArgs e)
        {
            // When a HexagonButton is clicked the current mouse position is painted gray.
            // Then calculate the pathing for the mouse.
            if (newGame)
            {
                hexMap[StartMouseXCoordinate, StartMouseYCoordinate].BackColor = System.Drawing.Color.LightGray;
                hexMap[StartMouseXCoordinate, StartMouseYCoordinate].Enabled = true;
                _firstButtonInPath = path.FindPath(hexMap, hexMap[StartMouseXCoordinate, StartMouseYCoordinate]);
                newGame = false;
            }
            else if (!newGame)
            {
                hexMap[MouseXCoordinate, MouseYCoordinate].BackColor = System.Drawing.Color.LightGray;
                hexMap[MouseXCoordinate, MouseYCoordinate].Enabled = true;
                _firstButtonInPath = path.FindPath(hexMap, hexMap[MouseXCoordinate, MouseYCoordinate]);
                if (newGame) // if the game is reset make sure the pathing is reset too.
                {
                    _firstButtonInPath = hexMap[StartMouseXCoordinate, StartMouseYCoordinate];
                }
            }
            // Move the mouse to the next step in its path and paint it Aqua.
            MouseXCoordinate = _firstButtonInPath.XCoordinate;
            MouseYCoordinate = _firstButtonInPath.YCoordinate;
            hexMap[MouseXCoordinate, MouseYCoordinate].BackColor = System.Drawing.Color.Aqua;
            hexMap[MouseXCoordinate, MouseYCoordinate].Enabled = false;
        }

        /// <summary>
        /// Finds the neighbours for each HexagonButton in Map.cs (except of the edge buttons).
        /// </summary>
		public void FindNeighbours()
        {
            for (int i = 0; i < _totalHexagonColumns; i++)
            {
                for (int j = 0; j < _totalHexagonRows; j++)
                {
                    if (!hexMap[i, j].IsEdgeTile)
                    {
                        hexMap[i, j].neighbourList.Add(hexMap[i - 1, j]);
                        hexMap[i, j].neighbourList.Add(hexMap[i + 1, j]);

                        if (j % 2 == 1)
                        {
                            hexMap[i, j].neighbourList.Add(hexMap[i, j - 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i + 1, j - 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i, j + 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i + 1, j + 1]);
                        }

                        else if (j % 2 == 0)
                        {
                            hexMap[i, j].neighbourList.Add(hexMap[i, j - 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i - 1, j - 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i, j + 1]);
                            hexMap[i, j].neighbourList.Add(hexMap[i - 1, j + 1]);
                        }
                    }
                }
            }
        }

        static public void ResetMouse()
        {
            newGame = true;
        }
    }
}
