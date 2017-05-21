using System;
using System.Windows.Forms;

namespace P2SeriousGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainMenu = new MainMenu(13);

            Application.Run(mainMenu);


            // MISSING
            // AdministratorForm Graphs!
        }
    }
}
