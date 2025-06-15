using NAudio.Wave;
using System.Drawing.Drawing2D;

namespace MyMusicPlayer
{
    public partial class Form1 : Form
    {
        private string musicLibraryPath = GetMusicLibraryPath();
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;
        private float currentVolume = 0.7f; // Default volume at 70%
        private bool isPaused = false;
        private System.Windows.Forms.Timer? progressTimer;
        private bool isUserDragging = false;
        private string currentShowPath = "";
        private string currentFolderPath = "";
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            SetupCustomListView();
            MaximizeBox = false;
            MinimizeBox = true;
            LoadImages();
            LoadMainFolders();
            UpdateButtonStates();

                    // Set initial volume
            if (trackVolume != null)
            {
                trackVolume.Value = (int)(currentVolume * 100);
            }
        }
        
        private Image? LoadEmbeddedImage(string imageName)
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                string resourceName = $"MyMusicPlayer.Images.{imageName}";
                
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading embedded image {imageName}: {ex.Message}");
            }
            
            return null;
        }
        private static string GetMusicLibraryPath()
        {
            return string.Empty;
        }

        private void TrackVolume_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is TrackBar volumeBar)
            {
                currentVolume = volumeBar.Value / 100.0f;
                
                // Update volume label
                if (lblVolume != null)
                {
                    lblVolume.Text = $"VOL: {volumeBar.Value}%";
                }
                
                // Apply volume to current audio output
                if (outputDevice != null)
                {
                    outputDevice.Volume = currentVolume;
                }
                
                System.Diagnostics.Debug.WriteLine($"Volume changed to: {volumeBar.Value}%");
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

        private void SetupCustomListView()
        {
            lstShows.OwnerDraw = true;
            lstShows.DrawItem += LstShows_DrawItem;
            lstShows.BorderStyle = BorderStyle.None;
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

                    Console.WriteLine($"Navigated to: {collection} -> {year} -> {fileName}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to file: {ex.Message}");
            }
        }

        private void LstShows_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            // Determine colors based on selection
            Color textColor = Color.White;
            
            // Create rounded rectangle path
            int cornerRadius = 8; // Adjust this value to make more or less rounded
            Rectangle itemRect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 1, e.Bounds.Width - 4, e.Bounds.Height - 2);
            
            using (GraphicsPath path = CreateRoundedRectangle(itemRect, cornerRadius))
            {
                // Enable anti-aliasing for smooth rounded edges
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                if (e.Item.Selected)
                {
                    // Selected item: Same bronze gradient as the buttons with rounded edges
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        itemRect,
                        ColorTranslator.FromHtml("#CD853F"), // Sandy brown (lighter)
                        ColorTranslator.FromHtml("#8B4513"), // Saddle brown (darker)
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    
                    // Optional: Add a subtle border to selected items
                    using (Pen borderPen = new Pen(ColorTranslator.FromHtml("#A0522D"), 1))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }
                }
                else
                {
                    // Non-selected item: Black background with rounded edges
                    using (SolidBrush backgroundBrush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.FillPath(backgroundBrush, path);
                    }
                }
                
                // Reset smoothing mode for text rendering
                e.Graphics.SmoothingMode = SmoothingMode.Default;
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

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = cornerRadius * 2;
            
            // Top-left corner
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            
            // Top-right corner
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            
            // Bottom-right corner
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            
            // Bottom-left corner
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            
            path.CloseFigure();
            return path;
        }

        private void LoadImages()
        {
            // Load dial image from embedded resources
            var dialImage = LoadEmbeddedImage("knob.png");
            if (dialImage != null)
            {
                // Create separate copies for each dial to avoid disposal issues
                dialCollection.DialBackgroundImage = new Bitmap(dialImage);
                dialYear.DialBackgroundImage = new Bitmap(dialImage);
                dialImage.Dispose();
            }

            // Load cassette image from embedded resources
            var cassetteImage = LoadEmbeddedImage("cassette.png");
            if (cassetteImage != null)
            {
                spinningCassette.CassetteImage = cassetteImage;
                System.Diagnostics.Debug.WriteLine("Cassette image loaded from embedded resources");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cassette image not found in embedded resources!");
                spinningCassette.Visible = false;
            }

            // Load bottom board image from embedded resources
            var bottomBoardImage = LoadEmbeddedImage("bottomboard.png");
            if (bottomBoardImage != null)
            {
                picBottomBoard.Image = bottomBoardImage;
            }
        }

        private void LoadMainFolders()
        {
            dialCollection.ClearItems();
            dialYear.ClearItems();
            lstShows.Items.Clear();

            Console.WriteLine($"Music library path: '{musicLibraryPath}'");
            Console.WriteLine($"Directory exists: {Directory.Exists(musicLibraryPath)}");

            // Check if music library path is set and exists
            if (string.IsNullOrEmpty(musicLibraryPath))
            {
                Console.WriteLine("No music library path set. Use Settings button to select a folder.");
                // Display greeting when no music library is set
                var greetingItem1 = new ListViewItem("   SHALL WE PLAY SOME MUSIC?");
                var greetingItem2 = new ListViewItem("1. CLICK THE STAR");
                var greetingItem3 = new ListViewItem("2. SELECT YOUR MUSIC LIBRARY");
                var greetingItem4 = new ListViewItem("3. SELECT A SHOW OR ROLL DICE");

                lstShows.Items.Add(greetingItem1);
                lstShows.Items.Add(greetingItem2);
                lstShows.Items.Add(greetingItem3);
                lstShows.Items.Add(greetingItem4);
                UpdateButtonStates();
                return;
            }

            if (Directory.Exists(musicLibraryPath))
            {
                var mf = Directory.GetDirectories(musicLibraryPath);
                Console.WriteLine($"Found {mf.Length} main folders");

                foreach (var folder in mf)
                {
                    string folderName = Path.GetFileName(folder);
                    Console.WriteLine($"Adding folder: {folderName}");
                    dialCollection.AddItem(folderName);
                }

                // Select the first folder if available
                if (dialCollection.Items.Count > 0)
                {
                    dialCollection.SelectedIndex = 0;
                }
                else
                {
                    // No folders found, display greeting
                    var greetingItem1 = new ListViewItem("I'M SORRY DAVE");
                    var greetingItem2 = new ListViewItem("FOLDER NOT FOUND");
                    lstShows.Items.Add(greetingItem1);
                    lstShows.Items.Add(greetingItem2);
                }
            }
            else
            {
                Console.WriteLine($"Music directory does not exist: {musicLibraryPath}");
                // Display greeting when music directory doesn't exist
                var greetingItem1 = new ListViewItem("I'M SORRY DAVE");
                var greetingItem2 = new ListViewItem("FOLDER NOT FOUND");
                lstShows.Items.Add(greetingItem1);
                lstShows.Items.Add(greetingItem2);
            }

            UpdateButtonStates();
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select your Music Library folder";
                folderDialog.SelectedPath = musicLibraryPath;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string newPath = folderDialog.SelectedPath;
                    
                    // Validate that the selected folder contains music folders
                    if (Directory.Exists(newPath))
                    {
                        var subFolders = Directory.GetDirectories(newPath);
                        
                        if (subFolders.Length == 0)
                        {
                            DialogResult result = MessageBox.Show(
                                $"The selected folder appears to be empty.\n\nDo you want to use it anyway?\n\nPath: {newPath}",
                                "Empty Folder", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question);
                                
                            if (result == DialogResult.No)
                                return;
                        }
                        
                        // Update the music library path
                        musicLibraryPath = newPath;
                        Console.WriteLine($"Music library path changed to: {musicLibraryPath}");
                        
                        // Reload the entire interface with the new path
                        LoadMainFolders();
                    }
                    else
                    {
                        MessageBox.Show("The selected folder does not exist!", 
                                    "Invalid Folder", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelectedItem = lstShows.SelectedItems.Count > 0;
            bool hasLoadedFile = !string.IsNullOrEmpty(currentShowPath) && File.Exists(currentShowPath);
            bool isPlaying = outputDevice?.PlaybackState == PlaybackState.Playing;
            bool isPausedState = outputDevice?.PlaybackState == PlaybackState.Paused;
            bool hasMusicLibrary = !string.IsNullOrEmpty(musicLibraryPath) &&
                                Directory.Exists(musicLibraryPath) &&
                                GetAllMp3Files(musicLibraryPath).Count > 0;

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
                    .OrderBy(year => int.Parse(year))
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

            if (dialCollection.SelectedItem == null || dialYear.SelectedItem == null) 
            {
                // Display greeting when no collection or year is selected
                var greetingItem1 = new ListViewItem("I'M SORRY DAVE,");
                var greetingItem2 = new ListViewItem("NO BAND OR YEAR SELECTED");
                lstShows.Items.Add(greetingItem1);
                lstShows.Items.Add(greetingItem2);
                UpdateButtonStates();
                return;
            }

            string selectedMainFolder = dialCollection.SelectedItem;
            string selectedYear = dialYear.SelectedItem;
            currentFolderPath = Path.Combine(musicLibraryPath, selectedMainFolder, selectedYear);

            if (Directory.Exists(currentFolderPath))
            {
                var mp3Files = Directory.GetFiles(currentFolderPath, "*.mp3");
                
                if (mp3Files.Length == 0)
                {
                    // Display greeting when folder exists but has no MP3 files
                    var greetingItem1 = new ListViewItem("I'M SORRY DAVE,");
                    var greetingItem2 = new ListViewItem("NO MP3 FILES FOUND");
                    lstShows.Items.Add(greetingItem1);
                    lstShows.Items.Add(greetingItem2);
                }
                else
                {
                    foreach (var mp3File in mp3Files)
                    {
                        string showName = Path.GetFileNameWithoutExtension(mp3File);
                        string showItem = "🎵 " + showName;

                        // For MaterialListView, we need to create ListViewItems
                        var listItem = new ListViewItem(showItem);
                        lstShows.Items.Add(listItem);
                    }
                }
            }
            else
            {
                // Display greeting when folder doesn't exist
                var greetingItem1 = new ListViewItem("I'M SORRY DAVE,");
                var greetingItem2 = new ListViewItem("FOLDER NOT FOUND");
                lstShows.Items.Add(greetingItem1);
                lstShows.Items.Add(greetingItem2);
            }
            
            UpdateButtonStates();
        }

        private void LstShows_DoubleClick(object? sender, EventArgs e)
        {
            if (lstShows.SelectedItems.Count > 0)
            {
                string selectedItem = lstShows.SelectedItems[0].Text ?? "";
                
                // Prevent double-click on greeting messages
                if (selectedItem == "   SHALL WE PLAY SOME MUSIC?" || 
                    selectedItem == "1. CLICK THE STAR," ||
                    selectedItem == "2. SELECT YOUR MUSIC LIBRARY," ||  
                    selectedItem == "3. SELECT A SHOW OR ROLL DICE," ||
                    selectedItem == "I'M SORRY DAVE," ||
                    selectedItem == "NO BAND OR YEAR SELECTED" ||
                    selectedItem == "NO MP3 FILES FOUND" ||
                    selectedItem == "FOLDER NOT FOUND") 
                {
                    return; // Do nothing for greeting messages
                }
                
                if (selectedItem.StartsWith("🎵 "))
                {
                    string showName = selectedItem.Substring(2).Trim(); // Remove the emoji and trim whitespace
                    string showPath = Path.Combine(currentFolderPath, showName + ".mp3"); // Construct the full file path

                    if (File.Exists(showPath))
                    {
                        PlayShow(showPath); // Play the selected show
                    }
                    else
                    {
                        MessageBox.Show($"The file '{showName}.mp3' could not be found in the folder '{currentFolderPath}'.",
                                        "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void LstShows_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Prevent selection of greeting messages
            if (lstShows.SelectedItems.Count > 0)
            {
                string selectedText = lstShows.SelectedItems[0].Text;
                if (selectedText == "GREETINGS PROFESSOR FALKEN," || 
                    selectedText == "SHALL WE PLAY SOME MUSIC?")
                {
                    // Clear the selection for greeting messages
                    lstShows.SelectedItems[0].Selected = false;
                    return;
                }
            }
            
            UpdateButtonStates(); // Update button states when selection changes
        }

        private void PlayShow(string filePath, TimeSpan? startPosition = null)
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

            // Set the starting position if provided
            if (startPosition.HasValue && startPosition.Value < audioFile.TotalTime)
            {
                audioFile.CurrentTime = startPosition.Value;
            }

            outputDevice.Init(audioFile);
            outputDevice.Volume = currentVolume; // Set the volume
            outputDevice.Play();
            isPaused = false;
            btnPlay.Text = "⏸";
            spinningCassette.IsSpinning = true;

            // UPDATE THE CURRENTLY PLAYING LABEL
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            lblCurrentlyPlaying.Text = $"♪ Now Playing: {fileName}";

            // UPDATE THE PATH LABEL TO SHOW CURRENT SELECTION
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
                    btnPlay.Text = "▶";
                    spinningCassette.IsSpinning = false; 
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
                spinningCassette.IsSpinning = false;
            }
            // If currently paused, resume it
            else if (outputDevice?.PlaybackState == PlaybackState.Paused)
            {
                outputDevice.Play();
                isPaused = false;
                btnPlay.Text = "⏸";  // Change to pause icon
                if (!isUserDragging)
                    progressTimer?.Start();
                spinningCassette.IsSpinning = true;
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
            lblCurrentPath.Text = "";
            spinningCassette.IsSpinning = false;

            isUserDragging = false;
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

                // Pick a random starting position within the file's duration
                TimeSpan randomStartPosition = TimeSpan.FromSeconds(random.Next(0, (int)new AudioFileReader(randomFile).TotalTime.TotalSeconds));

                // Play the random file starting at the random position
                PlayShow(randomFile, randomStartPosition);

                System.Diagnostics.Debug.WriteLine($"Random play: {randomFile} starting at {randomStartPosition}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing random show: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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