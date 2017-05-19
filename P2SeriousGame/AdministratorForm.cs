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
        List<float> ValueList = new List<float>();

        // ConnectionString that makes it possible to communicate to the database
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

        // Not used
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
            //drawGraph();

        }

        private void PopulateRounds()
        {
            string query = "SELECT a.Clicks, a.[AVG Clicks], a.Loss, a.Win, a.[Time Used] FROM Rounds a " +
                "INNER JOIN ForeignKeys b ON a.Id = b.RoundsId " +
                "WHERE b.PersonId = " + listBox1.SelectedValue;

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable roundsTable = new DataTable();
                adapter.Fill(roundsTable);
                this.dataGridView1.DataSource = roundsTable;
                
                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();

                /*
                // --------------------------------------
                Console.WriteLine(PersonTable.Rows.Count);

                for (int i = 0; i < PersonTable.Rows.Count; i++)
                {
                    *Console.WriteLine(PersonTable.Rows[i]["Id"]);
                } */
            }
        }

        private List<float> GetValueList()
        {
            List<float> list = new List<float>();


            return list;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //PopulateRounds();
        }

        // When a letter is writing it finds the best match in the database and shows the data in listboxes and datagrid...
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchString = textBox1.Text;

            if (searchString.Length != 0) // Vidst ikke nødvendig, da hver indtastning er en ny omgang i metoden...
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

        private void PopulateSession()
        {
            string query = "SELECT a.Id FROM Session a " +
                "INNER JOIN ForeignKeys b ON a.Id = b.SessionId " +
                "WHERE b.PersonId = @PersonId";

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {

                command.Parameters.AddWithValue("@PersonId", listBox1.SelectedValue);

                DataTable sessionTable = new DataTable();
                adapter.Fill(sessionTable);

                listBox2.DisplayMember = "Id"; 
                listBox2.ValueMember = "Id";
                listBox2.DataSource = sessionTable;
            }
        }

        // Når en person er fundet, men man sletter søgningen, så vil personens session og runde stadig stå der...
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSession(); // filling listbox 2
            PopulateRounds(); // filling datagrid
            drawGraph(ValueList, "xAxisTitle", "yAxisTitle", "graphTitle", 2, 10, SeriesChartType.FastLine);
        }
    }
}
