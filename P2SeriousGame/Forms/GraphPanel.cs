﻿using System;
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

        /// <summary>
        /// Adds IEnumerable<float> to chart graph.
        /// </summary>
        /// <param name="roundList">The list of values to be added to the graph</param>
        public void AddSeriesToGraph(IEnumerable<float> roundList)
        {
            Series series = new Series
            {
                Color = System.Drawing.Color.Red,
                BorderWidth = 5,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = this.ChartType
            };

            int index = 0;
            foreach (float value in roundList)
            {
                float xValue = index;
                float yValue = value;
                series.Points.AddXY(xValue, yValue);

                index++;
            }

            chart.Series.Add(series);
        }

        #region Excess code
        private void GraphPanel_Load(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
