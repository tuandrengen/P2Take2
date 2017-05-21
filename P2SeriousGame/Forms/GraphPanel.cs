using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace P2SeriousGame
{
    public partial class GraphPanel : UserControl
    {
        public GraphPanel()
        {
            InitializeComponent();
            chart = new Chart();
        }

        private Chart chart { get; set; }
        public SeriesChartType ChartType { get; set; }
        public int XAxisInterval { get; set; }
        public int YAxisMin { get; set; }
        public int YAxisMax { get; set; }

        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string GraphTitle { get; set; }

        private void GraphPanel_Load(object sender, EventArgs e)
        {

        }

        public void UpdateChartLook()
        {
            Axis xAxis = new Axis
            {
                Interval = XAxisInterval,
                Title = XAxisTitle
            };

            Axis yAxis = new Axis
            {
                Minimum = YAxisMin,
                Maximum = YAxisMax,
                Title = YAxisTitle
            };

            ChartArea chartArea = new ChartArea
            {
                AxisX = xAxis,
                AxisY = yAxis
            };

            Title title = new Title
            {
                Text = GraphTitle,
                Visible = true
            };

            chart.ChartAreas.Add(chartArea);
            chart.Titles.Add(title);

            Controls.Add(chart);
        }

        public void AddSeriesToGraph(IEnumerable<float> roundList)
        {
            Series series = new Series
            {
                //Name = "Some name",
                Color = System.Drawing.Color.Red,
                BorderWidth = 5,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = this.ChartType
            };

            int index = 0;
            foreach (float value in roundList)
            {
                Console.WriteLine(value);
                float xValue = index;
                float yValue = value;
                series.Points.AddXY(xValue, yValue);

                index++;
            }

            chart.Series.Add(series);
        }
    }
}
