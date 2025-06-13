using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Windows.Forms;

public class SpinningCassette : Control
{
    private Image? cassetteImage;
    private System.Windows.Forms.Timer animationTimer;
    private float leftWheelAngle = 0f;
    private float rightWheelAngle = 0f;
    private bool isSpinning = false;

    // Define the wheel positions (you'll need to adjust these based on your cassette image)
    private Rectangle leftWheelRect = new Rectangle(81, 63, 50, 50);   // Adjust position/size
    private Rectangle rightWheelRect = new Rectangle(224, 63, 50, 50); // Adjust position/size

    public SpinningCassette()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.UserPaint | 
                 ControlStyles.DoubleBuffer | 
                 ControlStyles.SupportsTransparentBackColor, true);

        BackColor = Color.Transparent;

        // Set up animation timer
        animationTimer = new System.Windows.Forms.Timer();
        animationTimer.Interval = 50; // 20 FPS
        animationTimer.Tick += AnimationTimer_Tick;
    }

    public Image? CassetteImage
    {
        get => cassetteImage;
        set
        {
            cassetteImage = value;
            Invalidate();
        }
    }

    public bool IsSpinning
    {
        get => isSpinning;
        set
        {
            if (isSpinning != value)
            {
                isSpinning = value;
                if (isSpinning)
                {
                    animationTimer.Start();
                }
                else
                {
                    animationTimer.Stop();
                }
            }
        }
    }

    // Adjust wheel positions if needed
    public void SetWheelPositions(Rectangle leftWheel, Rectangle rightWheel)
    {
        leftWheelRect = leftWheel;
        rightWheelRect = rightWheel;
        Invalidate();
    }

    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        if (isSpinning)
        {
            // Rotate wheels clockwise (different speeds for visual interest)
            leftWheelAngle += 5f;   // Degrees per frame
            rightWheelAngle += 4f;  // Slightly different speed

            // Keep angles in 0-360 range
            if (leftWheelAngle >= 360f) leftWheelAngle -= 360f;
            if (rightWheelAngle >= 360f) rightWheelAngle -= 360f;

            Invalidate(); // Trigger repaint
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (cassetteImage == null) return;

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Draw the main cassette image
        g.DrawImage(cassetteImage, 0, 0, Width, Height);

        if (isSpinning)
        {
            // Draw spinning wheel overlays
            DrawSpinningWheel(g, leftWheelRect, leftWheelAngle);
            DrawSpinningWheel(g, rightWheelRect, rightWheelAngle);
        }
    }

private void DrawSpinningWheel(Graphics g, Rectangle wheelRect, float angle)
{
    // Save the current graphics state
    GraphicsState state = g.Save();

    // Move to the center of the wheel
    g.TranslateTransform(wheelRect.X + wheelRect.Width / 2f, 
                       wheelRect.Y + wheelRect.Height / 2f);
    
    // Rotate by the current angle
    g.RotateTransform(angle);

    // Draw the wheel spokes/lines to show rotation - MADE THICKER
    using (Pen spokePen = new Pen(Color.FromArgb(200, 255, 255, 255), 3f)) // CHANGED: Increased from 2f to 3f
    {
        int spokeLength = wheelRect.Width / 3;
        
        // Draw 6 spokes at 60-degree intervals
        for (int i = 0; i < 6; i++)
        {
            float spokeAngle = i * 60f;
            float radians = spokeAngle * (float)(Math.PI / 180);
            
            float x1 = (float)Math.Cos(radians) * (spokeLength / 2);
            float y1 = (float)Math.Sin(radians) * (spokeLength / 2);
            float x2 = (float)Math.Cos(radians) * spokeLength;
            float y2 = (float)Math.Sin(radians) * spokeLength;
            
            g.DrawLine(spokePen, x1, y1, x2, y2);
        }

        // Draw center circle
        int centerSize = 8;
        using (SolidBrush centerBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255))) // Semi-transparent white
        {
            g.FillEllipse(centerBrush, -centerSize/2, -centerSize/2, centerSize, centerSize);
        }
    }

    // Restore the graphics state to remove rotation
    g.Restore(state);

    // Draw the outer white circle (non-rotating) - THICKER
    using (Pen circlePen = new Pen(Color.FromArgb(200, 255, 255, 255), 4f)) // Thick outer circle
    {
        int spokeLength = wheelRect.Width / 3;
        int circleSize = spokeLength * 2; // Circle diameter matches the spoke length
        int offset = (wheelRect.Width - circleSize) / 2; // Center the circle
        Rectangle circleRect = new Rectangle(
            wheelRect.X + offset, 
            wheelRect.Y + offset, 
            circleSize, 
            circleSize
        );
        
        g.DrawEllipse(circlePen, circleRect);
    }
}

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            animationTimer?.Dispose();
            cassetteImage?.Dispose();
        }
        base.Dispose(disposing);
    }
}