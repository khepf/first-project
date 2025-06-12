namespace MyMusicPlayer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private RoundedButton btnPlay;
    private RoundedButton btnStop;
    private RoundedButton btnRandom;
    private CustomTrackBar trackProgress;
    private System.Windows.Forms.Label lblCurrentTime;
    private System.Windows.Forms.Label lblTotalTime; 
    private StringCircularDial dialCollection;
    private StringCircularDial dialYear;
    private System.Windows.Forms.ListView lstShows;
    private System.Windows.Forms.Label lblCurrentPath;
    private System.Windows.Forms.Label lblCurrentlyPlaying;
    private System.Windows.Forms.PictureBox picCassette;
    private System.Windows.Forms.PictureBox picBottomBoard;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 650);

        this.SetBackgroundImage();

        // Use FixedDialog to completely prevent resizing
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = true; // Keep minimize button

        // Set the form to start at a specific position
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Location = new System.Drawing.Point(1920, 200);

        // Initialize Material UI Components
        this.btnPlay = new RoundedButton();
        this.btnStop = new RoundedButton();
        this.btnRandom = new RoundedButton(); 
        this.trackProgress = new CustomTrackBar();
        this.lblCurrentTime = new System.Windows.Forms.Label();
        this.lblTotalTime = new System.Windows.Forms.Label();
        this.dialCollection = new StringCircularDial();
        this.dialYear = new StringCircularDial();
        this.lstShows = new System.Windows.Forms.ListView();
        this.lblCurrentPath = new System.Windows.Forms.Label();
        this.lblCurrentlyPlaying = new System.Windows.Forms.Label();
        this.picCassette = new System.Windows.Forms.PictureBox();
        this.picBottomBoard = new System.Windows.Forms.PictureBox();

        // Cassette image
        this.picCassette.Size = new System.Drawing.Size(360, 216);
        this.picCassette.Location = new System.Drawing.Point(220, 210);
        this.picCassette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picCassette.BackColor = Color.Transparent;

        // bottom board image
        this.picBottomBoard.Size = new System.Drawing.Size(784, 220); // Adjust size as needed
        this.picBottomBoard.Location = new System.Drawing.Point(0, 410); // Position at the bottom
        this.picBottomBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.picBottomBoard.BackColor = Color.Transparent;

        // list of shows
        this.lstShows.Location = new System.Drawing.Point(90, 10);
        this.lstShows.Size = new System.Drawing.Size(600, 200);
        this.lstShows.View = System.Windows.Forms.View.Details;
        this.lstShows.FullRowSelect = true;
        this.lstShows.GridLines = false;
        this.lstShows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.lstShows.MultiSelect = false;
        this.lstShows.BackColor = Color.Black;
        this.lstShows.ForeColor = Color.White;
        this.lstShows.BorderStyle = BorderStyle.FixedSingle;
        this.lstShows.HideSelection = false;
        this.lstShows.Font = new Font("Times New Roman", 18, FontStyle.Italic);

        // Add a column for the ListView
        this.lstShows.Columns.Add("Shows", 600, System.Windows.Forms.HorizontalAlignment.Center);
        this.lstShows.DoubleClick += new System.EventHandler(this.LstShows_DoubleClick);
        this.lstShows.SelectedIndexChanged += new System.EventHandler(this.LstShows_SelectedIndexChanged);

        // Current path label 
        this.lblCurrentPath.Text = "";
        this.lblCurrentPath.Location = new System.Drawing.Point(266, 240);
        this.lblCurrentPath.AutoSize = true;
        this.lblCurrentPath.Font = new Font("Times New Roman", 18, FontStyle.Italic);
        this.lblCurrentPath.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentPath.BackColor = Color.Transparent;
        this.lblCurrentPath.TextAlign = ContentAlignment.MiddleCenter;

        // Currently playing label
        this.lblCurrentlyPlaying.Text = "";
        this.lblCurrentlyPlaying.Location = new System.Drawing.Point(260, 332);
        this.lblCurrentlyPlaying.Size = new System.Drawing.Size(270, 32);
        this.lblCurrentlyPlaying.AutoSize = false;
        this.lblCurrentlyPlaying.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        this.lblCurrentlyPlaying.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentlyPlaying.BackColor = Color.Transparent;
        this.lblCurrentlyPlaying.TextAlign = ContentAlignment.MiddleCenter;
        this.lblCurrentlyPlaying.BorderStyle = BorderStyle.None;

        // Collection dial (Bands)
        this.dialCollection.Location = new System.Drawing.Point(0, 210);
        // this.dialCollection.Size = new System.Drawing.Size(120, 120);
        this.dialCollection.SelectedIndexChanged += new System.EventHandler(this.DialCollection_SelectedIndexChanged);

        // Year dial
        this.dialYear.Location = new System.Drawing.Point(580, 210);
        // this.dialYear.Size = new System.Drawing.Size(120, 120);
        this.dialYear.SelectedIndexChanged += new System.EventHandler(this.DialYear_SelectedIndexChanged);

        // Progress bar area
        this.lblCurrentTime.Text = "00:00";
        this.lblCurrentTime.Location = new System.Drawing.Point(16, 560);
        this.lblCurrentTime.Size = new System.Drawing.Size(50, 23);
        this.lblCurrentTime.BackColor = Color.Transparent;
        this.lblCurrentTime.ForeColor = Color.White;
        this.lblCurrentTime.Font = new Font("Arial", 10, FontStyle.Bold);

        this.trackProgress.Location = new System.Drawing.Point(76, 560);
        this.trackProgress.Size = new System.Drawing.Size(640, 45);
        this.trackProgress.Minimum = 0;
        this.trackProgress.Maximum = 100;
        this.trackProgress.TabStop = false;
        this.trackProgress.BackColor = Color.Black;
        this.trackProgress.ValueChanged += new System.EventHandler(this.TrackProgress_ValueChanged);
        this.trackProgress.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackProgress_MouseDown);
        this.trackProgress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackProgress_MouseUp);

        this.lblTotalTime.Text = "00:00";
        this.lblTotalTime.Location = new System.Drawing.Point(726, 560);
        this.lblTotalTime.Size = new System.Drawing.Size(50, 23);
        this.lblTotalTime.BackColor = Color.Transparent;
        this.lblTotalTime.ForeColor = Color.White;
        this.lblTotalTime.Font = new Font("Arial", 10, FontStyle.Bold);

        // Combined Play/Pause button - centered position
        this.btnPlay.Text = "▶";
        this.btnPlay.Location = new System.Drawing.Point(284, 478);
        this.btnPlay.Size = new System.Drawing.Size(70, 70);
        this.btnPlay.Click += new System.EventHandler(this.BtnPlayPause_Click);

        // Stop button - moved closer to play button
        this.btnStop.Text = "⏹";
        this.btnStop.Location = new System.Drawing.Point(362, 478);
        this.btnStop.Size = new System.Drawing.Size(70, 70);
        this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);

        // Random button - NEW
        this.btnRandom.Text = "🎲";
        this.btnRandom.Location = new System.Drawing.Point(440, 478);
        this.btnRandom.Size = new System.Drawing.Size(70, 70);
        this.btnRandom.Font = new Font("Segoe UI Symbol", 24, FontStyle.Bold);
        this.btnRandom.Click += new System.EventHandler(this.BtnRandom_Click);

        // Add controls to the form
        this.Controls.Add(this.btnPlay);
        this.Controls.Add(this.btnStop);
        this.Controls.Add(this.btnRandom); 
        this.Controls.Add(this.trackProgress);
        this.Controls.Add(this.dialCollection);
        this.Controls.Add(this.dialYear);
        this.Controls.Add(this.lblCurrentPath);
        this.Controls.Add(this.picCassette);
        this.Controls.Add(this.lblCurrentTime);
        this.Controls.Add(this.lblTotalTime);
        this.Controls.Add(this.picBottomBoard);

        this.Controls.Add(this.lstShows);
        this.Controls.Add(this.lblCurrentlyPlaying);

        this.lblCurrentlyPlaying.BringToFront();
        this.lblCurrentlyPlaying.Visible = true;

        // Resume layout
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    
    private void SetBackgroundImage()
{
    // Try multiple possible paths for background.png
    string[] possiblePaths = {
        Path.Combine(Application.StartupPath, "Images", "background.png"),
        Path.Combine(Directory.GetCurrentDirectory(), "Images", "background.png"),
        Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "background.png")
    };
    
    bool imageLoaded = false;
    
    foreach (var path in possiblePaths)
    {
        if (File.Exists(path))
        {
            try
            {
                this.BackgroundImage = Image.FromFile(path);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                imageLoaded = true;
                break;
            }
            catch
            {
                // If loading fails, continue to next path
            }
        }
    }
    
    if (!imageLoaded)
    {
        // Fallback to solid color if image not found
        this.BackColor = Color.FromArgb(20, 20, 20); // Dark gray
    }
}

    #endregion
}