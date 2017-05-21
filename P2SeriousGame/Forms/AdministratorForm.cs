﻿using System;
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
			int alreadyOccupiedWidth = ((administratorPanel.Right / 4) - margin) * ((graphCount - 1) % 2) + margin;
			int height = graphCount > 1 ? Bounds.Top + 150 : Bounds.Top + 100 + graph.Height;
			Console.WriteLine($"Width: {administratorPanel.Width}");
			Console.WriteLine($"Positionx: {alreadyOccupiedWidth}");
			Console.WriteLine($"Positiony: {height}");

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
        /// 
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="graphTitle"></param>
        /// <param name="xAxisInterval"></param>
        /// <param name="yAxisMin"></param>
        /// <param name="yAxisMax"></param>
        /// <param name="chartType"></param>
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
			graphCount++;

            newGraph.UpdateChartLook();
            InitializeGraph(valueList);
        }

        public void drawGraph(List<float> valueList, string xAxisTitle, string yAxisTitle, string graphTitle, int xAxisInterval, int yAxisMin, SeriesChartType chartType)
        {
            double yMaxDouble = GetMaxValue(valueList) + 1 * 1.7;
			int yMax = Convert.ToInt32(yMaxDouble);
            drawGraph(valueList, xAxisTitle, yAxisTitle, graphTitle, xAxisInterval, yAxisMin, yMax, chartType);
        }

        /// <summary>
        /// Finds the max value in the list.
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
            string query = "SELECT r.[Round Number], r.[AVG Clicks], r.Loss, r.Win, r.[Time Used] FROM Rounds r " +
                "INNER JOIN ForeignKeys fk ON fk.RoundsId = r.RoundID " +
                "WHERE fk.PersonId = " + listBox1.SelectedValue;

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable roundsTable = new DataTable();
                adapter.Fill(roundsTable);
                this.dataGridView1.DataSource = roundsTable;

				graphCount = 0;
                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["AVG Clicks"])).ToList();
                drawGraph(ValueList, "Rounds", "AVG Clicks", "AVG Clicks over Rounds", 1, 0, SeriesChartType.FastLine);

                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();
                drawGraph(ValueList, "Rounds", "Time Used", "Time Used over Rounds", 1, 0, SeriesChartType.FastLine);

				ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["Time Used"])).ToList();
				drawGraph(ValueList, "Title", "Title", "Title", 1, 0, SeriesChartType.FastLine);

				// not good yet gives nothing
				/*
                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["AVG Clicks"])).ToList();
                drawGraph(ValueList, "Rounds", "AVG Clicks", "AVG Clicks over Rounds", 1, 0, SeriesChartType.FastLine);

                ValueList = (from row in roundsTable.AsEnumerable() select Convert.ToSingle(row["AVG Clicks"])).ToList();
                drawGraph(ValueList, "Rounds", "AVG Clicks", "AVG Clicks over Rounds", 1, 0, SeriesChartType.FastLine);
                */

			}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //PopulateRounds();
        }

        // When a letter is writing it finds the best match in the database and shows the data in listboxes and datagrid...
        /// <summary>
        /// When there is input it will search the database and if succesfull, then the person will be found.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                PopulateSession(); // filling datagrid 2
                PopulateRounds(); // filling datagrid 1
            }
            else
            {
                listBox1.DataSource = null;
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
            }
        }

        
        private void PopulateSession()
        {
            string query = "SELECT s.Rounds, s.Clicks, s.[AVG Clicks], s.Losses, s.Wins, s.[Time Used]  FROM [Session] s " +
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

        // Når en person er fundet, men man sletter søgningen, så vil personens session og runde stadig stå der...
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            
        }

		private void AdministratorForm_Load(object sender, EventArgs e)
		{

		}
	}
}
