using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace P2SeriousGame
{
    public partial class AdministratorForm : Form, IStatistic
    {
        Formatting formatting = new Formatting(new Control());
        Panel administratorPanel = new Panel();
        GraphPanel[] graphList = new GraphPanel[4];
        SqlConnection connection = new SqlConnection();
        
        //List<Persons> Persons = new List<Persons>();

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
        {
            DataSource = "p2-avengers.database.windows.net",
            UserID = "tuandrengen",
            Password = "Aouiaom17",
            InitialCatalog = "p2-database"
        }; // https://docs.microsoft.com/en-us/azure/sql-database/sql-database-connect-query-dotnet

        public AdministratorForm()
        {
            InitializeComponent();
            InitializePanels();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void InitializePanels()
        {
            this.Controls.Add(administratorPanel);
            administratorPanel.Width = formatting.ScreenWidth;
            administratorPanel.Height = formatting.ScreenHeight;

            //AddSearchSession();
            CloseMenuButton(administratorPanel);
        }

        private double InitializeGraph(List<float> valueList, int graphNumber)
        {
            GraphPanel graph = graphList[graphNumber];
            graph.AddSeriesToGraph(valueList);

            graph.Size = new Size(300, 400);
            graph.Location = new Point((administratorPanel.Right / 4 - graph.Width / 2) * graphNumber, this.Bounds.Top + 180);

            administratorPanel.Controls.Add(graph);

            return (valueList.Max() * 1.05);
        }

        private void CloseMenuButton(Panel panel)
        {
            Button btnCloseGame = new Button();
            formatting.BtnLeftFormat(btnCloseGame, "Return to menu", Color.GhostWhite);
            btnCloseGame.MouseClick += ReturnToMainMenu;
            panel.Controls.Add(btnCloseGame);
        }

        private void AddSearchSession()
        {
            int screenMidPoint = administratorPanel.Width / 2;

            NumericUpDown sessionInput = new NumericUpDown();
            sessionInput.AutoSize = false;
            sessionInput.Size = new Size(150, 100);
            sessionInput.Location = new Point(screenMidPoint - (250 / 2), Bounds.Top + 20);
            administratorPanel.Controls.Add(sessionInput);
        }

        private void ReturnToMainMenu(object sender, MouseEventArgs e)
        {
            Close();
        }

        public void drawGraph(List<float> valueList, string xAxisTitle, string yAxisTitle, string graphTitle, int xAxisInterval, int yAxisMin, int yAxisMax, SeriesChartType chartType)
        {
            graphList[graphList.Length] = new GraphPanel
            {
                XAxisTitle = xAxisTitle,
                YAxisTitle = yAxisTitle,
                GraphTitle = graphTitle,
                XAxisInterval = xAxisInterval,
                YAxisMin = yAxisMin,
                YAxisMax = yAxisMax,
                ChartType = chartType,
            };
            GraphPanel newGraph = graphList[graphList.Length];

            newGraph.UpdateChartLook();
            InitializeGraph(valueList, graphList.Length);
        }

        public void drawGraph(List<float> valueList, string xAxisTitle, string yAxisTitle, string graphTitle, int xAxisInterval, int yAxisMin, SeriesChartType chartType)
        {
            double yMaxDouble = valueList.Max() * 1.05;
            int yMax = Int32.Parse(yMaxDouble.ToString());
            drawGraph(valueList, xAxisTitle, yAxisTitle, graphTitle, xAxisInterval, yAxisMin, yMax, chartType);
        }

        private void AdministratorForm_Load(object sender, EventArgs e)
        {
            PopulateDataGrid();
            CreatePersonList();
            //PrintList();
        }

        private void PopulateDataGrid()
        {
            string query = "SELECT * FROM Person";

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {

                DataTable PersonTable = new DataTable();
                adapter.Fill(PersonTable);
                this.dataGridView1.DataSource = PersonTable;
                //List<DataRow> PersonList = PersonTable.AsEnumerable().ToList();
            }
        }

        List<DataRow> PersonList = new List<DataRow>(); // test

        /*private void PrintList()
        {
            foreach (var item in PersonList)
            {
                Console.WriteLine(item.Table);
            }
        }*/

        private void CreatePersonList()
        {
            string query = "SELECT * FROM Person";

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable PersonTable = new DataTable();
                adapter.Fill(PersonTable);
                PersonTable.AsEnumerable().ToList(); // filling the list




                // --------------------------------------
                Console.WriteLine(PersonTable.Rows.Count);

                for (int i = 0; i < PersonTable.Rows.Count; i++)
                {
                    Console.WriteLine(PersonTable.Rows[i]["Id"]);
                }

            }
        }

        private void PopulateSession()
        {
            string query = "SELECT a.Id, a.Clicks, a.[AVG Clicks], a.Rounds, a.Losses, a.Wins, a.[Time Used] FROM Game a " +
                "INNER JOIN PersonGameRounds b ON a.Id = b.GameId " +
                "WHERE b.PersonId = @PersonID";

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {

                // Mangler personid fra et sted
                command.Parameters.AddWithValue("@PersonId", listBox1.SelectedValue); // Vi tildeler @RecipeId værdien af Id af den valgte recipe


                DataTable gameTable = new DataTable();
                adapter.Fill(gameTable);

                listBox1.DisplayMember = "Rounds"; // viser kun runder indtil videre...
                listBox1.ValueMember = "Id";
                listBox1.DataSource = gameTable;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show()
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        // When a letter is writing it finds the best match in the database - needs more testing...
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchString = textBox1.Text;

            if (searchString.Length != 0)
            {
                string query = "SELECT * FROM Person " +
                "WHERE Name LIKE '" + searchString + "%'";

                using (connection = new SqlConnection(builder.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable personTable = new DataTable();
                    adapter.Fill(personTable);

                    listBox1.DisplayMember = "Name";
                    listBox1.ValueMember = "Id";
                    listBox1.DataSource = personTable;
                }
            }
        }

        private void ChangeLabelText()
        {
            label1.Text = textBox1.Text;
        }
    }
}
