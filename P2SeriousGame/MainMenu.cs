using System;
using System.Drawing;
using System.Windows.Forms;

namespace P2SeriousGame
{
    public partial class MainMenu : Form
    {
        private FlowLayoutPanel menuPanel = new FlowLayoutPanel();
        private Database SQL = new Database();
        Formatting formatting;
        int mapSize;

        public MainMenu(int mapSize)
        {
            formatting = new Formatting(this);
            InitializeComponent();
            this.mapSize = mapSize;
        }

        private void InitializeMenues()
        {
            MenuPanel();
        }

        public void DrawWindow(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeMenues();
        }

        private void MenuPanel()
        {
            this.Controls.Add(menuPanel);
            menuPanel.Width = formatting.ScreenWidth;
            menuPanel.Height = formatting.ScreenHeight;
            menuPanel.BackColor = Color.LightSteelBlue;
            menuPanel.FlowDirection = FlowDirection.TopDown;
            menuPanel.Padding = new Padding(Size.Width / 2 - 150, 25, Size.Width / 2 + 150, 25);
            UsernameBox(menuPanel);
            StartGameButton(menuPanel);
            StartAdministratorMenuButton(menuPanel);
            CloseMenuButton(menuPanel);
        }

        private void UsernameBox(Panel panel)
        {
            Label userNameLbl = new Label();
            userNameLbl.Text = "Brugernavn:";
            TextBox nameBox = new TextBox();
            nameBox.Size = new Size(300, 50);
            panel.Controls.Add(userNameLbl);
            panel.Controls.Add(nameBox);

        }

        private void StartGameButton(Panel panel)
        {
            Button btnStartGame = new Button();
            formatting.BtnCenterFormat(btnStartGame, "Start Game", Color.GhostWhite);
            btnStartGame.MouseClick += SwitchToGame;
            panel.Controls.Add(btnStartGame);
        }

        private void StartAdministratorMenuButton(Panel panel)
        {
            Button btnStartAdministrator = new Button();
            formatting.BtnCenterFormat(btnStartAdministrator, "Administrator", Color.GhostWhite);
            btnStartAdministrator.MouseClick += SwitchToAdministration;
            panel.Controls.Add(btnStartAdministrator);
        }

        private void CloseMenuButton(Panel panel)
        {
            Button btnCloseGame = new Button();
            formatting.BtnCenterFormat(btnCloseGame, "Exit Game", Color.GhostWhite);
            btnCloseGame.Location = new Point(this.Bounds.Right / 2 - btnCloseGame.Width / 2, this.Bounds.Top + 60);
            btnCloseGame.MouseClick += ExitButtonClick;
            panel.Controls.Add(btnCloseGame);
        }

        private void SwitchToGame(object sender, MouseEventArgs e)
        {
            Hide();
            Form gameWindow = new GameForm(mapSize);
            gameWindow.ShowDialog();
            Show();
        }

        private void SwitchToAdministration(object sender, MouseEventArgs e)
        {
            Hide();
            Form administatorWindow = new AdministratorForm();
            administatorWindow.ShowDialog();
            Show();
        }

        public void ExitButtonClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void ResetButtonClick(object sender, MouseEventArgs e)
        {
            foreach (HexagonButton hex in Map.hexMap)
            {
                hex.Visited = false;
                hex.Passable = true;
                hex.Enabled = true;
                hex.BackColor = System.Drawing.Color.LightGray;
                PlaceHexagonButton(hex);
            }
            Map.ResetMouse();
        }

        public void PlaceHexagonButton(HexagonButton button)
        {
            //For at farve midten før man har klikket på skærmen.
            if (button.XCoordinate == Map.TotalHexagonColumns / 2 && button.YCoordinate == Map.TotalHexagonRows / 2)
            {
                button.BackColor = System.Drawing.Color.Aqua;
                button.Enabled = false;
            }

            button.Left = CalculateButtonWidthOffset(button.XCoordinate, button.YCoordinate);
            button.Top = CalculateButtonHeightOffset(button.YCoordinate);
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
            //Gives every second button an offset to make the grid
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

    }
}
