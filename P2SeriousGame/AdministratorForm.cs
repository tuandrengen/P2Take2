using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        }
    }
}
