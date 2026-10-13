
namespace AudioCompressor
{
    partial class Form1
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
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.pnlWorkspace = new System.Windows.Forms.Panel();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblProperties = new System.Windows.Forms.Label();
            this.btnCompress = new System.Windows.Forms.Button();
            this.btnSaveCompres = new System.Windows.Forms.Button();
            this.lblCompressStatus = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnDecompress = new System.Windows.Forms.Button();
            this.btnSaveDecompress = new System.Windows.Forms.Button();
            this.lblDecompressStatus = new System.Windows.Forms.Label();
            this.numSampleRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numMu = new System.Windows.Forms.NumericUpDown();
            this.numStepSize = new System.Windows.Forms.NumericUpDown();
            this.numQuantization = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.btnShowPerformance = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlWorkspace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantization)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(23, 23);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(115, 34);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "oppenfile";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // pnlWorkspace
            // 
            this.pnlWorkspace.Controls.Add(this.lblFileInfo);
            this.pnlWorkspace.Location = new System.Drawing.Point(-7, 63);
            this.pnlWorkspace.Name = "pnlWorkspace";
            this.pnlWorkspace.Size = new System.Drawing.Size(265, 198);
            this.pnlWorkspace.TabIndex = 1;
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.AutoSize = true;
            this.lblFileInfo.Location = new System.Drawing.Point(9, 149);
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Size = new System.Drawing.Size(67, 17);
            this.lblFileInfo.TabIndex = 2;
            this.lblFileInfo.Text = "Nochoice";
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(3, 384);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(105, 33);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "btnPlay";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(150, 384);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(108, 33);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "btnStop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(113, 424);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 17);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "lblStatus";
            // 
            // lblProperties
            // 
            this.lblProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProperties.Location = new System.Drawing.Point(264, 61);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Size = new System.Drawing.Size(275, 200);
            this.lblProperties.TabIndex = 5;
            this.lblProperties.Text = "lblProperties\n";
            // 
            // btnCompress
            // 
            this.btnCompress.Location = new System.Drawing.Point(316, 370);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(126, 33);
            this.btnCompress.TabIndex = 6;
            this.btnCompress.Text = "btnCompress\n";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // btnSaveCompres
            // 
            this.btnSaveCompres.Location = new System.Drawing.Point(316, 409);
            this.btnSaveCompres.Name = "btnSaveCompres";
            this.btnSaveCompres.Size = new System.Drawing.Size(126, 32);
            this.btnSaveCompres.TabIndex = 7;
            this.btnSaveCompres.Text = "btnSaveCompressed\n";
            this.btnSaveCompres.UseVisualStyleBackColor = true;
            this.btnSaveCompres.Click += new System.EventHandler(this.btnSaveCompres_Click);
            // 
            // lblCompressStatus
            // 
            this.lblCompressStatus.AutoSize = true;
            this.lblCompressStatus.Location = new System.Drawing.Point(448, 400);
            this.lblCompressStatus.Name = "lblCompressStatus";
            this.lblCompressStatus.Size = new System.Drawing.Size(125, 17);
            this.lblCompressStatus.TabIndex = 8;
            this.lblCompressStatus.Text = "lblCompressStatus\n";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "Nonlinear Quantization (μ-law)",
            "",
            "DPCM",
            "",
            "Delta Modulation",
            "",
            "Adaptive Delta Modulation",
            "",
            "Predictive Coding"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(463, 314);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(121, 24);
            this.cmbAlgorithm.TabIndex = 9;
            this.cmbAlgorithm.Text = "cmbAlgorithm\n";
            // 
            // btnDecompress
            // 
            this.btnDecompress.Location = new System.Drawing.Point(628, 290);
            this.btnDecompress.Name = "btnDecompress";
            this.btnDecompress.Size = new System.Drawing.Size(150, 33);
            this.btnDecompress.TabIndex = 10;
            this.btnDecompress.Text = "btnDecompress\n";
            this.btnDecompress.UseVisualStyleBackColor = true;
            this.btnDecompress.Click += new System.EventHandler(this.btnDecompress_Click);
            // 
            // btnSaveDecompress
            // 
            this.btnSaveDecompress.Location = new System.Drawing.Point(628, 333);
            this.btnSaveDecompress.Name = "btnSaveDecompress";
            this.btnSaveDecompress.Size = new System.Drawing.Size(150, 31);
            this.btnSaveDecompress.TabIndex = 11;
            this.btnSaveDecompress.Text = "btnSaveDecompressed\n";
            this.btnSaveDecompress.UseVisualStyleBackColor = true;
            this.btnSaveDecompress.Click += new System.EventHandler(this.btnSaveDecompress_Click);
            // 
            // lblDecompressStatus
            // 
            this.lblDecompressStatus.AutoSize = true;
            this.lblDecompressStatus.Location = new System.Drawing.Point(628, 384);
            this.lblDecompressStatus.Name = "lblDecompressStatus";
            this.lblDecompressStatus.Size = new System.Drawing.Size(141, 17);
            this.lblDecompressStatus.TabIndex = 12;
            this.lblDecompressStatus.Text = "lblDecompressStatus\n";
            // 
            // numSampleRate
            // 
            this.numSampleRate.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSampleRate.Location = new System.Drawing.Point(659, 75);
            this.numSampleRate.Maximum = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            this.numSampleRate.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numSampleRate.Name = "numSampleRate";
            this.numSampleRate.Size = new System.Drawing.Size(120, 22);
            this.numSampleRate.TabIndex = 13;
            this.numSampleRate.Value = new decimal(new int[] {
            44100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(564, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sample Rate";
            // 
            // numMu
            // 
            this.numMu.Location = new System.Drawing.Point(659, 104);
            this.numMu.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMu.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMu.Name = "numMu";
            this.numMu.Size = new System.Drawing.Size(120, 22);
            this.numMu.TabIndex = 15;
            this.numMu.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // numStepSize
            // 
            this.numStepSize.DecimalPlaces = 3;
            this.numStepSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numStepSize.Location = new System.Drawing.Point(659, 133);
            this.numStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numStepSize.Name = "numStepSize";
            this.numStepSize.Size = new System.Drawing.Size(120, 22);
            this.numStepSize.TabIndex = 16;
            this.numStepSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // numQuantization
            // 
            this.numQuantization.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numQuantization.Location = new System.Drawing.Point(659, 162);
            this.numQuantization.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numQuantization.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numQuantization.Name = "numQuantization";
            this.numQuantization.Size = new System.Drawing.Size(120, 22);
            this.numQuantization.TabIndex = 17;
            this.numQuantization.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(567, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "μ-law";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(570, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "Step Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(538, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "numQuantization\n";
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.Location = new System.Drawing.Point(628, 238);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(148, 32);
            this.btnResetSettings.TabIndex = 21;
            this.btnResetSettings.Text = "btnResetSettings\n";
            this.btnResetSettings.UseVisualStyleBackColor = true;
            this.btnResetSettings.Click += new System.EventHandler(this.btnResetSettings_Click);
            // 
            // btnShowPerformance
            // 
            this.btnShowPerformance.Location = new System.Drawing.Point(12, 302);
            this.btnShowPerformance.Name = "btnShowPerformance";
            this.btnShowPerformance.Size = new System.Drawing.Size(168, 36);
            this.btnShowPerformance.TabIndex = 22;
            this.btnShowPerformance.Text = "btnShowPerformance\n";
            this.btnShowPerformance.UseVisualStyleBackColor = true;
            this.btnShowPerformance.Click += new System.EventHandler(this.btnShowPerformance_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 36);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShowPerformance);
            this.Controls.Add(this.btnResetSettings);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numQuantization);
            this.Controls.Add(this.numStepSize);
            this.Controls.Add(this.numMu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numSampleRate);
            this.Controls.Add(this.lblDecompressStatus);
            this.Controls.Add(this.btnSaveDecompress);
            this.Controls.Add(this.btnDecompress);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.lblCompressStatus);
            this.Controls.Add(this.btnSaveCompres);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.lblProperties);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pnlWorkspace);
            this.Controls.Add(this.btnOpenFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlWorkspace.ResumeLayout(false);
            this.pnlWorkspace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantization)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Panel pnlWorkspace;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblProperties;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Button btnSaveCompres;
        private System.Windows.Forms.Label lblCompressStatus;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Button btnDecompress;
        private System.Windows.Forms.Button btnSaveDecompress;
        private System.Windows.Forms.Label lblDecompressStatus;
        private System.Windows.Forms.NumericUpDown numSampleRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMu;
        private System.Windows.Forms.NumericUpDown numStepSize;
        private System.Windows.Forms.NumericUpDown numQuantization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnResetSettings;
        private System.Windows.Forms.Button btnShowPerformance;
        private System.Windows.Forms.Button btnCancel;
    }
}

