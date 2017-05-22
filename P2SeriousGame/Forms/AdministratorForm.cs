using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace P2SeriousGame
{
    public partial class AdministratorForm : Form, IStatistic
    {
        Formatting formatting = new Formatting(new Control());
        Panel administratorPanel = new Panel();
        GraphPanel[] graphList = new GraphPanel[4];
		public int graphCount = 0;
        SqlConnection connection = new SqlConnection();
        List<float> ValueList = new List<float>();
		bool firstRun = true;

        /// <summary>
        /// ConnectionString that makes it possible to communicate to the database
        /// </summary>
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

        /// <summary>
        /// Sets the size of the administratorPanel, and adds CloseMenuButton.
        /// </summary>
        private void InitializePanels()
        {
            this.Controls.Add(administratorPanel);
            administratorPanel.Width = formatting.ScreenWidth;
            administratorPanel.Height = formatting.ScreenHeight;
            CloseMenuButton(administratorPanel);
        }

        /// <summary>
        /// Creates a graph form a List<float>, and addse it to administratorPanel.
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        private double InitializeGraph(List<float> valueList)
        {
            GraphPanel graph = graphList[graphCount - 1];
            graph.AddSeriesToGraph(valueList);

			int margin = 50;

            graph.Size = new Size(300, 400);

			// Places the graphs next to each other
			int alreadyOccupiedWidth = ((administratorPanel.Right / 4) - margin) * ((graphCount - 1) % 2) + margin;

			// Stacks the graphs, with the first two graphs on top, and the next two below.
			int height = graphCount > 2 ? Bounds.Top + 150 : Bounds.Top + 100 + graph.Height;

			Console.WriteLine($"x: {alreadyOccupiedWidth}. y: {height}");

			graph.Location = new Point(alreadyOccupiedWidth, height);

            administratorPanel.Controls.Add(graph);

            return (GetMaxValue(valueList) * 1.7);
        }

        /// <summary>
        /// Creates and adds CloseMenuButton to administratorPanel.
        /// </summary>
        /// <param name="panel"></param>
        private void CloseMenuButton(Panel panel)
        {
            Button btnCloseGame = new Button();
            formatting.BtnLeftFormat(btnCloseGame, "Return to menu", Color.GhostWhite);
            btnCloseGame.MouseClick += ReturnToMainMenu;
            panel.Controls.Add(btnCloseGame);
        }

        /// <summary>
        /// Close form click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToMainMenu(object sender, MouseEventArgs e)
        {
            Close();
        }

		/// <summary>
		/// Draws a graph on the administratorform
		/// </summary>
		/// <param name="valueList">The list values, plotted on the y-axis</param>
		/// <param name="xAxisTitle"></param>
		/// <param name="yAxisTitle"></param>
		/// <param name="graphTitle"></param>
		/// <param name="xAxisInterval">The interval that should be shown between the values on the x-axis</param>
		/// <param name="yAxisMin">The minimum value of the y-axis</param>
		/// <param name="yAxisMax">The maximum value of the y-axis</param>
		/// <param name="chartType">The type of chart that should be displayed</param>
		public void drawGraph(List<float> valueList, string xAxisTitle, string yAxisTitle, string graphTitle, int xAxisInterval, int yAxisMin, int yAxisMax, SeriesChartType chartType)
        {
			GraphPanel newGraph = new GraphPanel
            {
                XAxisTitle = xAxisTitle,
                YAxisTitle = yAxisTitle,
                GraphTitle = graphTitle,
                XAxisInterval = xAxisInterval,
                YAxisMin = yAxisMin,
                YAxisMax = yAxisMax,
                ChartType = chartType,
            };

			graphList[graphCount] = newGraph;

			if (graphCount == 4)
				graphCount = 0;
			else
				graphCount++;

            newGraph.UpdateChartLook();
            InitializeGraph(valueList);
        }

		/// <summary>
		/// Draws a graph on the administratorform. 
		/// </summary>
		/// <param name="valueList">The list values, plotted on the y-axis</param>
		/// <param name="xAxisTitle"></param>
		/// <param name="yAxisTitle"></param>
		/// <param name="graphTitle"></param>
		/// <param name="xAxisInterval">The interval that should be shown between the values on the x-axis</param>
		/// <param name="yAxisMin">The maximum value of the y-axis</param>
		/// <param name="chartType">The type of chart that should be displayed</param>
		public void drawGraph(List<float> valueList, string xAxisTitle, string yAxisTitle, string graphTitle, int xAxisInterval, int yAxisMin, SeriesChartType chartType)
        {
            double yMaxDouble = GetMaxValue(valueList) + 1 * 1.7;
			int yMax = Convert.ToInt32(yMaxDouble);
            drawGraph(valueList, xAxisTitle, yAxisTitle, graphTitle, xAxisInterval, yAxisMin, yMax, chartType);
        }

        /// <summary>
        /// Finds the max value in a list.
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        private float GetMaxValue(List<float> valueList)
        {
            float maxValue = 0;
            foreach (var item in valueList)
            {
                if (item > maxValue)
                {
                    maxValue = item;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Populates the chosen listbox with data from the Rounds table and adds graphs accordingly
        /// </summary>
        private void PopulateRounds()
        {
            string query = "SELECT r.[Round Number], r.[Avg. Clicks Per Minute], r.Loss, r.Win, r.[Time Used] FROM Rounds r " +
                "INNER JOIN ForeignKeys fk ON fk.RoundsId = r.RoundID " +
                "WHERE fk.PersonId = " + listBox1.SelectedValue;

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable roundsTable = new DataTable();
                adapter.Fill(roundsTable);
                this.dataGridView1.DataSource = roundsTable;

				// Instantiates the graphs that should be shown.
				graphCount = 0;
				if(!firstRun)
				{
					foreach (var item in graphList)
					{
						item.Visible = false;
					}
				}

				firstRun = false;

				ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Avg. Clicks Per Minute"])).ToList();
                drawGraph(ValueList, "Rounds", "Avg. Clicks Per Minute", "Avg. Clicks over Rounds", 1, 0, SeriesChartType.FastLine);

                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();
                drawGraph(ValueList, "Rounds", "Time Used", "Time Used over Rounds", 1, 0, SeriesChartType.FastLine);

				ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();
				drawGraph(ValueList, "Rounds", "Time Used", "Time Used over Rounds", 1, 0, SeriesChartType.FastLine);

				ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();
				drawGraph(ValueList, "Rounds", "Time Used", "Time Used over Rounds", 1, 0, SeriesChartType.FastLine);
			}
        }

        /// <summary>
        /// When there is input it will then search the database for the desired person,
        /// if succesfull, then listBox1, DataGrid1 and DataGrid 2 will be populated with relevant data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchString = textBox1.Text; // Every letter input equals Text

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

                    if (personTable.Rows.Count != 0)
                    {
                        PopulateSession();
                        PopulateRounds();
                    }
                    else
                    {
                        listBox1.DataSource = null;
                        dataGridView1.DataSource = null;
                        dataGridView2.DataSource = null;
                    }
                }
            }
            else
            {
                listBox1.DataSource = null;
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
            }
        }

        /// <summary>
        /// Populates the second datagrid for a Session containing data of all Rounds of the given ID
        /// </summary>
        private void PopulateSession()
        {
            string query = "SELECT s.Rounds, s.Clicks, s.[Avg. Clicks Per Minute], s.Losses, s.Wins, s.[Time Used]  FROM [Session] s " +
                "WHERE  s.Id = " + listBox1.SelectedValue;

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable sessionTable = new DataTable();
                adapter.Fill(sessionTable);
                this.dataGridView2.DataSource = sessionTable;
            }
        }

        #region Excess code
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {}
		private void AdministratorForm_Load(object sender, EventArgs e)
		{}
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}
        private void label1_Click(object sender, EventArgs e)
        {}
        #endregion
    }
}
