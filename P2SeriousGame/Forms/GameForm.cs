﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace P2SeriousGame
{
    public partial class GameForm : Form
    {
        Panel gamePanel = new Panel();
        Database SQL = new Database();
        Map FirstLevel;
        IPathfinding path;

        Formatting formatting;

        /// <summary>
        /// Creates the Game Window
        /// </summary>
        /// <param name="mapSize"></param>
        public GameForm(int mapSize)
        {
            formatting = new Formatting(this);
            path = new Pathfinding(this);
            FirstLevel = new Map(this, mapSize, path);
            InitializeComponent();
            SQL.StartStopwatch();
        }

        /// <summary>
        /// Sets Game Panel size and buttons inside.
        /// </summary>
        private void InitializePanels()
        {
            this.Controls.Add(gamePanel);
            gamePanel.Width = formatting.ScreenWidth;
            gamePanel.Height = formatting.ScreenHeight;
            gamePanel.Visible = true;
            AddExitButton(gamePanel);
            AddResetButton(gamePanel);
        }

        /// <summary>
        /// Finds button dimensions after screen height or width.
        /// </summary>
        public void CalculateButtonDimension()
        {
            CalculateButtonDimensionBasedOnScreenHeight();

            /// Does the calculated width fit the screen width, if not then calculate height and width based on screen width.
            if ((formatting.ButtonWidth * Map.TotalHexagonColumns) > formatting._gameScreenWidth)
                CalculateButtonDimensionBasedOnScreenWidth();
        }

        /// <summary>
        /// Calculates the button size based on screen height.
        /// </summary>
        private void CalculateButtonDimensionBasedOnScreenHeight()
        {
            double rowHeight;
            double hexagonRows = Map.TotalHexagonRows;
            const double evenRowsToHeight = 0.75;

            /// The height to width ratio for a pointy top regulare hexagon.
            double heightToWidth = System.Math.Sqrt(3) / 2;

            /// These series of if-else calculates the height of one button, determined by the number of rows and the screen height.
            if (hexagonRows == 1)
                formatting.ButtonHeight = (int)(formatting._gameScreenHeight / hexagonRows);
            else if (hexagonRows % 2 == 0)
            {
                rowHeight = (hexagonRows * evenRowsToHeight) + 0.25;
                formatting.ButtonHeight = (int)(formatting._gameScreenHeight / rowHeight);
            }
            else if (hexagonRows % 2 == 1 && hexagonRows > 1)
            {
                rowHeight = ((hexagonRows - 1) / 4) + ((hexagonRows + 1) / 2);
                formatting.ButtonHeight = (int)(formatting._gameScreenHeight / rowHeight);
            }

            /// We calculate the width by multiplying height to width ratio.
            formatting.ButtonWidth = (int)((formatting.ButtonHeight * heightToWidth));
        }

        /// <summary>
        /// Calculates the button size based on scren width.
        /// </summary>
        private void CalculateButtonDimensionBasedOnScreenWidth()
        {
            /// The width to height ratio for a pointy top regulare hexagon.
            double widthToHeight = System.Math.Sqrt(3) * ((double)2 / 3);

            double buttonWidthTemp;

            /// We calculate the button width by dividing the screen width with number of columns + 0.5 (because we have an offset).
            buttonWidthTemp = (int)(formatting._gameScreenWidth / (Map.TotalHexagonColumns + 0.5));

            /// We calculate the height by multiplying width to height ratio
            formatting.ButtonHeight = (int)(buttonWidthTemp * widthToHeight);

            /// Now we do not need the buttonWidthTemp with precision, so we typecast the double to an int.
            formatting.ButtonWidth = (int)buttonWidthTemp;
        }

        /// <summary>
        /// Sets propeties of Game Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawWindow(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializePanels();
        }

        /// <summary>
        /// Initialises and draws a hexagon button, 
        /// and adds a click event which calculates a new route when an HexButton is clicked.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="map"></param>
        public void DrawButton(HexagonButton button, Map map)
        {
            button.Size = new Size((int)(ConvertPointToPixel(formatting.ButtonHeight)), (int)(ConvertPointToPixel(formatting.ButtonWidth)));
            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.LightGray;
            button.Paint += ButtonPainter;
            button.MouseClick += button.HexClicked;
            button.MouseClick += HexClickedCounter;
            button.MouseClick += map.MousePositioner;
            gamePanel.Controls.Add(button);
        }

        public static float hexClickedRound;

        /// <summary>
        /// Increments hexClickedRound for counting the amount of clicks the player did this game session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HexClickedCounter(object sender, MouseEventArgs e)
        {
            hexClickedRound += 1;
        }

        /// <summary>
        /// Places HexagonButtons in GameForm based on the coordinates assigned in the button.
        /// </summary>
        /// <param name="button"></param>
        public void PlaceHexagonButton(HexagonButton button)
        {
            /// Colors the center of the map
            if (button.XCoordinate == Map.TotalHexagonColumns / 2 && button.YCoordinate == Map.TotalHexagonRows / 2)
            {
                button.BackColor = System.Drawing.Color.Aqua;
                button.Enabled = false;
            }

            button.Left = CalculateButtonWidthOffset(button.XCoordinate, button.YCoordinate);
            button.Top = CalculateButtonHeightOffset(button.YCoordinate);
        }

        /// <summary>
        /// Calculates the points in a hexagon and makes it a button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonPainter(object sender, PaintEventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath buttonPath =
            new System.Drawing.Drawing2D.GraphicsPath();
            Button hexagonButton = sender as Button;

            System.Drawing.Rectangle newRectangle = hexagonButton.ClientRectangle;
            e.Graphics.DrawPolygon(Pens.Black, Math.GetPoints(formatting.ButtonHeight, formatting.ButtonWidth));

            /// Create a hexagon within the new rectangle.
            buttonPath.AddPolygon(Math.GetPoints(formatting.ButtonHeight, formatting.ButtonWidth));
            /// Hexagon region.
            hexagonButton.Region = new Region(buttonPath);
        }

        /// <summary>
        /// Creates a button, which resets the game
        /// </summary>
        /// <param name="panel"></param>
        private void AddResetButton(Panel panel)
        {
            Button ResetButton = new Button();
            formatting.BtnRightFormat(ResetButton, "Reset Game", Color.Red);
            ResetButton.MouseClick += ResetButtonClick;
            panel.Controls.Add(ResetButton);
        }

        /// <summary>
        /// Creates a button, which resets the game
        /// </summary>
        /// <param name="panel"></param>
        private void AddExitButton(Panel panel)
        {
            Button ExitButton = new Button();
            formatting.BtnRightFormat(ExitButton, "Return to menu", Color.LightGray);
            ExitButton.MouseClick += ResetButtonClick;
            ExitButton.MouseClick += ReturnToMainMenu;
            panel.Controls.Add(ExitButton);
        }

        /// <summary>
        /// Converts a coordinate into a position in a hexgrid.
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        private int CalculateButtonWidthOffset(int xCoordinate, int yCoordinate)
        {
            int width = formatting.WidthStart;
            width += (xCoordinate * formatting.ButtonWidth);
            
            /// Gives every second button an offset to make the grid.
            if (yCoordinate % 2 == 1)
            {
                width += formatting.ButtonWidth / 2;
            }
            return width;
        }

        /// <summary>
        /// Converts a coordinate into a position in a hexgrid.
        /// </summary>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        private int CalculateButtonHeightOffset(int yCoordinate)
        {
            int height = formatting._heightStart;

            height += (yCoordinate * formatting.ButtonHeightOffset);

            return height;
        }
               
        /// <summary>
        /// MouseClickEvent for resetting the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ResetButtonClick(object sender, MouseEventArgs e)
        {
            ResetHex();
            SQL.ResetGameToListFromReset();
        }

        /// <summary>
        /// Reset the game if the player wins or loses.
        /// </summary>
        public void ResetByWinningOrLosing()
        {
            ResetHex();
            SQL.ResetGameToList();
        }

        /// <summary>
        /// Resets the Game.
        /// </summary>
        private void ResetHex()
        {
            foreach (HexagonButton hex in Map.hexMap)
            {
                hex.Visited = false;
                hex.Passable = true;
                hex.Enabled = true;
                PlaceHexagonButton(hex);
            }
            Map.ResetMouse();
        }

        /// We assume that there is 72 points per inch and 96 pixels per inch.
        private double ConvertPointToPixel(double point)
        {
            return point * 96 / 72;
        }

        /// <summary>
        /// MouseClickEvent for closing the GameForm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToMainMenu(object sender, MouseEventArgs e)
        {
            SQL.ExitGameToDatabase();
            Close();
        }


        /// <summary>
        /// Opens a new window with the message that you won the game.
        /// </summary>
        public void WinNotification()
        {
            using (Form form = new Form())
            {
                DialogResult dr = MessageBox.Show(" You won the round.", "Round notification", MessageBoxButtons.OK);
                if (dr == DialogResult.OK)
                {
                    Pathfinding.gameRoundWin = true;
                    ResetByWinningOrLosing();
                }
            }
        }

        /// <summary>
        /// Opens a new window with the message that you have lost the game.
        /// </summary>
        public void LoseNotification()
        {
            using (Form form = new Form())
            {
                DialogResult dr = MessageBox.Show(" You lose the round.", "Round notification", MessageBoxButtons.OK);
                if (dr == DialogResult.OK)
                {
                    Pathfinding.gameRoundWin = false;
                    ResetByWinningOrLosing();
                }
            }
        }
    }
}

