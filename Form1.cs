using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MyMusicPlayer
{
    public partial class Form1 : Form  // Keep as Form
    {
        private readonly string musicLibraryPath = @"C:\CODE\garcia\Music";
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;
        private bool isPaused = false;
        private System.Windows.Forms.Timer? progressTimer;
        private bool isUserDragging = false;
        private string currentShowPath = "";
        private string currentFolderPath = "";
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            // Set up custom list view drawing for bronze selection
            SetupCustomListView();

            // Force the form properties after MaterialSkin (more aggressive)
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            // Additional override to ensure it sticks
            this.MinimumSize = new Size(800, 650);
            this.MaximumSize = new Size(800, 650);

            LoadMainFolders();
            UpdateButtonStates();
        }

        private void BtnRandom_Click(object? sender, EventArgs e)
        {
            try
            {
                // Get all MP3 files from the entire music library
                List<string> allMp3Files = GetAllMp3Files(musicLibraryPath);

                if (allMp3Files.Count == 0)
                {
                    MessageBox.Show("No MP3 files found in the music library!",
                                "No Music Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Pick a random file
                string randomFile = allMp3Files[random.Next(allMp3Files.Count)];

                // Update the dials and list to show the selected file's location
                NavigateToFile(randomFile);

                // Play the random file
                PlayShow(randomFile);

                System.Diagnostics.Debug.WriteLine($"Random play: {randomFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing random show: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> GetAllMp3Files(string rootPath)
        {
            List<string> mp3Files = new List<string>();

            try
            {
                if (!Directory.Exists(rootPath))
                    return mp3Files;

                // Recursively search all subdirectories for MP3 files
                mp3Files.AddRange(Directory.GetFiles(rootPath, "*.mp3", SearchOption.AllDirectories));

                System.Diagnostics.Debug.WriteLine($"Found {mp3Files.Count} MP3 files total");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning for MP3 files: {ex.Message}");
            }

            return mp3Files;
        }

        private void NavigateToFile(string filePath)
        {
            try
            {
                // Extract the path components
                string relativePath = Path.GetRelativePath(musicLibraryPath, filePath);
                string[] pathParts = relativePath.Split(Path.DirectorySeparatorChar);

                if (pathParts.Length >= 3) // Should be: Collection/Year/filename.mp3
                {
                    string collection = pathParts[0];
                    string year = pathParts[1];
                    string fileName = Path.GetFileNameWithoutExtension(pathParts[2]);

                    // Update collection dial
                    for (int i = 0; i < dialCollection.Items.Count; i++)
                    {
                        if (dialCollection.Items[i] == collection)
                        {
                            dialCollection.SelectedIndex = i;
                            break;
                        }
                    }

                    // Load years for the selected collection
                    LoadYears();

                    // Update year dial
                    for (int i = 0; i < dialYear.Items.Count; i++)
                    {
                        if (dialYear.Items[i] == year)
                        {
                            dialYear.SelectedIndex = i;
                            break;
                        }
                    }

                    // Load shows for the selected year
                    LoadShows();

                    // Select the file in the list
                    string showItem = "🎵 " + fileName;
                    for (int i = 0; i < lstShows.Items.Count; i++)
                    {
                        if (lstShows.Items[i].Text == showItem)
                        {
                            lstShows.Items[i].Selected = true;
                            lstShows.Items[i].Focused = true;
                            lstShows.EnsureVisible(i); // Scroll to make it visible
                            break;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Navigated to: {collection} -> {year} -> {fileName}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to file: {ex.Message}");
            }
        }

        private void SetupCustomListView()
        {
            // Enable custom drawing
            lstShows.OwnerDraw = true;
            lstShows.DrawItem += LstShows_DrawItem;
            lstShows.BorderStyle = BorderStyle.None;
        }

        private void LstShows_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            // Determine colors based on selection
            Color textColor = Color.White;

            if (e.Item.Selected)
            {
                // Selected item: Same bronze gradient as the buttons
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    e.Bounds,
                    ColorTranslator.FromHtml("#CD853F"), // Sandy brown (lighter)
                    ColorTranslator.FromHtml("#8B4513"), // Saddle brown (darker)
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            else
            {
                // Non-selected item: Black background
                using (SolidBrush backgroundBrush = new SolidBrush(Color.Black))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }
            }

            // Draw the music note icon - CENTERED VERTICALLY
            string musicIcon = "🎵";
            Font iconFont = new Font("Segoe UI Emoji", 14, FontStyle.Regular);
            Rectangle iconBounds = new Rectangle(
                e.Bounds.X + 10, // Left padding
                e.Bounds.Y, // Full height for vertical centering
                25, // Icon width
                e.Bounds.Height // Full item height
            );

            using (SolidBrush iconBrush = new SolidBrush(textColor))
            {
                StringFormat iconFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center // CENTER the icon vertically
                };

                e.Graphics.DrawString(musicIcon, iconFont, iconBrush, iconBounds, iconFormat);
            }

            // Draw the text - LEFT ALIGNED AND VERTICALLY CENTERED
            string displayText = e.Item.Text;
            if (displayText.StartsWith("🎵 "))
            {
                displayText = displayText.Substring(2).Trim(); // Remove the emoji from text
            }

            Rectangle textBounds = new Rectangle(
                e.Bounds.X + 40, // Left padding after icon
                e.Bounds.Y, // Start from top
                e.Bounds.Width - 50, // Account for icon space and right padding
                e.Bounds.Height  // Full item height for vertical centering
            );

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Near, // LEFT ALIGN TEXT
                    LineAlignment = StringAlignment.Center, // CENTER VERTICALLY
                    Trimming = StringTrimming.EllipsisCharacter
                };

                e.Graphics.DrawString(displayText, lstShows.Font, textBrush, textBounds, format);
            }

            // Dispose the icon font
            iconFont.Dispose();
        }

        private void UpdateButtonStates()
        {
            bool hasSelectedItem = lstShows.SelectedItems.Count > 0;
            bool hasLoadedFile = !string.IsNullOrEmpty(currentShowPath) && File.Exists(currentShowPath);
            bool isPlaying = outputDevice?.PlaybackState == PlaybackState.Playing;
            bool isPausedState = outputDevice?.PlaybackState == PlaybackState.Paused;
            bool hasMusicLibrary = Directory.Exists(musicLibraryPath) && GetAllMp3Files(musicLibraryPath).Count > 0;

            // Enable Play/Pause button if there's a selected item, loaded file, or something playing/paused
            btnPlay.Enabled = hasSelectedItem || hasLoadedFile || isPlaying || isPausedState;

            // Enable Stop button only if something is playing or paused
            btnStop.Enabled = isPlaying || isPausedState;

            // Enable Random button if there are any MP3 files in the library
            btnRandom.Enabled = hasMusicLibrary;

            // Update the icon based on current state
            if (isPlaying)
            {
                btnPlay.Text = "⏸";  // Show pause icon when playing
            }
            else
            {
                btnPlay.Text = "▶";  // Show play icon when stopped or paused
            }
        }

        private void LoadMainFolders()
        {
            dialCollection.ClearItems();
            dialYear.ClearItems();
            lstShows.Items.Clear();


            // Try multiple possible paths
            string[] possiblePaths = {
                Path.Combine(Application.StartupPath, "Images", "knob3.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "Images", "knob3.png"),
                Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "knob3.png")
            };

            bool imageLoaded = false;
            Image? dialImage = null;

            foreach (var path in possiblePaths)
            {
                System.Diagnostics.Debug.WriteLine($"Trying path: {path}");
                System.Diagnostics.Debug.WriteLine($"File exists: {File.Exists(path)}");

                if (File.Exists(path))
                {
                    try
                    {
                        dialImage = Image.FromFile(path);
                        System.Diagnostics.Debug.WriteLine($"SUCCESS! Image loaded from: {path}");
                        imageLoaded = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading image from {path}: {ex.Message}");
                    }
                }
            }

            // If no image loaded, create a test pattern
            if (!imageLoaded)
            {
                System.Diagnostics.Debug.WriteLine("Creating test pattern image...");
                try
                {
                    dialImage = new Bitmap(120, 120);
                    using (Graphics g = Graphics.FromImage(dialImage))
                    {
                        // Create a nice gradient test pattern
                        using (var brush = new LinearGradientBrush(
                            new Rectangle(0, 0, 120, 120),
                            ColorTranslator.FromHtml("#0C2999"), // Your blue
                            ColorTranslator.FromHtml("#6B85FF"), // Your light blue
                            45f))
                        {
                            g.FillEllipse(brush, 0, 0, 120, 120);
                        }

                        // Add border
                        using (var pen = new Pen(Color.White, 3))
                        {
                            g.DrawEllipse(pen, 3, 3, 114, 114);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Test pattern created!");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error creating test pattern: {ex.Message}");
                }
            }

            // Apply the image to BOTH dials
            if (dialImage != null)
            {
                // Create separate copies for each dial to avoid disposal issues
                dialCollection.DialBackgroundImage = new Bitmap(dialImage);
                dialYear.DialBackgroundImage = new Bitmap(dialImage);

                // Dispose the original to prevent memory leaks
                dialImage.Dispose();
            }

            // Load cassette image
            string[] cassettePaths = {
                Path.Combine(Application.StartupPath, "Images", "cassette.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "Images", "cassette.png"),
                Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "cassette.png")
            };

            bool cassetteLoaded = false;
            foreach (var path in cassettePaths)
            {
                System.Diagnostics.Debug.WriteLine($"Trying cassette path: {path}");

                if (File.Exists(path))
                {
                    try
                    {
                        picCassette.Image = Image.FromFile(path);
                        System.Diagnostics.Debug.WriteLine($"SUCCESS! Cassette image loaded from: {path}");
                        cassetteLoaded = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading cassette image from {path}: {ex.Message}");
                    }
                }
            }

            if (!cassetteLoaded)
            {
                System.Diagnostics.Debug.WriteLine("Cassette image not found!");
                // Optionally hide the PictureBox if image not found
                picCassette.Visible = false;
            }

            string[] bottomBoardPaths = {
                Path.Combine(Application.StartupPath, "Images", "bottomboard.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "Images", "bottomboard.png"),
                Path.Combine(Directory.GetParent(Application.StartupPath)?.Parent?.Parent?.FullName ?? Application.StartupPath, "Images", "bottomboard.png")
            };

            foreach (var path in bottomBoardPaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        this.picBottomBoard.Image = Image.FromFile(path);
                        break;
                    }
                    catch
                    {
                        // Handle image loading failure
                    }
                }
            }


            if (!Directory.Exists(musicLibraryPath))
            {
                Directory.CreateDirectory(musicLibraryPath);
                MessageBox.Show($"Created music library at: {musicLibraryPath}\nAdd your two main collection folders here.",
                               "Music Library Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateButtonStates(); // Update after clearing
                return;
            }

            // Load main folders
            var mainFolders = Directory.GetDirectories(musicLibraryPath);
            foreach (var folder in mainFolders)
            {
                string folderName = Path.GetFileName(folder);
                dialCollection.AddItem(folderName);
            }

            // Select the first folder if available
            if (dialCollection.Items.Count > 0)
            {
                dialCollection.SelectedIndex = 0;
            }

            UpdateButtonStates(); // Update after loading
        }

        private void DialCollection_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (dialCollection.SelectedItem != null)
            {
                LoadYears();
            }
        }

        private void LoadYears()
        {
            dialYear.ClearItems();
            lstShows.Items.Clear();

            if (dialCollection.SelectedItem == null) return;

            string selectedMainFolder = dialCollection.SelectedItem;
            string mainFolderPath = Path.Combine(musicLibraryPath, selectedMainFolder);

            if (Directory.Exists(mainFolderPath))
            {
                // Load year subfolders and sort them
                var yearFolders = Directory.GetDirectories(mainFolderPath)
                    .Select(dir => Path.GetFileName(dir))
                    .Where(name => int.TryParse(name, out _)) // Only include folders that are numeric (years)
                    .OrderByDescending(year => int.Parse(year)) // Sort years in descending order (newest first)
                    .ToList();

                foreach (string year in yearFolders)
                {
                    dialYear.AddItem(year);
                }

                // Select the first year if available
                if (dialYear.Items.Count > 0)
                {
                    dialYear.SelectedIndex = 0;
                }
            }
            UpdateButtonStates(); // Update after loading years
        }

        private void DialYear_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (dialYear.SelectedItem != null && dialCollection.SelectedItem != null)
            {
                LoadShows();
            }
        }

        private void LoadShows()
        {
            lstShows.Items.Clear();

            if (dialCollection.SelectedItem == null || dialYear.SelectedItem == null) return;

            string selectedMainFolder = dialCollection.SelectedItem;
            string selectedYear = dialYear.SelectedItem;
            currentFolderPath = Path.Combine(musicLibraryPath, selectedMainFolder, selectedYear);

            if (Directory.Exists(currentFolderPath))
            {
                var mp3Files = Directory.GetFiles(currentFolderPath, "*.mp3");
                foreach (var mp3File in mp3Files)
                {
                    string showName = Path.GetFileNameWithoutExtension(mp3File); // Remove .mp3 extension
                    string showItem = "🎵 " + showName;

                    // For MaterialListView, we need to create ListViewItems
                    var listItem = new ListViewItem(showItem);
                    lstShows.Items.Add(listItem);
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {mp3Files.Length} shows from {currentFolderPath}");
            }
            UpdateButtonStates(); // Update after loading shows
        }

        private void LstShows_DoubleClick(object? sender, EventArgs e)
        {
            if (lstShows.SelectedItems.Count > 0)
            {
                string selectedItem = lstShows.SelectedItems[0].Text ?? "";
                if (selectedItem.StartsWith("🎵 "))
                {
                    string showName = selectedItem.Substring(2); // Remove music icon
                    string showPath = Path.Combine(currentFolderPath, showName + ".mp3"); // Add .mp3 back

                    if (File.Exists(showPath))
                    {
                        PlayShow(showPath);
                    }
                }
            }
        }

        private void LstShows_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateButtonStates(); // Update button states when selection changes
        }

        private void PlayShow(string filePath)
        {
            // Stop any current playback and timer
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            progressTimer?.Stop();
            progressTimer?.Dispose();

            currentShowPath = filePath;
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);
            outputDevice.Init(audioFile);
            outputDevice.Play();
            isPaused = false;
            btnPlay.Text = "⏸";  // Set to pause icon when playing

            // UPDATE THE CURRENTLY PLAYING LABEL
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            lblCurrentlyPlaying.Text = $"♪ Now Playing: {fileName}";

            // UPDATE THE PATH LABEL TO SHOW CURRENT SELECTION - ADD THIS
            if (dialCollection.SelectedItem != null && dialYear.SelectedItem != null)
            {
                lblCurrentPath.Text = $"{dialCollection.SelectedItem} ➤ {dialYear.SelectedItem}";
            }

            // Set up progress tracking
            trackProgress.Maximum = (int)audioFile.TotalTime.TotalSeconds;
            lblTotalTime.Text = FormatTime(audioFile.TotalTime);

            // Start progress timer
            progressTimer = new System.Windows.Forms.Timer { Interval = 100 };
            progressTimer.Tick += ProgressTimer_Tick;
            progressTimer.Start();

            // Highlight the currently playing show in the list
            HighlightCurrentShow(filePath);
            UpdateButtonStates();
        }

        private void HighlightCurrentShow(string showPath)
        {
            string showName = Path.GetFileName(showPath);
            string showItem = "🎵 " + showName;

            for (int i = 0; i < lstShows.Items.Count; i++)
            {
                if (lstShows.Items[i].Text == showItem)
                {
                    lstShows.Items[i].Selected = true;
                    lstShows.Items[i].Focused = true;
                    break;
                }
            }
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null && !isUserDragging)
            {
                int currentSeconds = (int)audioFile.CurrentTime.TotalSeconds;
                trackProgress.Value = Math.Min(currentSeconds, trackProgress.Maximum);
                lblCurrentTime.Text = FormatTime(audioFile.CurrentTime);

                // Check if show has ended
                if (outputDevice.PlaybackState == PlaybackState.Stopped && trackProgress.Value > 0)
                {
                    btnPlay.Text = "▶";  // Reset to play icon when show ends
                    BtnStop_Click(sender, e);
                }
            }
        }

        private void TrackProgress_MouseDown(object? sender, MouseEventArgs e)
        {
            isUserDragging = true;
            progressTimer?.Stop();
        }

        private void TrackProgress_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isUserDragging && audioFile != null)
            {
                // Set the audio position to the trackbar value
                audioFile.CurrentTime = TimeSpan.FromSeconds(trackProgress.Value);
                lblCurrentTime.Text = FormatTime(audioFile.CurrentTime);

                isUserDragging = false;

                // Restart timer if playing
                if (!isPaused && outputDevice?.PlaybackState == PlaybackState.Playing)
                {
                    progressTimer?.Start();
                }
            }
        }

        private void TrackProgress_ValueChanged(object? sender, EventArgs e)
        {
            if (audioFile != null && isUserDragging)
            {
                // Update time display while dragging
                lblCurrentTime.Text = FormatTime(TimeSpan.FromSeconds(trackProgress.Value));
            }
        }

        private void BtnPlayPause_Click(object? sender, EventArgs e)
        {
            // If nothing is loaded and a show is selected, load and play it
            if (lstShows.SelectedItems.Count > 0 && (outputDevice == null || outputDevice.PlaybackState == PlaybackState.Stopped))
            {
                string selectedItem = lstShows.SelectedItems[0].Text ?? "";
                if (selectedItem.StartsWith("🎵 "))
                {
                    string showName = selectedItem.Substring(2).Trim();
                    string showPath = Path.Combine(currentFolderPath, showName + ".mp3"); // Add .mp3 back

                    if (File.Exists(showPath))
                    {
                        PlayShow(showPath);
                        btnPlay.Text = "⏸";  // Change to pause icon
                        return;
                    }
                }
            }

            // If currently playing, pause it
            if (outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
                isPaused = true;
                btnPlay.Text = "▶";  // Change to play icon
                progressTimer?.Stop();
            }
            // If currently paused, resume it
            else if (outputDevice?.PlaybackState == PlaybackState.Paused)
            {
                outputDevice.Play();
                isPaused = false;
                btnPlay.Text = "⏸";  // Change to pause icon
                if (!isUserDragging)
                    progressTimer?.Start();
            }
            // If stopped but we have a loaded show, restart from beginning
            else if (!string.IsNullOrEmpty(currentShowPath) && File.Exists(currentShowPath))
            {
                PlayShow(currentShowPath);
                btnPlay.Text = "⏸";  // Change to pause icon
            }

            UpdateButtonStates();
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            progressTimer?.Stop();
            progressTimer?.Dispose();
            outputDevice = null;
            audioFile = null;
            progressTimer = null;
            isPaused = false;
            btnPlay.Text = "▶";  // Reset to play icon when stopped
            trackProgress.Value = 0;
            lblCurrentTime.Text = "00:00";
            lblTotalTime.Text = "00:00";

            // CLEAR BOTH LABELS WHEN STOPPED
            lblCurrentlyPlaying.Text = "";
            lblCurrentPath.Text = ""; // ADD THIS LINE

            isUserDragging = false;
            UpdateButtonStates();
        }

        private string FormatTime(TimeSpan time)
        {
            // Handle times over 99 minutes properly
            if (time.TotalMinutes >= 100)
            {
                return $"{(int)time.TotalMinutes:D3}:{time.Seconds:D2}";
            }
            else
            {
                return $"{(int)time.TotalMinutes:D2}:{time.Seconds:D2}";
            }
        }
         
    }
}