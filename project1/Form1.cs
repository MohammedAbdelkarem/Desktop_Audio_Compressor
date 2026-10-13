using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using project1;
using System.Diagnostics;



namespace AudioCompressor
{
    public partial class Form1 : Form
       
    {
        Stopwatch stopwatch = new Stopwatch();
        FormPerformance perfWindow;
        private string currentFilePath;
        private byte[] compressedData;
        private float[] decompressedSamples;

        private AudioFileReader audioFileReader;
        private WaveOutEvent outputDevice;
        private bool cancelRequested = false;


        public Form1()
        {
            InitializeComponent();
            InitializeDragAndDrop();
            lblStatus.Text = "Statue: No file choice.";

        }

        
        private void InitializeDragAndDrop()
        {
            pnlWorkspace.AllowDrop = true;
            pnlWorkspace.DragEnter += pnlWorkspace_DragEnter;
            pnlWorkspace.DragDrop += pnlWorkspace_DragDrop;
        }

        private void pnlWorkspace_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0 && IsAudioFile(files[0]))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pnlWorkspace_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files != null && files.Length > 0)
            {
                string filePath = files[0];

                if (IsAudioFile(filePath))
                {
                    LoadAudio(filePath);
                }
                else
                {
                    MessageBox.Show("The file is not audio.");
                }
            }
        }

        private bool IsAudioFile(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            string[] audioExtensions = { ".wav", ".mp3" };

            foreach (string audioExt in audioExtensions)
                if (ext == audioExt)
                    return true;

            return false;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Audio Files|*.wav;*.mp3";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadAudio(ofd.FileName);
            }
        }
        private void LoadAudio(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("file not exit.");
                return;
            }

            StopAudio();

            currentFilePath = filePath;
            audioFileReader = new AudioFileReader(currentFilePath);

            lblFileInfo.Text = $"current file:\n{Path.GetFileName(currentFilePath)}";
            lblStatus.Text = "Statue:ready to play";
            pnlWorkspace.BackColor = System.Drawing.Color.FromArgb(230, 255, 230);
            ShowAudioProperties(filePath);


        }
        private void ShowAudioProperties(string filePath)
        {
            FileInfo info = new FileInfo(filePath);

            double fileSizeMB = info.Length / (1024.0 * 1024.0);

            using (var reader = new AudioFileReader(filePath))
            {
                TimeSpan duration = reader.TotalTime;
                int sampleRate = reader.WaveFormat.SampleRate;
                int channels = reader.WaveFormat.Channels;
                int bitsPerSample = reader.WaveFormat.BitsPerSample;
                string encoding = reader.WaveFormat.Encoding.ToString();

                double bitRate = (info.Length * 8.0) / duration.TotalSeconds;

                lblProperties.Text =
                    $"Size: {fileSizeMB:F2} MB\n" +
                    $"Time: {duration}\n" +
                    $"Sample rate: {sampleRate} Hz\n" +
                    $"channels: {channels}\n" +
                    $"bitRate: {bitRate:F0} bps\n" +
                    $"encoding: {encoding}";
            }
        }
        private byte[] CompressMuLaw_Performance(float[] samples, int mu)
        {
            byte[] compressed = new byte[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                if (cancelRequested)
                    break;

                float x = Math.Max(-1f, Math.Min(1f, samples[i]));
                float sign = Math.Sign(x);
                float magnitude = (float)(Math.Log(1 + mu * Math.Abs(x)) / Math.Log(1 + mu));
                float y = sign * magnitude;

                compressed[i] = (byte)((y + 1f) * 127.5f);

                UpdatePerformance(i, samples.Length);
            }

            return compressed;
        }
        private byte[] CompressDPCM_Performance(float[] samples, int quantLevels)
        {
            short[] diffs = new short[samples.Length];

            short prev = (short)(samples[0] * 32767);
            diffs[0] = prev;

            for (int i = 1; i < samples.Length; i++)
            {
                if (cancelRequested)
                    break;

                short current = (short)(samples[i] * 32767);
                short diff = (short)(current - prev);

                diff = (short)(diff / (32767 / quantLevels));

                diffs[i] = diff;
                prev = current;

                UpdatePerformance(i, samples.Length);
            }

            byte[] bytes = new byte[diffs.Length * 2];
            Buffer.BlockCopy(diffs, 0, bytes, 0, bytes.Length);

            return bytes;
        }
        private byte[] CompressDelta_Performance(float[] samples, float stepSize)
        {
            byte[] bits = new byte[samples.Length];

            float prev = samples[0];

            for (int i = 1; i < samples.Length; i++)
            {
                if (cancelRequested)
                    break;

                if (samples[i] > prev)
                {
                    bits[i] = 1;
                    prev += stepSize;
                }
                else
                {
                    bits[i] = 0;
                    prev -= stepSize;
                }

                UpdatePerformance(i, samples.Length);
            }

            return bits;
        }
        private byte[] CompressAdaptiveDelta_Performance(float[] samples, float stepSize)
        {
            byte[] bits = new byte[samples.Length];

            float prev = samples[0];
            float step = stepSize;

            for (int i = 1; i < samples.Length; i++)
            {
                if (cancelRequested)
                    break;

                if (samples[i] > prev)
                {
                    bits[i] = 1;
                    prev += step;
                    step *= 1.1f;
                }
                else
                {
                    bits[i] = 0;
                    prev -= step;
                    step *= 0.9f;
                }

                UpdatePerformance(i, samples.Length);
            }

            return bits;
        }
        private byte[] CompressPredictive_Performance(float[] samples, int quantLevels)
        {
            short[] diffs = new short[samples.Length];

            float prev = samples[0];
            diffs[0] = (short)(prev * 32767);

            for (int i = 1; i < samples.Length; i++)
            {
                if (cancelRequested)
                    break;

                float predicted = prev;
                float error = samples[i] - predicted;

                short qError = (short)(error * quantLevels);

                diffs[i] = qError;
                prev = samples[i];

                UpdatePerformance(i, samples.Length);
            }

            byte[] bytes = new byte[diffs.Length * 2];
            Buffer.BlockCopy(diffs, 0, bytes, 0, bytes.Length);

            return bytes;
        }
        private void UpdatePerformance(int i, int total)
        {
            if (perfWindow == null || perfWindow.IsDisposed) return;

            if (i % 2000 == 0)
            {
                int progress = (int)((i / (float)total) * 100);
                perfWindow.Progress.Value = progress;

                double ratio = (i + 1) / (double)total;
                perfWindow.CompressionChart.Series["CompressionSeries"].Points.AddY(ratio);

                double elapsed = stopwatch.ElapsedMilliseconds / 1000.0;
                if (elapsed > 0)
                {
                    double speed = (i + 1) / elapsed;
                    perfWindow.SpeedChart.Series["SpeedSeries"].Points.AddY(speed);
                }
            }
        }

        private float[] DecompressMuLaw(byte[] compressed, int mu = 255)
        {
            float[] samples = new float[compressed.Length];

            for (int i = 0; i < compressed.Length; i++)
            {
                float y = (compressed[i] / 127.5f) - 1f;
                float sign = Math.Sign(y);
                float magnitude = (float)((Math.Pow(1 + mu, Math.Abs(y)) - 1) / mu);
                samples[i] = sign * magnitude;
            }

            return samples;
        }
        private float[] DecompressDPCM(byte[] compressed)
        {
            short[] diffs = new short[compressed.Length / 2];
            Buffer.BlockCopy(compressed, 0, diffs, 0, compressed.Length);

            float[] samples = new float[diffs.Length];

            short prev = diffs[0];
            samples[0] = prev / 32767f;

            for (int i = 1; i < diffs.Length; i++)
            {
                short current = (short)(prev + diffs[i]);
                samples[i] = current / 32767f;
                prev = current;
            }

            return samples;
        }
        private float[] DecompressDelta(byte[] bits)
        {
            float[] samples = new float[bits.Length];

            float value = 0;
            float step = 0.01f;

            samples[0] = value;

            for (int i = 1; i < bits.Length; i++)
            {
                if (bits[i] == 1)
                    value += step;
                else
                    value -= step;

                samples[i] = value;
            }

            return samples;
        }
        private float[] DecompressAdaptiveDelta(byte[] bits)
        {
            float[] samples = new float[bits.Length];

            float value = 0;
            float step = 0.01f;

            samples[0] = value;

            for (int i = 1; i < bits.Length; i++)
            {
                if (bits[i] == 1)
                {
                    value += step;
                    step *= 1.1f;
                }
                else
                {
                    value -= step;
                    step *= 0.9f;
                }

                samples[i] = value;
            }

            return samples;
        }
        private float[] DecompressPredictive(byte[] compressed)
        {
            short[] diffs = new short[compressed.Length / 2];
            Buffer.BlockCopy(compressed, 0, diffs, 0, compressed.Length);

            float[] samples = new float[diffs.Length];

            float prev = diffs[0] / 32767f;
            samples[0] = prev;

            for (int i = 1; i < diffs.Length; i++)
            {
                float error = diffs[i] / 32767f;
                float current = prev + error;

                samples[i] = current;
                prev = current;
            }

            return samples;
        }
        private float[] ReadSamples(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                int sampleCount = (int)(reader.Length / sizeof(float));
                float[] buffer = new float[sampleCount];
                reader.Read(buffer, 0, sampleCount);
                return buffer;
            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentFilePath))
            {
                MessageBox.Show("No audio file choice.");
                return;
            }

            if (!File.Exists(currentFilePath))
            {
                MessageBox.Show("file not exit .");
                return;
            }

            try
            {
                StopAudio(); 

                audioFileReader = new AudioFileReader(currentFilePath);

                outputDevice = new WaveOutEvent()
                {
                    DeviceNumber = 0  
                };

                outputDevice.Init(audioFileReader);
                outputDevice.Play();

                lblStatus.Text = "Statue: play...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:\n" + ex.Message);
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopAudio();
        }

        private void StopAudio()
        {
            try
            {
                if (outputDevice != null)
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    outputDevice = null;
                }

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }

                lblStatus.Text = "Statue: stop";
            }
            catch { }
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            int sampleRate = (int)numSampleRate.Value;
            int mu = (int)numMu.Value;
            float stepSize = (float)numStepSize.Value;
            int quantLevels = (int)numQuantization.Value;

            if (string.IsNullOrWhiteSpace(currentFilePath))
            {
                MessageBox.Show("please choice audio file");
                return;
            }

            if (cmbAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("please choice algorithm");
                return;
            }

            try
            {
                float[] samples = ReadSamples(currentFilePath);

                cancelRequested = false;

                if (perfWindow != null && !perfWindow.IsDisposed)
                {
                    perfWindow.Progress.Value = 0;
                    perfWindow.CompressionChart.Series["CompressionSeries"].Points.Clear();
                    perfWindow.SpeedChart.Series["SpeedSeries"].Points.Clear();
                    perfWindow.TimeLabel.Text = "Time: —";
                    perfWindow.RatioLabel.Text = "compress ratio: —";
                }

                stopwatch.Restart();

                string algo = cmbAlgorithm.SelectedItem.ToString();

                switch (algo)
                {
                    case "Nonlinear Quantization (μ-law)":
                        compressedData = CompressMuLaw_Performance(samples, mu);
                        break;

                    case "DPCM":
                        compressedData = CompressDPCM_Performance(samples, quantLevels);
                        break;

                    case "Delta Modulation":
                        compressedData = CompressDelta_Performance(samples, stepSize);
                        break;

                    case "Adaptive Delta Modulation":
                        compressedData = CompressAdaptiveDelta_Performance(samples, stepSize);
                        break;

                    case "Predictive Coding":
                        compressedData = CompressPredictive_Performance(samples, quantLevels);
                        break;
                }

                stopwatch.Stop();

                if (cancelRequested)
                {
                    lblCompressStatus.Text = "compress canceled.";
                    MessageBox.Show("compress canceled done!");
                    return;
                }

                double originalSize = samples.Length * sizeof(float);
                double compressedSize = compressedData.Length;

                if (perfWindow != null && !perfWindow.IsDisposed)
                {
                    perfWindow.Progress.Value = 100;
                    perfWindow.TimeLabel.Text = $"Time: {stopwatch.ElapsedMilliseconds} ms";

                    double ratioFinal = (1 - (compressedSize / originalSize)) * 100;
                    perfWindow.RatioLabel.Text = $"compress ratio: {ratioFinal:F2}%";
                }

                ShowCompressionReport(algo, originalSize, compressedSize, stopwatch.ElapsedMilliseconds);

                lblCompressStatus.Text = $"compress file by: {algo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:\n" + ex.Message);
            }
        }

        private void btnSaveCompres_Click(object sender, EventArgs e)
        {
            if (compressedData == null)
            {
                MessageBox.Show("Ni compress file to save");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Compressed Audio|*.bin";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, compressedData);
                MessageBox.Show("Save done");
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            if (compressedData == null)
            {
                MessageBox.Show("No compress file to decompress");
                return;
            }

            if (cmbAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("please choice algorithm");
                return;
            }

            string algo = cmbAlgorithm.SelectedItem.ToString();

            switch (algo)
            {
                case "Nonlinear Quantization (μ-law)":
                    decompressedSamples = DecompressMuLaw(compressedData);
                    break;

                case "DPCM":
                    decompressedSamples = DecompressDPCM(compressedData);
                    break;

                case "Delta Modulation":
                    decompressedSamples = DecompressDelta(compressedData);
                    break;

                case "Adaptive Delta Modulation":
                    decompressedSamples = DecompressAdaptiveDelta(compressedData);
                    break;

                case "Predictive Coding":
                    decompressedSamples = DecompressPredictive(compressedData);
                    break;
            }

            lblDecompressStatus.Text = "Decompress Done";
        }

        private void btnSaveDecompress_Click(object sender, EventArgs e)
        {
            if (decompressedSamples == null)
            {
                MessageBox.Show("No decompress data to save");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WAV File|*.wav";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (WaveFileWriter writer = new WaveFileWriter(sfd.FileName, new WaveFormat(44100, 1)))
                {
                    foreach (float sample in decompressedSamples)
                    {
                        writer.WriteSample(sample);
                    }
                }

                MessageBox.Show("Decompress file save");
            }
        }


        private void btnShowPerformance_Click(object sender, EventArgs e)
        {
            if (perfWindow == null || perfWindow.IsDisposed)
                perfWindow = new FormPerformance();

            perfWindow.Show();
            perfWindow.BringToFront();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelRequested = true;

            if (perfWindow != null && !perfWindow.IsDisposed)
            {
                perfWindow.Progress.BackColor = System.Drawing.Color.Red;
                perfWindow.Progress.ForeColor = System.Drawing.Color.White;

                perfWindow.CompressionChart.Series["CompressionSeries"].Points.Clear();
                perfWindow.SpeedChart.Series["SpeedSeries"].Points.Clear();

                perfWindow.RatioLabel.Text = "canceled";
                perfWindow.TimeLabel.Text = "—";
            }

            lblCompressStatus.Text = "compress canceled.";
            MessageBox.Show("compress canceled done!");
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            StopAudio();

            cancelRequested = true;

            compressedData = null;
            decompressedSamples = null;

           

            numSampleRate.Value = 44100;
            numMu.Value = 255;
            numStepSize.Value =1;
            numQuantization.Value = 16;
            cmbAlgorithm.SelectedIndex = -1;

            if (perfWindow != null && !perfWindow.IsDisposed)
            {
                perfWindow.Progress.Value = 0;
                perfWindow.CompressionChart.Series["CompressionSeries"].Points.Clear();
                perfWindow.SpeedChart.Series["SpeedSeries"].Points.Clear();
                perfWindow.TimeLabel.Text = "Time: —";
                perfWindow.RatioLabel.Text = "compress Ratio: —";
            }

            MessageBox.Show("Reset Done !");

        }
        private void ShowCompressionReport(string algo, double originalSize, double compressedSize, long elapsedMs)
        {
            double savingRatio = (1 - (compressedSize / originalSize)) * 100;

            string report =
                "Compress Report\n\n" +
                $"Algorithm: {algo}\n" +
                $"Sample rate (Sample Rate): {numSampleRate.Value} Hz\n" +
                $"μ-law: {numMu.Value}\n" +
                $"Step Size: {numStepSize.Value}\n" +
                $"Quantization Levels: {numQuantization.Value}\n\n" +
                $"originalSize: {(originalSize / (1024 * 1024)):F2} MB\n" +
                $"compressedSize: {(compressedSize / (1024 * 1024)):F2} MB\n" +
                $"savingRatio: {savingRatio:F2}%\n" +
                $"Time: {elapsedMs} ms";

            MessageBox.Show(report, "Compress Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
