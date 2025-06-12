using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyMusicPlayer
{
    public class DigitalTimeLabel : Label
    {
        private int cornerRadius = 8;
        
        public int CornerRadius
        {
            get { return cornerRadius; }
            set 
            { 
                cornerRadius = value;
                Invalidate();
            }
        }

        public DigitalTimeLabel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent; // Set to transparent so we can draw our own background
            ForeColor = ColorTranslator.FromHtml("#D2691E");
            Font = new Font("Consolas", 14, FontStyle.Bold); // Using Consolas for better availability
            TextAlign = ContentAlignment.MiddleCenter;
            BorderStyle = BorderStyle.None; // Remove default border
        }

         protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Adjust rectangle to account for border pen width
            Rectangle drawingRect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Create rounded rectangle path
            using (GraphicsPath path = GetRoundedRectanglePath(drawingRect, cornerRadius))
            {
                // Fill background with black
                using (SolidBrush backgroundBrush = new SolidBrush(Color.Black))
                {
                    e.Graphics.FillPath(backgroundBrush, path);
                }

                // Draw border
                using (Pen borderPen = new Pen(Color.Gray, 1))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }

            // Calculate text position for perfect centering (use full ClientRectangle for text)
            SizeF textSize = e.Graphics.MeasureString(Text, Font);
            float x = (Width - textSize.Width) / 2;
            float y = (Height - textSize.Height) / 2;

            // Draw shadow/glow effect
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, ForeColor)))
            {
                e.Graphics.DrawString(Text, Font, shadowBrush, x + 1, y + 1);
            }

            // Draw main text
            using (SolidBrush textBrush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, textBrush, x, y);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Top-left corner
            path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            
            // Top edge
            path.AddLine(rectangle.X + radius, rectangle.Y, rectangle.Right - radius, rectangle.Y);
            
            // Top-right corner
            path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
            
            // Right edge
            path.AddLine(rectangle.Right, rectangle.Y + radius, rectangle.Right, rectangle.Bottom - radius);
            
            // Bottom-right corner
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            
            // Bottom edge
            path.AddLine(rectangle.Right - radius, rectangle.Bottom, rectangle.X + radius, rectangle.Bottom);
            
            // Bottom-left corner
            path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
            
            // Left edge
            path.AddLine(rectangle.X, rectangle.Bottom - radius, rectangle.X, rectangle.Y + radius);

            path.CloseFigure();
            return path;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // Force the control to be square by using the larger dimension
            int size = Math.Max(width, height);
            base.SetBoundsCore(x, y, size, size, specified);
        }
    }
}