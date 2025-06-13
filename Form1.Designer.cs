namespace MyMusicPlayer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private ActionButton btnPlay;
    private ActionButton btnStop;
    private ActionButton btnRandom;
    private TrackBar trackProgress;
    private DigitalTimeLabel lblCurrentTime;
    private DigitalTimeLabel lblTotalTime;
    private CircularDial dialCollection;
    private CircularDial dialYear;
    private System.Windows.Forms.ListView lstShows;
    private System.Windows.Forms.Label lblCurrentPath;
    private System.Windows.Forms.Label lblCurrentlyPlaying;
    private System.Windows.Forms.PictureBox picBottomBoard;
    private System.Windows.Forms.PictureBox picScreen;
    private SpinningCassette spinningCassette;

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
        this.btnPlay = new ActionButton();
        this.btnStop = new ActionButton();
        this.btnRandom = new ActionButton();
        this.trackProgress = new TrackBar();
        this.lblCurrentTime = new DigitalTimeLabel();
        this.lblTotalTime = new DigitalTimeLabel();
        this.dialCollection = new CircularDial();
        this.dialYear = new CircularDial();
        this.lstShows = new System.Windows.Forms.ListView();
        this.lblCurrentPath = new System.Windows.Forms.Label();
        this.lblCurrentlyPlaying = new System.Windows.Forms.Label();
        this.picBottomBoard = new System.Windows.Forms.PictureBox();
        this.picScreen = new System.Windows.Forms.PictureBox();
        this.spinningCassette = new SpinningCassette();

        // Screen image
        this.picScreen.Size = new System.Drawing.Size(650, 200);
        this.picScreen.Location = new System.Drawing.Point(70, 10);
        this.picScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.picScreen.BackColor = Color.Transparent;
        LoadScreenImage();

        // Spinning Cassette
        this.spinningCassette.Size = new System.Drawing.Size(360, 200);
        this.spinningCassette.Location = new System.Drawing.Point(220, 210);
        this.spinningCassette.BackColor = Color.Transparent;

        // bottom board image
        this.picBottomBoard.Size = new System.Drawing.Size(790, 220); // Adjust size as needed
        this.picBottomBoard.Location = new System.Drawing.Point(0, 410); // Position at the bottom
        this.picBottomBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.picBottomBoard.BackColor = Color.Transparent;

        // list of shows
        this.lstShows.Location = new System.Drawing.Point(140, 37);
        this.lstShows.Size = new System.Drawing.Size(520, 140);
        this.lstShows.View = System.Windows.Forms.View.Details;
        this.lstShows.FullRowSelect = true;
        this.lstShows.GridLines = false;
        this.lstShows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.lstShows.MultiSelect = false;
        this.lstShows.BackColor = Color.Black;
        this.lstShows.ForeColor = Color.White;
        this.lstShows.BorderStyle = BorderStyle.FixedSingle;
        this.lstShows.HideSelection = false;
        this.lstShows.Font = new Font("Consolas", 18, FontStyle.Bold);

        // Add a column for the ListView
        this.lstShows.Columns.Add("Shows", 500, System.Windows.Forms.HorizontalAlignment.Center);
        this.lstShows.DoubleClick += new System.EventHandler(this.LstShows_DoubleClick);
        this.lstShows.SelectedIndexChanged += new System.EventHandler(this.LstShows_SelectedIndexChanged);

        // Current path label 
        this.lblCurrentPath.Text = "";
        this.lblCurrentPath.Location = new System.Drawing.Point(262, 240);
        this.lblCurrentPath.Size = new System.Drawing.Size(270, 32);
        this.lblCurrentPath.Font = new Font("Times New Roman", 18, FontStyle.Italic);
        this.lblCurrentPath.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentPath.BackColor = Color.Black;
        this.lblCurrentPath.TextAlign = ContentAlignment.MiddleCenter;

        // Currently playing label
        this.lblCurrentlyPlaying.Text = "";
        this.lblCurrentlyPlaying.Location = new System.Drawing.Point(262, 322);
        this.lblCurrentlyPlaying.Size = new System.Drawing.Size(270, 32);
        this.lblCurrentlyPlaying.AutoSize = false;
        this.lblCurrentlyPlaying.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        this.lblCurrentlyPlaying.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentlyPlaying.BackColor = Color.Black;
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

        // Progress bar area with square digital time displays
        this.lblCurrentTime.Text = "00:00";
        this.lblCurrentTime.Location = new System.Drawing.Point(50, 480);
        this.lblCurrentTime.Size = new System.Drawing.Size(70, 70); // Perfect square

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
        this.lblTotalTime.Location = new System.Drawing.Point(660, 480);
        this.lblTotalTime.Size = new System.Drawing.Size(70, 70); // Perfect square
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

        // Add all controls to the form
        this.Controls.Add(this.lstShows);
        this.Controls.Add(this.picScreen);
        this.Controls.Add(this.lblCurrentPath);
        this.Controls.Add(this.lblCurrentlyPlaying);
        this.Controls.Add(this.spinningCassette);
        this.Controls.Add(this.dialCollection);
        this.Controls.Add(this.dialYear);
        this.Controls.Add(this.lblCurrentTime);
        this.Controls.Add(this.trackProgress);
        this.Controls.Add(this.lblTotalTime);
        this.Controls.Add(this.btnPlay);
        this.Controls.Add(this.btnStop);
        this.Controls.Add(this.btnRandom);
        this.Controls.Add(this.picBottomBoard);
    }
    
    private void SetBackgroundImage()
    {
        bool imageLoaded = false;
        
        string[] imagePaths = {
            Path.Combine(Application.StartupPath, "Images", "background.png"),
            Path.Combine(Directory.GetCurrentDirectory(), "Images", "background.png"),
            Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "background.png")
        };

        foreach (var path in imagePaths)
        {
            System.Diagnostics.Debug.WriteLine($"Trying background path: {path}");
            
            if (File.Exists(path))
            {
                try
                {
                    this.BackgroundImage = Image.FromFile(path);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                    System.Diagnostics.Debug.WriteLine($"SUCCESS! Background image loaded from: {path}");
                    imageLoaded = true;
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading background image from {path}: {ex.Message}");
                }
            }
        }
        
        if (!imageLoaded)
        {
            System.Diagnostics.Debug.WriteLine("Background image not found!");
            this.BackColor = Color.Black;
        }
    }

    private void LoadScreenImage()
    {
        bool screenLoaded = false;
        
        string[] screenPaths = {
            Path.Combine(Application.StartupPath, "Images", "screen.png"),
            Path.Combine(Directory.GetCurrentDirectory(), "Images", "screen.png"),
            Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "screen.png")
        };

        foreach (var path in screenPaths)
        {
            System.Diagnostics.Debug.WriteLine($"Trying screen path: {path}");
            
            if (File.Exists(path))
            {
                try
                {
                    this.picScreen.Image = Image.FromFile(path);
                    System.Diagnostics.Debug.WriteLine($"SUCCESS! Screen image loaded from: {path}");
                    screenLoaded = true;
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading screen image from {path}: {ex.Message}");
                }
            }
        }
        
        if (!screenLoaded)
        {
            System.Diagnostics.Debug.WriteLine("Screen image not found!");
            this.picScreen.Visible = false;
        }
    }

    #endregion
}