using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Lab7_Winforms
{
    public partial class Form1 : Form
    {
        private double xAxisMin = 0;
        private double xAxisMax = 5;
        private double yAxisMin = -10;
        private double yAxisMax = 10;

        private bool isDragging;
        private Point lastMousePosition;

        private VScrollBar verticalScrollBar;
        public Form1()
        {
            InitializeComponent();
            CreateCharts();
            AddVerticalScrollBar();
        }

        private void CreateCharts()
        {
            double t = 0;  // initial time
            double y = 1;  // initial value of y

            double h = 0.1;  // step size
            double tLimit = 5;  // upper limit of the interval

            // Create chart objects for each variation
            var chartAB = new LiveCharts.WinForms.CartesianChart();
            var chartCD = new LiveCharts.WinForms.CartesianChart();

            panelAB.Width = 1400;
            panelCD.Width = 1400;
            panelAB.Height = 300;
            panelCD.Height = 300;
            chartAB.Width = 1400;
            chartAB.Height = 300;
            chartCD.Width = 1400;
            chartCD.Height = 300;


            // Create series for each variation
            var seriesA = new LineSeries { Title = "Solution (a)", Values = new ChartValues<ObservablePoint>() };
            var seriesB = new LineSeries { Title = "Solution (b)", Values = new ChartValues<ObservablePoint>() };
            var seriesC = new LineSeries { Title = "Solution (c)", Values = new ChartValues<ObservablePoint>() };
            var seriesD = new LineSeries { Title = "Solution (d)", Values = new ChartValues<ObservablePoint>() };
            var seriesCorrectAB = new LineSeries { Title = "Correct solution", Values = new ChartValues<ObservablePoint>() };
            var seriesCorrectCD = new LineSeries { Title = "Correct solution", Values = new ChartValues<ObservablePoint>() };


            // Calculate solutions for each variation and add data points to the corresponding series
            CalculateMilneSimpson(t, y, h, tLimit, seriesA, false);
            //CalculateMilneSimpson(t, y, h, tLimit, seriesA, false);
            CalculateMilneSimpsonWithControlParameter(t, y, h, tLimit, seriesB, false);

            h = 0.2;  // step size doubled

            CalculateMilneSimpson(t, y, h, tLimit, seriesC, true);
            CalculateMilneSimpsonWithControlParameter(t, y, h, tLimit, seriesD, true);
            DisplayCorrectChart(tLimit, seriesCorrectAB);
            DisplayCorrectChart(tLimit, seriesCorrectCD);

            // Add series to the corresponding charts
            chartAB.Series.Add(seriesA);
            chartAB.Series.Add(seriesB);
            chartAB.Series.Add(seriesCorrectAB);
            chartCD.Series.Add(seriesC);
            chartCD.Series.Add(seriesD);
            chartCD.Series.Add(seriesCorrectCD);
            chartAB.MouseWheel += Chart_MouseWheel;
            chartCD.MouseWheel += Chart_MouseWheel;
            chartAB.MouseDown += Chart_MouseDown;
            chartAB.MouseMove += Chart_MouseMove;
            chartAB.MouseUp += Chart_MouseUp;
            chartCD.MouseDown += Chart_MouseDown;
            chartCD.MouseMove += Chart_MouseMove;
            chartCD.MouseUp += Chart_MouseUp;


            // Set the charts as the content of the corresponding panel controls
            panelAB.Controls.Add(chartAB);
            panelCD.Controls.Add(chartCD);
        }

        private void DisplayCorrectChart(double tLimit, LineSeries series)
        {
            for (double i = 0; i < tLimit; i += 0.1)
            {
                double tmp = -Math.Exp(-i) + i * i - 2 * i + 2;
                series.Values.Add(new ObservablePoint(i, tmp));
            }
        }

        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (LiveCharts.WinForms.CartesianChart)sender;

            double zoomFactor = 1.2;
            double zoomSpeed = 0.02;

            double xRange = xAxisMax - xAxisMin;
            double yRange = yAxisMax - yAxisMin;

            if (e.Delta > 0)  // Scrolling up
            {
                // Zoom in
                double mouseX = e.Location.X - chart.Location.X;
                double mouseY = e.Location.Y - chart.Location.Y;

                double zoomedXRange = xRange / zoomFactor;
                double zoomedYRange = yRange / zoomFactor;

                double zoomedXAxisMin = xAxisMin + (xRange - zoomedXRange) * mouseX / chart.Width;
                double zoomedXAxisMax = zoomedXAxisMin + zoomedXRange;

                double zoomedYAxisMin = yAxisMin + (yRange - zoomedYRange) * mouseY / chart.Height;
                double zoomedYAxisMax = zoomedYAxisMin + zoomedYRange;

                xAxisMin = zoomedXAxisMin;
                xAxisMax = zoomedXAxisMax;
                yAxisMin = zoomedYAxisMin;
                yAxisMax = zoomedYAxisMax;
            }
            else if (e.Delta < 0)  // Scrolling down
            {
                // Zoom out
                double zoomedXRange = xRange * zoomFactor;
                double zoomedYRange = yRange * zoomFactor;

                double zoomedXAxisMin = xAxisMin - (zoomedXRange - xRange) * zoomSpeed;
                double zoomedXAxisMax = zoomedXAxisMin + zoomedXRange;

                double zoomedYAxisMin = yAxisMin - (zoomedYRange - yRange) * zoomSpeed;
                double zoomedYAxisMax = zoomedYAxisMin + zoomedYRange;

                xAxisMin = zoomedXAxisMin;
                xAxisMax = zoomedXAxisMax;
                yAxisMin = zoomedYAxisMin;
                yAxisMax = zoomedYAxisMax;
            }

            // Update the chart's axis ranges
            chart.AxisX[0].MinValue = xAxisMin;
            chart.AxisX[0].MaxValue = xAxisMax;
            chart.AxisY[0].MinValue = yAxisMin;
            chart.AxisY[0].MaxValue = yAxisMax;
        }

        private void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var chart = (LiveCharts.WinForms.CartesianChart)sender;
                int deltaX = e.Location.X - lastMousePosition.X;
                int deltaY = e.Location.Y - lastMousePosition.Y;

                double xRange = xAxisMax - xAxisMin;
                double yRange = yAxisMax - yAxisMin;

                double deltaXValue = deltaX * xRange / chart.Width;
                double deltaYValue = deltaY * yRange / chart.Height;

                xAxisMin -= deltaXValue;
                xAxisMax -= deltaXValue;
                yAxisMin += deltaYValue;
                yAxisMax += deltaYValue;

                chart.AxisX[0].MinValue = xAxisMin;
                chart.AxisX[0].MaxValue = xAxisMax;
                chart.AxisY[0].MinValue = yAxisMin;
                chart.AxisY[0].MaxValue = yAxisMax;

                lastMousePosition = e.Location;
            }
        }

        private void Chart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        static double Function(double t, double y)
        {
            return t * t - y;  // define the differential equation y' = t^2 - y
        }

        static double GunnMethod(double t, double y, double h)
        {
            double k1 = Function(t, y);
            double k2 = Function(t + h / 2, y + h / 2 * k1);
            double k3 = Function(t + h / 2, y + h / 2 * k2);
            double k4 = Function(t + h, y + h * k3);

            return y + h / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
        }

        static void CalculateMilneSimpson(double t, double y, double h, double tLimit, LineSeries series, bool doubledStep)
        {
            while (t <= tLimit)
            {
                double yGunn = GunnMethod(t, y, h);  // calculate initial data using Gunn's method

                // Milne-Simpson forecast step
                double yForecast = y + 4 * h / 3 * Function(t, y) - h / 3 * Function(t - h, yGunn);

                // Milne-Simpson correction step
                double yCorrected = y + h / 3 * (Function(t + h, yForecast) + 4 * Function(t, y) - Function(t - h, yGunn));
                
                series.Values.Add(new ObservablePoint(t, yCorrected));

                t += h;
                y = yCorrected;
            }
        }

        static void CalculateMilneSimpsonWithControlParameter(double t, double y, double h, double tLimit, LineSeries series, bool doubledStep)
        {
            double controlParameter = 0.01;  // control parameter for adaptive step size adjustment

            while (t <= tLimit)
            {
                double yGunn = GunnMethod(t, y, h);  // calculate initial data using Gunn's method

                // Milne-Simpson forecast step
                double yForecast = y + 4 * h / 3 * Function(t, y) - h / 3 * Function(t - h, yGunn);

                // Milne-Simpson correction step
                double yCorrected = y + h / 3 * (Function(t + h, yForecast) + 4 * Function(t, y) - Function(t - h, yGunn));

                series.Values.Add(new ObservablePoint(t, yCorrected));

                // Calculate the new step size using the control parameter
                if (doubledStep)
                {
                    double delta = Math.Abs(yCorrected - yForecast);
                    h *= Math.Pow(controlParameter / delta, 0.2);
                }

                t += h;
                y = yCorrected;
            }
        }

        private void AddVerticalScrollBar()
        {
            verticalScrollBar = new VScrollBar();
            verticalScrollBar.Dock = DockStyle.Right;
            verticalScrollBar.Minimum = 0;
            verticalScrollBar.Maximum = 100;  // Set the maximum value based on your data
            verticalScrollBar.LargeChange = 10;  // Set the large change value based on your data
            verticalScrollBar.Scroll += VerticalScrollBar_Scroll;
            Controls.Add(verticalScrollBar);
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            // Calculate the visible range based on the scroll position
            double scrollPosition = verticalScrollBar.Value;
            double visibleRange = yAxisMax - yAxisMin;
            double newMin = yAxisMin + scrollPosition * (visibleRange / verticalScrollBar.Maximum);
            double newMax = newMin + visibleRange;

            // Update the axis ranges
            yAxisMin = newMin;
            yAxisMax = newMax;

            //// Update the chart's axis ranges
            //chart.AxisY[0].MinValue = yAxisMin;
            //chart.AxisY[0].MaxValue = yAxisMax;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
