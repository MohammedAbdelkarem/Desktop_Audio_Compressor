using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace project1
{
    public partial class FormPerformance : Form
    {
        public FormPerformance()
        {
            InitializeComponent();
            InitializeCharts();
        }

        // تجهيز الرسمات
        private void InitializeCharts()
        {
            // رسم نسبة الضغط
            chartCompression.Series.Clear();
            Series s1 = new Series("CompressionSeries");
            s1.ChartType = SeriesChartType.Line;
            s1.BorderWidth = 2;
            s1.Color = System.Drawing.Color.Blue;
            chartCompression.Series.Add(s1);

            // رسم سرعة المعالجة
            chartSpeed.Series.Clear();
            Series s2 = new Series("SpeedSeries");
            s2.ChartType = SeriesChartType.Line;
            s2.BorderWidth = 2;
            s2.Color = System.Drawing.Color.Red;
            chartSpeed.Series.Add(s2);
        }

        // خصائص ليتحكم فيها Form1
        public ProgressBar Progress => progressBar1;
        public Chart CompressionChart => chartCompression;
        public Chart SpeedChart => chartSpeed;
        public Label TimeLabel => lblTime;
        public Label RatioLabel => lblRatio;

    }
}
