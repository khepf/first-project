﻿namespace MyMusicPlayer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblVolume;
    private TrackBar trackVolume;
    private ActionButton btnSettings;
    private ActionButton btnHelp;
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
    private WaveformDisplay waveformDisplay;

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
        
        // Dispose of our AudioPlayer
        audioPlayer?.Dispose();
        
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 650);

        this.SetBackgroundImage();

        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = true;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

        // Set the application icon
        this.SetApplicationIcon();

        this.lblVolume = new System.Windows.Forms.Label();
        this.trackVolume = new TrackBar();
        this.btnSettings = new ActionButton();
        this.btnHelp = new ActionButton();
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
        this.waveformDisplay = new WaveformDisplay();

        // Settings button - top right corner
        this.btnSettings.Text = "⭐";
        this.btnSettings.Location = new System.Drawing.Point(750, 10);
        this.btnSettings.Size = new System.Drawing.Size(40, 40);
        this.btnSettings.BorderRadius = 8;
        this.btnSettings.Font = new Font("Segoe UI Emoji", 16F, FontStyle.Regular);
        this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
        this.btnSettings.UseVisualStyleBackColor = true;

        // Help button - positioned below the settings button
        this.btnHelp.Text = "❓";
        this.btnHelp.Location = new System.Drawing.Point(750, 55);
        this.btnHelp.Size = new System.Drawing.Size(40, 40);
        this.btnHelp.BorderRadius = 8;
        this.btnHelp.Font = new Font("Segoe UI Emoji", 16F, FontStyle.Regular);
        this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
        this.btnHelp.UseVisualStyleBackColor = true;

        // TV Screen image
        this.picScreen.Size = new System.Drawing.Size(650, 220);
        this.picScreen.Location = new System.Drawing.Point(70, 10);
        this.picScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.picScreen.BackColor = Color.Transparent;
        LoadScreenImage();

        // Spinning Cassette
        this.spinningCassette.Size = new System.Drawing.Size(340, 200);
        this.spinningCassette.Location = new System.Drawing.Point(232, 234);
        this.spinningCassette.BackColor = Color.Transparent;

        // Waveform display - positioned between cassette wheels
        this.waveformDisplay.Location = new System.Drawing.Point(132, 65);
        this.waveformDisplay.Size = new System.Drawing.Size(74, 48);

        // bottom wood board image
        this.picBottomBoard.Size = new System.Drawing.Size(800, 220);
        this.picBottomBoard.Location = new System.Drawing.Point(0, 430);
        this.picBottomBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.picBottomBoard.BackColor = Color.Transparent;

        // 'current show playing' progress track bar - coordinates relative to picBottomBoard
        this.trackProgress.Location = new System.Drawing.Point(76, -10);
        this.trackProgress.Size = new System.Drawing.Size(640, 45);
        this.trackProgress.Minimum = 0;
        this.trackProgress.Maximum = 100;
        this.trackProgress.TabStop = false;
        this.trackProgress.ValueChanged += new System.EventHandler(this.TrackProgress_ValueChanged);
        this.trackProgress.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackProgress_MouseDown);
        this.trackProgress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackProgress_MouseUp);

        // list of shows
        this.lstShows.Location = new System.Drawing.Point(132, 48);
        this.lstShows.Size = new System.Drawing.Size(518, 150);
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

        // Current path label (Band and Year)
        this.lblCurrentPath.Text = "";
        this.lblCurrentPath.Location = new System.Drawing.Point(256, 260);
        this.lblCurrentPath.Size = new System.Drawing.Size(288, 32);
        this.lblCurrentPath.Font = new Font("Times New Roman", 18, FontStyle.Italic);
        this.lblCurrentPath.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentPath.BackColor = Color.Black;
        this.lblCurrentPath.TextAlign = ContentAlignment.MiddleCenter;

        // Currently playing label (complete file name)
        this.lblCurrentlyPlaying.Text = "";
        this.lblCurrentlyPlaying.Location = new System.Drawing.Point(256, 346);
        this.lblCurrentlyPlaying.Size = new System.Drawing.Size(288, 32);
        this.lblCurrentlyPlaying.AutoSize = false;
        this.lblCurrentlyPlaying.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        this.lblCurrentlyPlaying.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblCurrentlyPlaying.BackColor = Color.Black;
        this.lblCurrentlyPlaying.TextAlign = ContentAlignment.MiddleCenter;
        this.lblCurrentlyPlaying.BorderStyle = BorderStyle.None;

        // Collection dial (Bands)
        this.dialCollection.Location = new System.Drawing.Point(20, 232);
        this.dialCollection.SelectedIndexChanged += new System.EventHandler(this.DialCollection_SelectedIndexChanged);

        // Year dial
        this.dialYear.Location = new System.Drawing.Point(580, 232);
        this.dialYear.SelectedIndexChanged += new System.EventHandler(this.DialYear_SelectedIndexChanged);

        // square digital time display (current time of show) - coordinates relative to picBottomBoard
        this.lblCurrentTime.Text = "00:00";
        this.lblCurrentTime.Location = new System.Drawing.Point(50, 70);
        this.lblCurrentTime.Size = new System.Drawing.Size(70, 70);

        // square digital time display (total time duration of show) - coordinates relative to picBottomBoard
        this.lblTotalTime.Text = "00:00";
        this.lblTotalTime.Location = new System.Drawing.Point(660, 70);
        this.lblTotalTime.Size = new System.Drawing.Size(70, 70);

        // Combined Play/Pause button - coordinates relative to picBottomBoard
        this.btnPlay.Text = "▶";
        this.btnPlay.Location = new System.Drawing.Point(280, 68);
        this.btnPlay.Size = new System.Drawing.Size(70, 70);
        this.btnPlay.Click += new System.EventHandler(this.BtnPlayPause_Click);

        // Stop button - coordinates relative to picBottomBoard
        this.btnStop.Text = "⏹";
        this.btnStop.Location = new System.Drawing.Point(368, 68);
        this.btnStop.Size = new System.Drawing.Size(70, 70);
        this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);

        // Random button - coordinates relative to picBottomBoard
        this.btnRandom.Text = "🎲";
        this.btnRandom.Location = new System.Drawing.Point(456, 68);
        this.btnRandom.Size = new System.Drawing.Size(70, 70);
        this.btnRandom.Font = new Font("Segoe UI Symbol", 24, FontStyle.Bold);
        this.btnRandom.Click += new System.EventHandler(this.BtnRandom_Click);

        // Volume label - coordinates relative to picBottomBoard
        this.lblVolume.Text = "🔊"; // Speaker icon
        this.lblVolume.Location = new System.Drawing.Point(168, 146);
        this.lblVolume.Size = new System.Drawing.Size(30, 30);
        this.lblVolume.Font = new Font("Segoe UI Emoji", 14, FontStyle.Regular);
        this.lblVolume.ForeColor = ColorTranslator.FromHtml("#D2691E");
        this.lblVolume.BackColor = Color.Transparent;
        this.lblVolume.TextAlign = ContentAlignment.MiddleCenter;
        this.lblVolume.Cursor = Cursors.Hand;
        this.lblVolume.BorderStyle = BorderStyle.None;
        this.lblVolume.FlatStyle = FlatStyle.Flat;
        this.lblVolume.AutoSize = false;
        this.lblVolume.Click += new System.EventHandler(this.LblVolume_Click);

        // Volume bar - coordinates relative to picBottomBoard
        this.trackVolume.Location = new System.Drawing.Point(200, 148);
        this.trackVolume.Size = new System.Drawing.Size(400, 30);
        this.trackVolume.Minimum = 0;
        this.trackVolume.Maximum = 100;
        this.trackVolume.Value = 70;
        this.trackVolume.TabStop = false;
        this.trackVolume.ValueChanged += new System.EventHandler(this.TrackVolume_ValueChanged);

        // Add all controls to the form
        this.Controls.Add(this.picBottomBoard); // Add parent first
        
        // Add all bottom board controls as children of picBottomBoard
        this.picBottomBoard.Controls.Add(this.trackProgress);
        this.picBottomBoard.Controls.Add(this.lblCurrentTime);
        this.picBottomBoard.Controls.Add(this.lblTotalTime);
        this.picBottomBoard.Controls.Add(this.btnPlay);
        this.picBottomBoard.Controls.Add(this.btnStop);
        this.picBottomBoard.Controls.Add(this.btnRandom);
        this.picBottomBoard.Controls.Add(this.trackVolume);
        this.picBottomBoard.Controls.Add(this.lblVolume);

        // Add waveform display to the cassette (not bottom board)
        this.spinningCassette.Controls.Add(this.waveformDisplay);

        // Add remaining controls directly to the form
        this.Controls.Add(this.btnSettings);
        this.Controls.Add(this.btnHelp);
        this.Controls.Add(this.lstShows);
        this.Controls.Add(this.picScreen);
        this.Controls.Add(this.lblCurrentPath);
        this.Controls.Add(this.lblCurrentlyPlaying);
        this.Controls.Add(this.spinningCassette);
        this.Controls.Add(this.dialCollection);
        this.Controls.Add(this.dialYear);
    }
    
    private void SetBackgroundImage()
    {
        var backgroundImage = LoadEmbeddedImage("background.png");
        if (backgroundImage != null)
        {
            this.BackgroundImage = backgroundImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            System.Diagnostics.Debug.WriteLine("Background image loaded from embedded resources");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Background image not found in embedded resources!");
            this.BackColor = Color.Black;
        }
    }
    private void LoadScreenImage()
    {
        var screenImage = LoadEmbeddedImage("screen.png");
        if (screenImage != null)
        {
            this.picScreen.Image = screenImage;
            System.Diagnostics.Debug.WriteLine("Screen image loaded from embedded resources");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Screen image not found in embedded resources!");
            this.picScreen.Visible = false;
        }
    }
    private void SetApplicationIcon()
    {
        try
        {
            // Try to load icon from embedded resources first
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.Images.favicon.ico";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                    System.Diagnostics.Debug.WriteLine("Icon loaded from embedded resources");
                    return;
                }
            }

            // Fallback: try to load from Images folder
            string iconPath = Path.Combine(Application.StartupPath, "Images", "favicon.ico");
            if (File.Exists(iconPath))
            {
                this.Icon = new Icon(iconPath);
                System.Diagnostics.Debug.WriteLine("Icon loaded from Images folder");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Icon file not found");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading icon: {ex.Message}");
        }
    }

    #endregion
}