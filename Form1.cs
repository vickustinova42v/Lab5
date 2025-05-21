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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public string filledChart;
        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (filledChart == "" || filledChart == "chart2")
            {
                chart1.Series.Clear();
                chart1.Series.Add("y(x)");
                chart1.Series["y(x)"].ChartType = SeriesChartType.Line;
                chart1.Series["y(x)"].BorderWidth = 2;
                chart1.ChartAreas[0].AxisX.Title = "x";
                chart1.ChartAreas[0].AxisY.Title = "y";
                chart1.ChartAreas[0].AxisX.Minimum = Double.NaN; // Авто-масштаб
                chart1.ChartAreas[0].AxisX.Maximum = Double.NaN; // Авто-масштаб
                chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // Авто-масштаб
                chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // Авто-масштаб
                filledChart = "chart1";
            } else
            {
                chart2.Series.Clear();
                chart2.Series.Add("y(x)");
                chart2.Series["y(x)"].ChartType = SeriesChartType.Line;
                chart2.Series["y(x)"].BorderWidth = 2;
                chart2.ChartAreas[0].AxisX.Title = "x";
                chart2.ChartAreas[0].AxisY.Title = "y";
                chart2.ChartAreas[0].AxisX.Minimum = Double.NaN; // Авто-масштаб
                chart2.ChartAreas[0].AxisX.Maximum = Double.NaN; // Авто-масштаб
                chart2.ChartAreas[0].AxisY.Minimum = Double.NaN; // Авто-масштаб
                chart2.ChartAreas[0].AxisY.Maximum = Double.NaN; // Авто-масштаб
                filledChart = "chart2";
            }

            double x0 = double.Parse(textBoxX0.Text); // начальное значение x
            double y1 = double.Parse(textBoxY0.Text); // начальное значение y (y1)
            double y2 = double.Parse(textBoxY1.Text); // начальное значение y' (y2)
            double y3 = double.Parse(textBoxY2.Text); // начальное значение y'' (y3)
            double h = double.Parse(textBoxH.Text); // шаг
            int steps = int.Parse(textBoxN.Text); // количество шагов

            for (int i = 0; i < steps; i++)
            {

                if (filledChart == "chart1")
                {
                    chart1.Series["y(x)"].Points.AddXY(x0, y1);
                } else
                {
                    chart2.Series["y(x)"].Points.AddXY(x0, y1);
                }

                // Преобразование уравнения 3-го порядка y''' = x^2 + xy + x^2y' + xy''
                // в систему трёх уравнений первого порядка:
                // y1 = y,    y2 = y',    y3 = y''
                // y1' = y2
                // y2' = y3
                // y3' = x^2 + x*y1 + x^2*y2 + x*y3

                double k1_1 = h * y2;
                double k1_2 = h * y3;
                double k1_3 = h * f1(x0, y1, y2, y3);

                double k2_1 = h * (y2 + 0.5 * k1_2);
                double k2_2 = h * (y3 + 0.5 * k1_3);
                double k2_3 = h * f1(x0 + 0.5 * h, y1 + 0.5 * k1_1, y2 + 0.5 * k1_2, y3 + 0.5 * k1_3);

                double k3_1 = h * (y2 + 0.5 * k2_2);
                double k3_2 = h * (y3 + 0.5 * k2_3);
                double k3_3 = h * f1(x0 + 0.5 * h, y1 + 0.5 * k2_1, y2 + 0.5 * k2_2, y3 + 0.5 * k2_3);

                double k4_1 = h * (y2 + k3_2);
                double k4_2 = h * (y3 + k3_3);
                double k4_3 = h * f1(x0 + h, y1 + k3_1, y2 + k3_2, y3 + k3_3);

                y1 += (k1_1 + 2 * k2_1 + 2 * k3_1 + k4_1) / 6;
                y2 += (k1_2 + 2 * k2_2 + 2 * k3_2 + k4_2) / 6;
                y3 += (k1_3 + 2 * k2_3 + 2 * k3_3 + k4_3) / 6;

                x0 += h;
            }
        }

       // Уравнение 3-го порядка y''' = x^2 + xy + x^2y' + xy''
        private double f1(double x, double y, double y1, double y2)
        {
            return x * x + x * y + x * x * y1 + x * y2;
        }
    }
}
