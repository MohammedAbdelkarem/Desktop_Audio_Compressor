
namespace project1
{
    partial class FormPerformance
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.chartCompression = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartSpeed = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblRatio = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartCompression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(661, 89);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // chartCompression
            // 
            chartArea1.Name = "ChartArea1";
            this.chartCompression.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCompression.Legends.Add(legend1);
            this.chartCompression.Location = new System.Drawing.Point(0, 150);
            this.chartCompression.Name = "chartCompression";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "CompressionSeries\n";
            this.chartCompression.Series.Add(series1);
            this.chartCompression.Size = new System.Drawing.Size(345, 300);
            this.chartCompression.TabIndex = 1;
            this.chartCompression.Text = "chartCompression\n";
            // 
            // chartSpeed
            // 
            chartArea2.Name = "ChartArea1";
            this.chartSpeed.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartSpeed.Legends.Add(legend2);
            this.chartSpeed.Location = new System.Drawing.Point(439, 150);
            this.chartSpeed.Name = "chartSpeed";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "SpeedSeries\n";
            this.chartSpeed.Series.Add(series2);
            this.chartSpeed.Size = new System.Drawing.Size(322, 300);
            this.chartSpeed.TabIndex = 2;
            this.chartSpeed.Text = "chart1";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(13, 94);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(46, 17);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "label1";
            // 
            // lblRatio
            // 
            this.lblRatio.AutoSize = true;
            this.lblRatio.Location = new System.Drawing.Point(135, 94);
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.Size = new System.Drawing.Size(46, 17);
            this.lblRatio.TabIndex = 4;
            this.lblRatio.Text = "label1";
            // 
            // FormPerformance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRatio);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.chartSpeed);
            this.Controls.Add(this.chartCompression);
            this.Controls.Add(this.progressBar1);
            this.Name = "FormPerformance";
            this.Text = "FormPerformance";
            ((System.ComponentModel.ISupportInitialize)(this.chartCompression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCompression;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpeed;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblRatio;
    }
}