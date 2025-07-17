using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MyMusicPlayer
{
    public class WaveformDisplay : Control
    {
        private List<float> audioSamples = new List<float>();
        private readonly object samplesLock = new object();
        private int maxSamples = 40; // Fewer bars for smaller width (70px / 2px per bar)
        private readonly Color waveColor = ColorTranslator.FromHtml("#D2691E"); // Bronze theme
        private readonly Color waveColorDark = ColorTranslator.FromHtml("#8B4513"); // Darker bronze

        public WaveformDisplay()
        {
            SetStyle(ControlStyles.UserPaint | 
                     ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.DoubleBuffer | 
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            
            BackColor = Color.Transparent;
            Size = new Size(100, 48); // Adjust this width as desired (was 80)
            
            // Initialize with silence
            for (int i = 0; i < maxSamples; i++)
            {
                audioSamples.Add(0.0f);
            }
        }

        /// <summary>
        /// Add new audio sample data for real-time visualization
        /// </summary>
        /// <param name="sample">Audio sample value (typically -1.0 to 1.0)</param>
        public void AddSample(float sample)
        {
            lock (samplesLock)
            {
                // Add new sample and remove oldest if we exceed max
                audioSamples.Add(Math.Abs(sample)); // Use absolute value for amplitude
                if (audioSamples.Count > maxSamples)
                {
                    audioSamples.RemoveAt(0);
                }
            }
            
            // Trigger repaint on UI thread
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Invalidate()));
            }
            else
            {
                Invalidate();
            }
        }

        /// <summary>
        /// Clear the waveform display
        /// </summary>
        public void Clear()
        {
            lock (samplesLock)
            {
                for (int i = 0; i < audioSamples.Count; i++)
                {
                    audioSamples[i] = 0.0f;
                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // No background fill or border - let the cassette image show through completely

            lock (samplesLock)
            {
                if (audioSamples.Count == 0) return;

                float barWidth = (float)Width / maxSamples;
                float centerY = Height / 2.0f;
                float maxBarHeight = (Height - 6) / 2.0f; // Leave 3px padding top/bottom for compact look

                for (int i = 0; i < audioSamples.Count; i++)
                {
                    float amplitude = audioSamples[i];
                    float barHeight = amplitude * maxBarHeight;
                    
                    // Calculate bar position
                    float x = i * barWidth;
                    float topY = centerY - barHeight;
                    float bottomY = centerY + barHeight;
                    
                    // Create gradient brush for each bar
                    if (barHeight > 1)
                    {
                        RectangleF barRect = new RectangleF(x, topY, barWidth - 1, barHeight * 2);
                        
                        using (LinearGradientBrush barBrush = new LinearGradientBrush(
                            barRect,
                            waveColor,
                            waveColorDark,
                            LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(barBrush, barRect);
                        }
                    }
                    else
                    {
                        // For very small amplitudes, just draw a center line
                        using (Pen centerPen = new Pen(waveColorDark, 1))
                        {
                            g.DrawLine(centerPen, x, centerY, x + barWidth - 1, centerY);
                        }
                    }
                }
            }

            // Draw center line
            using (Pen centerPen = new Pen(Color.FromArgb(50, Color.Gray), 1))
            {
                float centerY = Height / 2.0f;
                g.DrawLine(centerPen, 0, centerY, Width, centerY);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            // Adjust maxSamples based on width for consistent bar spacing (targeting 2px per bar)
            maxSamples = Math.Max(15, Width / 2); // At least 15 samples for the cassette position
            
            // Trim or pad samples list
            lock (samplesLock)
            {
                if (audioSamples.Count > maxSamples)
                {
                    audioSamples = audioSamples.GetRange(audioSamples.Count - maxSamples, maxSamples);
                }
                else
                {
                    while (audioSamples.Count < maxSamples)
                    {
                        audioSamples.Insert(0, 0.0f);
                    }
                }
            }
            
            Invalidate();
        }
    }
}
