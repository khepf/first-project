using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyMusicPlayer
{
    public class CustomTrackBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private bool _isDragging = false;
        private Rectangle _thumbRect;
        private readonly Color _bronzeColor = ColorTranslator.FromHtml("#D2691E");
        private readonly Color _bronzeDark = ColorTranslator.FromHtml("#8B4513");

        public event EventHandler? ValueChanged;
        // Remove these conflicting event declarations
        // public new event MouseEventHandler? MouseDown;
        // public new event MouseEventHandler? MouseUp;

        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                int newValue = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != newValue)
                {
                    _value = newValue;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public CustomTrackBar()
        {
            try
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | 
                        ControlStyles.UserPaint | 
                        ControlStyles.DoubleBuffer | 
                        ControlStyles.ResizeRedraw, true);
                
                Size = new Size(200, 45);
                BackColor = Color.Black;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CustomTrackBar constructor error: {ex.Message}");
                // Set minimal defaults
                Size = new Size(200, 45);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Add validation for control size
            if (Width <= 20 || Height <= 0)
            {
                // Control not properly sized yet, skip painting
                return;
            }
            
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Calculate track rectangle
            int trackHeight = 6;
            Rectangle trackRect = new Rectangle(
                10, 
                (Height - trackHeight) / 2, 
                Width - 20, 
                trackHeight
            );

            // Additional validation for track rectangle
            if (trackRect.Width <= 0 || trackRect.Height <= 0)
            {
                return;
            }

            // Draw track background (dark)
            using (SolidBrush trackBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
            {
                g.FillRoundedRectangle(trackBrush, trackRect, trackHeight / 2);
            }

            // Draw track border
            using (Pen trackPen = new Pen(Color.FromArgb(100, 100, 100), 1))
            {
                g.DrawRoundedRectangle(trackPen, trackRect, trackHeight / 2);
            }

            // Calculate thumb position
            float percentage = (_maximum > _minimum) ? (float)(_value - _minimum) / (_maximum - _minimum) : 0;
            int thumbX = trackRect.X + (int)(percentage * trackRect.Width);
            int thumbSize = 20;
            
            _thumbRect = new Rectangle(
                thumbX - thumbSize / 2,
                (Height - thumbSize) / 2,
                thumbSize,
                thumbSize
            );

            // Draw progress fill (bronze gradient)
            if (percentage > 0)
            {
                Rectangle fillRect = new Rectangle(
                    trackRect.X,
                    trackRect.Y,
                    thumbX - trackRect.X,
                    trackRect.Height
                );

                // Validate fill rectangle
                if (fillRect.Width > 0 && fillRect.Height > 0)
                {
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        fillRect,
                        _bronzeColor,
                        _bronzeDark,
                        LinearGradientMode.Horizontal))
                    {
                        g.FillRoundedRectangle(fillBrush, fillRect, trackHeight / 2);
                    }
                }
            }

            // Draw thumb (bronze gradient)
            if (_thumbRect.Width > 0 && _thumbRect.Height > 0)
            {
                using (LinearGradientBrush thumbBrush = new LinearGradientBrush(
                    _thumbRect,
                    Color.FromArgb(255, 205, 133, 63), // Lighter bronze
                    _bronzeDark,
                    LinearGradientMode.Vertical))
                {
                    g.FillEllipse(thumbBrush, _thumbRect);
                }

                // Draw thumb border
                using (Pen thumbPen = new Pen(_bronzeDark, 2))
                {
                    g.DrawEllipse(thumbPen, _thumbRect);
                }

                // Draw thumb highlight
                using (Pen highlightPen = new Pen(Color.FromArgb(100, Color.White), 1))
                {
                    Rectangle highlightRect = new Rectangle(
                        _thumbRect.X + 3,
                        _thumbRect.Y + 3,
                        _thumbRect.Width - 6,
                        _thumbRect.Height - 6
                    );
                    
                    if (highlightRect.Width > 0 && highlightRect.Height > 0)
                    {
                        g.DrawEllipse(highlightPen, highlightRect);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            
            if (e.Button == MouseButtons.Left)
            {
                if (_thumbRect.Contains(e.Location))
                {
                    _isDragging = true;
                }
                else
                {
                    // Click on track - jump to position
                    UpdateValueFromMouse(e.X);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (_isDragging)
            {
                UpdateValueFromMouse(e.X);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            
            if (_isDragging)
            {
                _isDragging = false;
            }
        }

        private void UpdateValueFromMouse(int mouseX)
        {
            int trackStart = 10;
            int trackWidth = Width - 20;
            
            // Validate track width
            if (trackWidth <= 0)
            {
                return;
            }
            
            float percentage = Math.Max(0, Math.Min(1, (float)(mouseX - trackStart) / trackWidth));
            Value = _minimum + (int)(percentage * (_maximum - _minimum));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate(); // Redraw on resize
        }
    }

    // Extension methods for rounded rectangles
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, radius))
            {
                graphics.FillPath(brush, path);
            }
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, radius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            
            // Validate rectangle size
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                // Return empty path for invalid rectangles
                return path;
            }
            
            int diameter = radius * 2;
            
            // Ensure radius doesn't exceed rectangle dimensions
            diameter = Math.Min(diameter, Math.Min(rect.Width, rect.Height));
            
            if (diameter <= 0)
            {
                // Fall back to regular rectangle
                path.AddRectangle(rect);
                return path;
            }
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }
    }
}