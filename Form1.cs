using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using project1;
using project1.Audio;
using project1.Compression;
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
        private readonly AudioFileService audioFileService = new AudioFileService();
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
                float[] samples = audioFileService.ReadSamples(currentFilePath);

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
                CompressionSettings settings = new CompressionSettings
                {
                    Mu = mu,
                    StepSize = stepSize,
                    QuantizationLevels = quantLevels
                };
                IAudioCodec codec = AudioCodecFactory.Create(algo);
                compressedData = codec.Compress(
                    samples,
                    settings,
                    index => UpdatePerformance(index, samples.Length),
                    () => cancelRequested);

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
            CompressionSettings settings = new CompressionSettings
            {
                Mu = (int)numMu.Value,
                StepSize = (float)numStepSize.Value,
                QuantizationLevels = (int)numQuantization.Value
            };
            IAudioCodec codec = AudioCodecFactory.Create(algo);
            decompressedSamples = codec.Decompress(compressedData, settings);

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
                audioFileService.SaveAsWave(sfd.FileName, decompressedSamples, 44100, 1);

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
