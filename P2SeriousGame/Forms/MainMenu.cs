﻿using System;
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

        /// <summary>
        /// Creates the LightSteelBlue window, so it fits every screen, and adds UsernameBox,
        /// StartGameButton, StartAdministratorMenuButton, and CloseMenuButton.
        /// </summary>
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

        public static TextBox nameBox;
        const string defaultNameboxText = "Please insert your name here..";

        /// <summary>
        /// This method gets called in MenuPanel.
        /// This box is made from a Label and a Textbox.
        /// The player replaces 'Please insert your name here..' with their name.
        /// Whenever the player presses play the name
        /// </summary>
        /// <param name="panel"></param>
        private void UsernameBox(Panel panel)
        {
            Label userNameLbl = new Label();
            userNameLbl.Text = "Username:";
            nameBox = new TextBox();
            nameBox.Size = new Size(300, 50);
            nameBox.Text = defaultNameboxText;
            panel.Controls.Add(userNameLbl);
            panel.Controls.Add(nameBox);
        }

        /// <summary>
        /// Creates the 'Start Game' button.
        /// An event has been added for whenever the button gets clicked 
        /// the current window switches to GameWindow.
        /// </summary>
        /// <param name="panel"></param>
        private void StartGameButton(Panel panel)
        {
            Button btnStartGame = new Button();
            formatting.BtnCenterFormat(btnStartGame, "Start Game", Color.GhostWhite);
            btnStartGame.MouseClick += SwitchToGame;
            panel.Controls.Add(btnStartGame);
        }

        /// <summary>
        /// Creates the 'Administrator' button.
        /// An event has been added for whenever the button gets clicked
        /// the current window switches to AdministratorWindow.
        /// </summary>
        /// <param name="panel"></param>
        private void StartAdministratorMenuButton(Panel panel)
        {
            Button btnStartAdministrator = new Button();
            formatting.BtnCenterFormat(btnStartAdministrator, "Administrator", Color.GhostWhite);
            btnStartAdministrator.MouseClick += SwitchToAdministration;
            panel.Controls.Add(btnStartAdministrator);
        }

        /// <summary>
        /// Creates the 'Exit Game' button.
        /// An event has been added for whenever the button gets clicked
        /// the current terminates and thus ends the game.
        /// </summary>
        /// <param name="panel"></param>
        private void CloseMenuButton(Panel panel)
        {
            Button btnCloseGame = new Button();
            formatting.BtnCenterFormat(btnCloseGame, "Exit Game", Color.GhostWhite);
            btnCloseGame.Location = new Point(this.Bounds.Right / 2 - btnCloseGame.Width / 2, this.Bounds.Top + 60);
            btnCloseGame.MouseClick += ExitButtonClick;
            panel.Controls.Add(btnCloseGame);
        }

        /// <summary>
        /// This is the event called in StartGameButton().
        /// In charge of the MenuWindow changes to GameWindow.
        /// </summary>
        private void SwitchToGame(object sender, MouseEventArgs e)
        {
            if (string.Compare(nameBox.Text.ToString(), defaultNameboxText) == 0)
            {
                NoNameInsertedNotification();
            }
            else
            {
                Hide();
                Form gameWindow = new GameForm(mapSize);
                gameWindow.ShowDialog();
                Show();
            }

        }

        private void NoNameInsertedNotification()
        {
            using (Form form = new Form())
            {
                DialogResult dr = MessageBox.Show(" No Name Is Inserted", "", MessageBoxButtons.OK);
            }
        }


        /// <summary>
        /// This is the event called in StartAdministratorMenuButton().
        /// In charge of the MenuWindow changes to AdministratorWindow.
        /// </summary>
        private void SwitchToAdministration(object sender, MouseEventArgs e)
        {
            Hide();
            Form administatorWindow = new AdministratorForm();
            administatorWindow.ShowDialog();
            Show();
        }

        /// <summary>
        /// This is the event called in CloseMenuButton().
        /// In charge of closing down the game.
        /// </summary>
        public void ExitButtonClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
