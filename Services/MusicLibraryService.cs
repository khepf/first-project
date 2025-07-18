using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMusicPlayer.Services
{
    public class MusicLibraryService
    {
        private string musicLibraryPath = "";

        // Events for UI updates
        public event EventHandler<LibraryChangedEventArgs>? LibraryChanged;

        // Properties
        public string LibraryPath 
        { 
            get => musicLibraryPath; 
            set 
            { 
                if (musicLibraryPath != value)
                {
                    musicLibraryPath = value;
                    LibraryChanged?.Invoke(this, new LibraryChangedEventArgs(value));
                }
            } 
        }
        public bool HasValidLibrary => !string.IsNullOrEmpty(musicLibraryPath) && Directory.Exists(musicLibraryPath);

        public MusicLibraryService()
        {
            // Initialize with default path
            musicLibraryPath = GetDefaultMusicLibraryPath();
        }

        public MusicLibraryService(string libraryPath)
        {
            musicLibraryPath = libraryPath;
        }

        /// <summary>
        /// Gets the default music library path by looking for a "Music" folder in the executable directory
        /// </summary>
        public static string GetDefaultMusicLibraryPath()
        {
            string executableDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) ?? "";
            string musicFolderPath = Path.Combine(executableDirectory, "Music");
            
            if (Directory.Exists(musicFolderPath))
            {
                System.Diagnostics.Debug.WriteLine($"Found Music folder at: {musicFolderPath}");
                return musicFolderPath;
            }
            
            System.Diagnostics.Debug.WriteLine($"Music folder not found at: {musicFolderPath}");
            return string.Empty;
        }

        /// <summary>
        /// Gets all main collection folders (bands/artists) in the music library
        /// </summary>
        public List<string> GetCollections()
        {
            var collections = new List<string>();
            
            if (!HasValidLibrary)
                return collections;

            try
            {
                var directories = Directory.EnumerateDirectories(musicLibraryPath);
                foreach (var folder in directories)
                {
                    string folderName = Path.GetFileName(folder);
                    collections.Add(folderName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading collections: {ex.Message}");
            }

            return collections.OrderBy(c => c).ToList();
        }

        /// <summary>
        /// Gets all year folders for a specific collection
        /// </summary>
        public List<string> GetYears(string collection)
        {
            var years = new List<string>();
            
            if (!HasValidLibrary || string.IsNullOrEmpty(collection))
                return years;

            string collectionPath = Path.Combine(musicLibraryPath, collection);
            if (!Directory.Exists(collectionPath))
                return years;

            try
            {
                var yearFolders = Directory.EnumerateDirectories(collectionPath)
                    .Select(dir => Path.GetFileName(dir))
                    .Where(name => int.TryParse(name, out _)) // Only include folders that are numeric (years)
                    .OrderBy(year => int.Parse(year))
                    .ToList();

                years.AddRange(yearFolders);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading years for {collection}: {ex.Message}");
            }

            return years;
        }

        /// <summary>
        /// Gets shows/albums for a specific collection and year
        /// Returns both direct audio files and show folders
        /// </summary>
        public LibraryContent GetShows(string collection, string year)
        {
            var content = new LibraryContent();
            
            if (!HasValidLibrary || string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(year))
                return content;

            string yearPath = Path.Combine(musicLibraryPath, collection, year);
            if (!Directory.Exists(yearPath))
                return content;

            try
            {
                // First, check for direct audio files (3-level structure)
                var directAudioFiles = Directory.GetFiles(yearPath, "*.mp3")
                    .Concat(Directory.GetFiles(yearPath, "*.flac"))
                    .OrderBy(f => Path.GetFileNameWithoutExtension(f))
                    .ToArray();

                if (directAudioFiles.Length > 0)
                {
                    // 3-level structure: Direct audio files found
                    foreach (var audioFile in directAudioFiles)
                    {
                        content.AudioFiles.Add(new AudioFileInfo
                        {
                            Name = Path.GetFileNameWithoutExtension(audioFile),
                            FullPath = audioFile,
                            Extension = Path.GetExtension(audioFile).ToUpperInvariant().TrimStart('.')
                        });
                    }
                }
                else
                {
                    // No direct audio files, check for show folders (4-level structure)
                    var showFolders = Directory.GetDirectories(yearPath)
                        .OrderBy(f => Path.GetFileName(f))
                        .ToArray();

                    foreach (var showFolder in showFolders)
                    {
                        // Check if this folder contains audio files
                        var folderAudioFiles = Directory.GetFiles(showFolder, "*.mp3")
                            .Concat(Directory.GetFiles(showFolder, "*.flac"))
                            .ToArray();

                        if (folderAudioFiles.Length > 0)
                        {
                            content.ShowFolders.Add(new ShowFolderInfo
                            {
                                Name = Path.GetFileName(showFolder),
                                FullPath = showFolder,
                                AudioFileCount = folderAudioFiles.Length
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading shows for {collection}/{year}: {ex.Message}");
            }

            return content;
        }

        /// <summary>
        /// Gets all audio files within a specific show folder
        /// </summary>
        public List<AudioFileInfo> GetSongsInShow(string showFolderPath)
        {
            var songs = new List<AudioFileInfo>();
            
            if (!Directory.Exists(showFolderPath))
                return songs;

            try
            {
                var audioFiles = Directory.GetFiles(showFolderPath, "*.mp3")
                    .Concat(Directory.GetFiles(showFolderPath, "*.flac"))
                    .OrderBy(f => Path.GetFileName(f))
                    .ToArray();

                foreach (var audioFile in audioFiles)
                {
                    songs.Add(new AudioFileInfo
                    {
                        Name = Path.GetFileNameWithoutExtension(audioFile),
                        FullPath = audioFile,
                        Extension = Path.GetExtension(audioFile).ToUpperInvariant().TrimStart('.')
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading songs from {showFolderPath}: {ex.Message}");
            }

            return songs;
        }

        /// <summary>
        /// Gets all audio files in the entire music library (for random selection)
        /// </summary>
        public List<string> GetAllAudioFiles()
        {
            var audioFiles = new List<string>();

            if (!HasValidLibrary)
                return audioFiles;

            try
            {
                // Search recursively for all audio files
                var extensions = new[] { "*.mp3", "*.flac" };
                foreach (var extension in extensions)
                {
                    audioFiles.AddRange(Directory.GetFiles(musicLibraryPath, extension, SearchOption.AllDirectories));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all audio files: {ex.Message}");
            }

            return audioFiles;
        }

        /// <summary>
        /// Navigates to a specific file and returns the navigation path
        /// </summary>
        public NavigationPath? NavigateToFile(string filePath)
        {
            if (!HasValidLibrary || string.IsNullOrEmpty(filePath))
                return null;

            try
            {
                // Get relative path from music library
                string relativePath = Path.GetRelativePath(musicLibraryPath, filePath);
                string[] pathParts = relativePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

                if (pathParts.Length >= 3)
                {
                    var navigation = new NavigationPath
                    {
                        Collection = pathParts[0],
                        Year = pathParts[1]
                    };

                    if (pathParts.Length == 3)
                    {
                        // 3-level structure: Band/Year/ShowFile.mp3
                        navigation.FileName = Path.GetFileNameWithoutExtension(pathParts[2]);
                        navigation.IsDirectFile = true;
                    }
                    else if (pathParts.Length >= 4)
                    {
                        // 4-level structure: Band/Year/Show/Song.mp3
                        navigation.ShowFolder = pathParts[2];
                        navigation.FileName = Path.GetFileNameWithoutExtension(pathParts[3]);
                        navigation.IsDirectFile = false;
                    }

                    return navigation;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to file {filePath}: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Validates if the provided path is a valid music library
        /// </summary>
        public static bool ValidateLibraryPath(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return false;

            // Check if it contains any subdirectories (collections)
            try
            {
                return Directory.GetDirectories(path).Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }

    // Data classes
    public class LibraryContent
    {
        public List<AudioFileInfo> AudioFiles { get; set; } = new List<AudioFileInfo>();
        public List<ShowFolderInfo> ShowFolders { get; set; } = new List<ShowFolderInfo>();
        
        public bool HasContent => AudioFiles.Count > 0 || ShowFolders.Count > 0;
        public bool IsThreeLevelStructure => AudioFiles.Count > 0 && ShowFolders.Count == 0;
        public bool IsFourLevelStructure => AudioFiles.Count == 0 && ShowFolders.Count > 0;
    }

    public class AudioFileInfo
    {
        public string Name { get; set; } = "";
        public string FullPath { get; set; } = "";
        public string Extension { get; set; } = "";
    }

    public class ShowFolderInfo
    {
        public string Name { get; set; } = "";
        public string FullPath { get; set; } = "";
        public int AudioFileCount { get; set; }
    }

    public class NavigationPath
    {
        public string Collection { get; set; } = "";
        public string Year { get; set; } = "";
        public string ShowFolder { get; set; } = "";
        public string FileName { get; set; } = "";
        public bool IsDirectFile { get; set; }
    }

    public class LibraryChangedEventArgs : EventArgs
    {
        public string NewPath { get; }

        public LibraryChangedEventArgs(string newPath)
        {
            NewPath = newPath;
        }
    }
}
