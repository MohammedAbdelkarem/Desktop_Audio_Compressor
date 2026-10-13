using System;

namespace project1.Compression
{
    public sealed class CompressionSettings
    {
        public int Mu { get; set; }
        public float StepSize { get; set; }
        public int QuantizationLevels { get; set; }
    }

    public interface IAudioCodec
    {
        byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested);
        float[] Decompress(byte[] compressed, CompressionSettings settings);
    }

    public static class AudioCodecFactory
    {
        public static IAudioCodec Create(string algorithm)
        {
            switch (algorithm)
            {
                case "Nonlinear Quantization (μ-law)":
                    return new MuLawCodec();
                case "DPCM":
                    return new DpcmCodec();
                case "Delta Modulation":
                    return new DeltaCodec();
                case "Adaptive Delta Modulation":
                    return new AdaptiveDeltaCodec();
                case "Predictive Coding":
                    return new PredictiveCodec();
                default:
                    throw new ArgumentException("Unknown compression algorithm.", nameof(algorithm));
            }
        }
    }

    internal sealed class MuLawCodec : IAudioCodec
    {
        public byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested)
        {
            byte[] compressed = new byte[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                if (isCancellationRequested())
                    break;

                float x = Math.Max(-1f, Math.Min(1f, samples[i]));
                float sign = Math.Sign(x);
                float magnitude = (float)(Math.Log(1 + settings.Mu * Math.Abs(x)) / Math.Log(1 + settings.Mu));
                float y = sign * magnitude;

                compressed[i] = (byte)((y + 1f) * 127.5f);
                progress(i);
            }

            return compressed;
        }

        public float[] Decompress(byte[] compressed, CompressionSettings settings)
        {
            float[] samples = new float[compressed.Length];

            for (int i = 0; i < compressed.Length; i++)
            {
                float y = (compressed[i] / 127.5f) - 1f;
                float sign = Math.Sign(y);
                float magnitude = (float)((Math.Pow(1 + settings.Mu, Math.Abs(y)) - 1) / settings.Mu);
                samples[i] = sign * magnitude;
            }

            return samples;
        }
    }

    internal sealed class DpcmCodec : IAudioCodec
    {
        public byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested)
        {
            short[] diffs = new short[samples.Length];
            short prev = (short)(samples[0] * 32767);
            diffs[0] = prev;

            for (int i = 1; i < samples.Length; i++)
            {
                if (isCancellationRequested())
                    break;

                short current = (short)(samples[i] * 32767);
                short diff = (short)(current - prev);
                diff = (short)(diff / (32767 / settings.QuantizationLevels));
                diffs[i] = diff;
                prev = current;
                progress(i);
            }

            byte[] bytes = new byte[diffs.Length * 2];
            Buffer.BlockCopy(diffs, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public float[] Decompress(byte[] compressed, CompressionSettings settings)
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
    }

    internal sealed class DeltaCodec : IAudioCodec
    {
        public byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested)
        {
            byte[] bits = new byte[samples.Length];
            float prev = samples[0];

            for (int i = 1; i < samples.Length; i++)
            {
                if (isCancellationRequested())
                    break;

                if (samples[i] > prev)
                {
                    bits[i] = 1;
                    prev += settings.StepSize;
                }
                else
                {
                    bits[i] = 0;
                    prev -= settings.StepSize;
                }

                progress(i);
            }

            return bits;
        }

        public float[] Decompress(byte[] compressed, CompressionSettings settings)
        {
            float[] samples = new float[compressed.Length];
            float value = 0;
            float step = 0.01f;
            samples[0] = value;

            for (int i = 1; i < compressed.Length; i++)
            {
                value += compressed[i] == 1 ? step : -step;
                samples[i] = value;
            }

            return samples;
        }
    }

    internal sealed class AdaptiveDeltaCodec : IAudioCodec
    {
        public byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested)
        {
            byte[] bits = new byte[samples.Length];
            float prev = samples[0];
            float step = settings.StepSize;

            for (int i = 1; i < samples.Length; i++)
            {
                if (isCancellationRequested())
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

                progress(i);
            }

            return bits;
        }

        public float[] Decompress(byte[] compressed, CompressionSettings settings)
        {
            float[] samples = new float[compressed.Length];
            float value = 0;
            float step = 0.01f;
            samples[0] = value;

            for (int i = 1; i < compressed.Length; i++)
            {
                if (compressed[i] == 1)
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
    }

    internal sealed class PredictiveCodec : IAudioCodec
    {
        public byte[] Compress(float[] samples, CompressionSettings settings, Action<int> progress, Func<bool> isCancellationRequested)
        {
            short[] diffs = new short[samples.Length];
            float prev = samples[0];
            diffs[0] = (short)(prev * 32767);

            for (int i = 1; i < samples.Length; i++)
            {
                if (isCancellationRequested())
                    break;

                float error = samples[i] - prev;
                short qError = (short)(error * settings.QuantizationLevels);
                diffs[i] = qError;
                prev = samples[i];
                progress(i);
            }

            byte[] bytes = new byte[diffs.Length * 2];
            Buffer.BlockCopy(diffs, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public float[] Decompress(byte[] compressed, CompressionSettings settings)
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
    }
}