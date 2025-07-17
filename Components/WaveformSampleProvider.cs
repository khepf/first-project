using NAudio.Wave;
using System;

namespace MyMusicPlayer
{
    /// <summary>
    /// Sample provider that captures audio samples for waveform visualization
    /// </summary>
    public class WaveformSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider sourceProvider;
        private readonly WaveformDisplay waveformDisplay;
        private int sampleCounter = 0;
        private float currentPeak = 0;
        private readonly int samplesPerUpdate = 1024; // Update waveform every 1024 samples (~23ms at 44.1kHz)

        public WaveFormat WaveFormat => sourceProvider.WaveFormat;

        public WaveformSampleProvider(ISampleProvider sourceProvider, WaveformDisplay waveformDisplay)
        {
            this.sourceProvider = sourceProvider;
            this.waveformDisplay = waveformDisplay;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sourceProvider.Read(buffer, offset, count);
            
            // Process samples for waveform visualization
            for (int i = offset; i < offset + samplesRead; i++)
            {
                float sample = Math.Abs(buffer[i]);
                
                // Track peak level over a window of samples
                if (sample > currentPeak)
                {
                    currentPeak = sample;
                }
                
                sampleCounter++;
                
                // Update waveform display periodically (not on every sample to avoid performance issues)
                if (sampleCounter >= samplesPerUpdate)
                {
                    waveformDisplay?.AddSample(currentPeak);
                    currentPeak = 0;
                    sampleCounter = 0;
                }
            }
            
            return samplesRead;
        }

        /// <summary>
        /// Clear the waveform display when playback stops
        /// </summary>
        public void ClearWaveform()
        {
            waveformDisplay?.Clear();
        }
    }
}
