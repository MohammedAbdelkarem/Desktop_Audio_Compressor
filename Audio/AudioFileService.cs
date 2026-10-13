using NAudio.Wave;

namespace project1.Audio
{
    public sealed class AudioFileService
    {
        public float[] ReadSamples(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                int sampleCount = (int)(reader.Length / sizeof(float));
                float[] buffer = new float[sampleCount];
                reader.Read(buffer, 0, sampleCount);
                return buffer;
            }
        }

        public void SaveAsWave(string filePath, float[] samples, int sampleRate, int channels)
        {
            using (WaveFileWriter writer = new WaveFileWriter(filePath, new WaveFormat(sampleRate, channels)))
            {
                foreach (float sample in samples)
                    writer.WriteSample(sample);
            }
        }
    }
}