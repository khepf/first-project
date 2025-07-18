# Jerry Player - User Guide

## Welcome to Jerry Player!

Jerry Player is a retro-style music player with a unique cassette tape interface designed for playing your MP3 music collection. This guide will help you get started and make the most of your music listening experience.

## Getting Started

### First Time Setup

1. **Download and Install**
   - Download the `JerryPlayer.exe` file to your computer
   - You can run it directly - no installation required!

2. **Prepare Your Music Library**
   - Create a folder called `Music` in the same location as the `MyMusicPlayer.exe` file
   - Inside the `Music` folder, organize your MP3/FLAC files into subfolders
   - **Jerry Player supports two folder structures:**
   
   **Option A - Direct Files (3-level):**
   ```
   Music/
   ├── Example Band 1
   │   ├── 1975
   │   │   └── Red Rocks, Colorado.mp3
   │   └── 1976
   │       └── The Slippery Biscuit, Dayton Ohio.flac
   └── Example Band 2
       ├── 1980
       │   └── Jackson Stadium, Chicago Illinois.mp3
       └── 1985
           └── Ed's Coffee Emporium, Toronto Canada.mp3
   ```
   
   **Option B - Show Folders (4-level):**
   ```
   Music/
   ├── Example Band 1
   │   ├── 1975
   │   │   └── Red Rocks, Colorado
   │   │       ├── 01 - Opening Song.mp3
   │   │       ├── 02 - Second Song.mp3
   │   │       └── 03 - Encore.mp3
   │   └── 1976
   │       └── The Slippery Biscuit, Dayton Ohio
   │           ├── Track 1.flac
   │           └── Track 2.flac
   └── Example Band 2
       ├── 1980
       │   └── Jackson Stadium, Chicago Illinois
       │       ├── Song A.mp3
       │       └── Song B.mp3
       └── 1985
           └── Ed's Coffee Emporium, Toronto Canada
               └── Full Show.mp3
   ```

3. **Launch Jerry Player**
   - Double-click `JerryPlayer.exe` to start the application
   - The retro-style interface will appear with a cassette tape design

## Understanding the Interface

### Main Controls

- **⭐ Star Button** (Top Right): Opens the music library folder selector
- **❓ Help Button**: Shows version information and support details
- **▶ Play/Pause Button**: Starts or pauses music playback
- **⏹ Stop Button**: Stops music playback completely
- **🎲 Random Button**: Plays a random show from your entire music library

### The Screen Display

The main screen shows:
- **Music folders** from your library (when no folder is selected)
- **Show list** from the selected folder
  - **🎵 Single Files**: Individual audio files (3-level structure)
  - **📁 Show Folders**: Folders containing multiple songs (4-level structure)
- **Current playing information** during playback

### The Dials

Two circular dials on the interface help you navigate:
- **Left Dial**: Select a Band
- **Right Dial**: Browse through shows by year from selected band

### Progress and Time Display

- **Progress Bar**: Shows current playback position (you can click to jump to different parts)
- **Digital Time Display**: Shows current time and total show duration
- **Spinning Cassette**: Visual indicator that shows when music is playing

### Volume Control

- **Volume Slider**: Adjust playback volume (0-100%)
- **🔊 Speaker Icon**: Click to mute/unmute audio instantly

## How to Use Jerry Player

### Playing Music

1. **Use the Left Dial to Select a Band**

2. **Use the Right Dial to Choose a Year From the Currently Selected Band**

3. **For different folder structures:**
   - **🎵 Single Files**: Double-click to play the audio file directly
   - **📁 Show Folders**: Double-click to navigate into the folder and see individual songs
     - Once inside a show folder, you'll see individual songs with a "⬅️ BACK TO SHOWS" option
     - Double-click any song to play it
   - OR select any item and click the ▶ Play button

4. **Control Playback**
   - **Play/Pause**: Click ▶ to play or ⏸ to pause
   - **Stop**: Click ⏹ to stop completely
   - **Volume**: Use the volume slider or click the speaker icon to mute
   - **Seek**: Click anywhere on the progress bar to jump to that position

### Quick Random Play

- Click the **🎲 Random Button** to instantly play a random show from your entire music library
- The player will automatically navigate to the show's location and start playing from a random position
- **For 4-level structures**: The interface will drill down to show the specific song playing within the show folder

### Continuous Playback

- **Automatic Advancement**: When a song finishes playing, the next song in the current list will automatically start
- **Seamless Experience**: No gaps between songs for uninterrupted listening
- **Smart Navigation**: Automatically skips non-playable items (back buttons, separators, info items)
- **Works Everywhere**: Functions in both 3-level structure (year view) and 4-level structure (within show folders)

### Changing Your Music Library

1. Click the **⭐ Star Button** in the top-right corner
2. Browse and select a different folder containing your music
3. The player will refresh and show your new music collection

## Tips for Best Experience

### Organizing Your Music

- **Use band names for folder names**: "Led Balloon", "The What", "Happily Committed", etc.
- **Use the year the show took place as the subfolder name**
- **Choose your preferred structure**:
  - **3-level**: For single-file shows (concerts recorded as one file)
  - **4-level**: For multi-track shows (individual songs in show folders)
- **File naming**: Use consistent naming for easy browsing
- **Avoid special characters** in folder and file names

### Playback Tips

- **Drag the progress bar** to jump to different parts of a show
- **Use the volume control** instead of system volume for better control
- **The cassette spins** when music is playing - just like a real tape player!

### Navigation Tips

- **Double-click shows** for quick playback (3-level) or navigation (4-level)
- **Use the dials** for smooth navigation through your library
- **Navigate show folders**: Double-click 📁 folders to see individual songs, use ⬅️ BACK to return
- **Random play** is great for discovering forgotten shows in your collection

## Troubleshooting

### "I'M SORRY DAVE, FOLDER NOT FOUND"

This message appears when:
- No `Music` folder exists next to the application
- The selected music folder is empty
- **Solution**: Create a `Music` folder and add MP3 files, or use the ⭐ button to select a different folder

### "NO MP3 FILES FOUND"

This appears when:
- The selected folder contains no MP3 files
- **Solution**: Add MP3 files to your music folders or select a different folder

### Music Won't Play

- **Check file format**: Only MP3 files are supported
- **Check file location**: Make sure the MP3 files are in the correct folder
- **Try different files**: Test with different MP3 files to isolate the issue

### Application Won't Start

- **Check Windows compatibility**: Requires Windows with .NET 8 support
- **Run as administrator**: Right-click the .exe and select "Run as administrator"
- **Check antivirus**: Some antivirus software may block the application

## Technical Requirements

- **Operating System**: Windows 10 or later
- **Audio**: Sound card or audio output device
- **File Format**: MP3 and FLAC audio files
- **Folder Structure**: Supports both 3-level and 4-level organization
- **Storage**: Minimal space required (application is self-contained)

## Keyboard Shortcuts

- **Spacebar**: Play/Pause toggle
- **Escape**: Stop playback
- **F1**: Open help dialog

## Support

If you encounter issues or want to support the development:
- **Help Button**: Click ❓ in the application for support information
- **Development Support**: Visit the donation link shown in the help dialog

## Enjoy Your Music!

Jerry Player is designed to give you a nostalgic, retro music listening experience. Take your time exploring your music collection with the unique cassette tape interface, and enjoy rediscovering your favorite songs!

---

*Jerry Player Version 1.0 - A retro music experience for the modern age*
