using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyMusicPlayer
{
    public class RoundedButton : Button
    {
        public int BorderRadius { get; set; } = 15; // Adjust this value for more/less rounding

        public RoundedButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(90, 50);
            Font = new Font("Segoe UI Symbol", 18, FontStyle.Bold);
            UseVisualStyleBackColor = false;
            ForeColor = Color.White;
        }

        private GraphicsPath GetRoundPath(RectangleF rect, float radius)
        {
            float x = rect.X, y = rect.Y, w = rect.Width, h = rect.Height;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(x, y, radius, radius, 180, 90);                 // Top left
            path.AddArc(x + w - radius, y, radius, radius, 270, 90);    // Top right
            path.AddArc(x + w - radius, y + h - radius, radius, radius, 0, 90);   // Bottom right
            path.AddArc(x, y + h - radius, radius, radius, 90, 90);     // Bottom left
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(0, 0, Width - 1, Height - 1);

            using (GraphicsPath path = GetRoundPath(rect, BorderRadius))
            {
                if (Enabled)
                {
                    // ENABLED STATE - Vintage radio button gradient
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect,
                        ColorTranslator.FromHtml("#CD853F"), // Sandy brown (lighter)
                        ColorTranslator.FromHtml("#8B4513"), // Saddle brown (darker)
                        LinearGradientMode.Vertical))
                    {
                        g.FillPath(brush, path);
                    }

                    // Draw bright border for enabled state
                    using (Pen borderPen = new Pen(ColorTranslator.FromHtml("#D2691E"), 2))
                    {
                        g.DrawPath(borderPen, path);
                    }

                    // Pressed effect for enabled buttons
                    if (Pressed)
                    {
                        using (SolidBrush pressBrush = new SolidBrush(Color.FromArgb(80, Color.Black)))
                        {
                            g.FillPath(pressBrush, path);
                        }
                    }
                }
                else
                {
                    // DISABLED STATE - Much darker and muted
                    using (LinearGradientBrush disabledBrush = new LinearGradientBrush(
                        rect,
                        ColorTranslator.FromHtml("#4A4A4A"), // Dark gray (lighter)
                        ColorTranslator.FromHtml("#2F2F2F"), // Darker gray (darker)
                        LinearGradientMode.Vertical))
                    {
                        g.FillPath(disabledBrush, path);
                    }

                    // Draw muted border for disabled state
                    using (Pen disabledBorderPen = new Pen(ColorTranslator.FromHtml("#666666"), 1))
                    {
                        g.DrawPath(disabledBorderPen, path);
                    }

                    // Add subtle inner shadow for "inset" disabled look
                    using (Pen innerShadowPen = new Pen(Color.FromArgb(60, Color.Black), 1))
                    {
                        RectangleF innerRect = new RectangleF(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
                        using (GraphicsPath innerPath = GetRoundPath(innerRect, BorderRadius - 1))
                        {
                            g.DrawPath(innerShadowPen, innerPath);
                        }
                    }
                }
            }

            // Draw text with different colors for enabled/disabled
            Color textColor = Enabled ? Color.White : ColorTranslator.FromHtml("#888888"); // Muted gray for disabled
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(Text, Font, textBrush, ClientRectangle, sf);
            }
        }

        private bool Pressed { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Enabled) // Only respond to mouse events when enabled
            {
                Pressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Enabled) // Only respond to mouse events when enabled
            {
                Pressed = false;
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate(); // Repaint when enabled state changes
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}