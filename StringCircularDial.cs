using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class StringCircularDial : Control
{
    private int _selectedIndex = 0;
    private List<string> _items = new List<string>();
    private Color _dialColor = Color.LightSteelBlue;
    private Color _knobColor = ColorTranslator.FromHtml("#D2691E");
    private Color _textColor = Color.Black;
    private Font _textFont = new Font("Arial", 12, FontStyle.Bold);
    private Image? _backgroundImage = null;
    private bool _useBackgroundImage = false;

    // Add properties for background image
    public Image? DialBackgroundImage
    {
        get => _backgroundImage;
        set
        {
            _backgroundImage = value;
            _useBackgroundImage = value != null;
            Invalidate();
        }
    }

    public bool UseBackgroundImage
    {
        get => _useBackgroundImage;
        set
        {
            _useBackgroundImage = value && _backgroundImage != null;
            Invalidate();
        }
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value >= 0 && value < _items.Count)
            {
                _selectedIndex = value;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public string? SelectedItem
    {
        get => _items.Count > 0 && _selectedIndex >= 0 && _selectedIndex < _items.Count 
               ? _items[_selectedIndex] : null;
    }

    public List<string> Items
    {
        get => _items;
        set
        {
            _items = value ?? new List<string>();
            if (_selectedIndex >= _items.Count)
                _selectedIndex = _items.Count > 0 ? 0 : -1;
            Invalidate();
        }
    }

    public event EventHandler? SelectedIndexChanged;

    public StringCircularDial()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.UserPaint | 
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
        Size = new Size(200, 200);
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        // Draw the dial background with more padding to accommodate the knob indicator
        Rectangle dialRect = new Rectangle(0, 0, Width, Height);

        if (_useBackgroundImage && _backgroundImage != null)
        {
            // Create a circular clipping region
            using (GraphicsPath clipPath = new GraphicsPath())
            {
                clipPath.AddEllipse(dialRect);
                g.SetClip(clipPath);

                // Draw the background image, scaled to fit the circle
                g.DrawImage(_backgroundImage, dialRect);

                // Reset clipping
                g.ResetClip();
            }
        }

        // Draw tick marks for each item with adjusted radius calculations
        if (_items.Count > 0)
        {
            float centerX = Width / 2f;
            float centerY = Height / 2f;
            float outerRadius = Math.Min(Width, Height) / 2f - 8;
            float innerRadius = outerRadius -20;

            for (int i = 0; i < _items.Count; i++)
            {
                float angle = (float)i / _items.Count * 360f - 90f; // Start from top
                float radians = angle * (float)Math.PI / 180f;

                float x1 = centerX + innerRadius * (float)Math.Cos(radians);
                float y1 = centerY + innerRadius * (float)Math.Sin(radians);
                float x2 = centerX + outerRadius * (float)Math.Cos(radians);
                float y2 = centerY + outerRadius * (float)Math.Sin(radians);

                using (Pen tickPen = new Pen(i == _selectedIndex ? Color.Red : (_useBackgroundImage ? Color.White : Color.Gray),
                                        i == _selectedIndex ? 3 : (_useBackgroundImage ? 2 : 1)))
                {
                    g.DrawLine(tickPen, x1, y1, x2, y2);
                }
            }

            // Draw the selected item text in center with outline, wrapping, and larger font
            if (_selectedIndex >= 0 && _selectedIndex < _items.Count)
            {
                string selectedText = _items[_selectedIndex];

                // Define the text area (larger for better text wrapping)
                float textAreaSize = Math.Min(Width, Height) * 0.45f;

                // Adjust the text rectangle position - move down and left for better centering
                float offsetX = -3f; // Move 5 pixels to the left
                float offsetY = 5f;  // Move 5 pixels down

                RectangleF textRect = new RectangleF(
                    centerX - textAreaSize / 2 + offsetX,
                    centerY - textAreaSize / 2 + offsetY,
                    textAreaSize,
                    textAreaSize);

                // Create StringFormat for center alignment and word wrapping
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.FormatFlags = StringFormatFlags.NoClip;
                    stringFormat.Trimming = StringTrimming.Word;

                    // First, try to draw with regular DrawString for proper wrapping
                    // Measure the text to see if it fits
                    SizeF textSize = g.MeasureString(selectedText, _textFont, (int)textAreaSize, stringFormat);

                    // If text is too large, use a smaller font temporarily
                    Font displayFont = _textFont;
                    if (textSize.Height > textAreaSize)
                    {
                        // Create a smaller font if text doesn't fit
                        float scaleFactor = textAreaSize / textSize.Height * 0.8f; // 80% to give some breathing room
                        float newSize = Math.Max(8, _textFont.Size * scaleFactor); // Minimum size of 8
                        displayFont = new Font(_textFont.FontFamily, newSize, _textFont.Style);
                    }

                    // Draw text with outline effect using DrawString for proper wrapping
                    // First draw multiple times in white for outline effect
                    using (Brush outlineBrush = new SolidBrush(Color.FromArgb(128, Color.White)))
                    {
                        // Draw outline by drawing text multiple times with slight offsets
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -2; y <= 2; y++)
                            {
                                if (x != 0 || y != 0) // Don't draw at center position
                                {
                                    RectangleF outlineRect = new RectangleF(
                                        textRect.X + x, textRect.Y + y,
                                        textRect.Width, textRect.Height);
                                    g.DrawString(selectedText, displayFont, outlineBrush, outlineRect, stringFormat);
                                }
                            }
                        }
                    }

                    // Draw the main text in black
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(selectedText, displayFont, textBrush, textRect, stringFormat);
                    }

                    // Dispose temporary font if we created one
                    if (displayFont != _textFont)
                    {
                        displayFont.Dispose();
                    }
                }
            }

            // Draw the knob/pointer with adjusted positioning to stay within bounds
            if (_items.Count > 0)
            {
                float knobAngle = (float)_selectedIndex / _items.Count * 360f - 90f;
                float knobRadians = knobAngle * (float)Math.PI / 180f;
                float knobRadius = outerRadius - 2;
                float knobX = centerX + knobRadius * (float)Math.Cos(knobRadians);
                float knobY = centerY + knobRadius * (float)Math.Sin(knobRadians);

                Rectangle knobRect = new Rectangle((int)knobX - 8, (int)knobY - 8, 16, 16);
                using (Brush knobBrush = new SolidBrush(_knobColor))
                {
                    g.FillEllipse(knobBrush, knobRect);
                }
                using (Pen knobBorder = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(knobBorder, knobRect);
                }
            }
        }
        else
        {
            // No items - show "Empty" message in black with outline
            string emptyText = "Empty";
            RectangleF emptyRect = new RectangleF(0, 0, Width, Height);

            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Draw outline for empty text
                using (Brush outlineBrush = new SolidBrush(Color.White))
                {
                    for (int x = -2; x <= 2; x++)
                    {
                        for (int y = -2; y <= 2; y++)
                        {
                            if (x != 0 || y != 0)
                            {
                                RectangleF outlineRect = new RectangleF(
                                    emptyRect.X + x, emptyRect.Y + y,
                                    emptyRect.Width, emptyRect.Height);
                                g.DrawString(emptyText, _textFont, outlineBrush, outlineRect, stringFormat);
                            }
                        }
                    }
                }

                // Draw main empty text
                using (Brush textBrush = new SolidBrush(Color.Black))
                {
                    g.DrawString(emptyText, _textFont, textBrush, emptyRect, stringFormat);
                }
            }
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && _items.Count > 0)
        {
            UpdateSelectedIndexFromMouse(e.Location);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && _items.Count > 0)
        {
            UpdateSelectedIndexFromMouse(e.Location);
        }
    }

    private void UpdateSelectedIndexFromMouse(Point mouseLocation)
    {
        if (_items.Count == 0) return;

        float centerX = Width / 2f;
        float centerY = Height / 2f;
        float deltaX = mouseLocation.X - centerX;
        float deltaY = mouseLocation.Y - centerY;
        float angle = (float)Math.Atan2(deltaY, deltaX) * 180f / (float)Math.PI;
        
        // Normalize angle to 0-360 range, starting from top
        angle += 90f;
        if (angle < 0) angle += 360f;
        if (angle >= 360) angle -= 360f;
        
        // Calculate which item this angle corresponds to
        int newIndex = (int)Math.Round(angle / 360f * _items.Count) % _items.Count;
        SelectedIndex = newIndex;
    }

    public void ClearItems()
    {
        _items.Clear();
        _selectedIndex = -1;
        Invalidate();
    }

    public void AddItem(string item)
    {
        _items.Add(item);
        if (_selectedIndex == -1)
            _selectedIndex = 0;
        Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _textFont?.Dispose();
            _backgroundImage?.Dispose(); // Dispose background image
        }
        base.Dispose(disposing);
    }
}