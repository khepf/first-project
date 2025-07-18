# Changelog

All notable changes to Jerry Player will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0]

### Added
- **Dual folder structure support**: Automatic detection and support for both 3-level and 4-level music organization
  - **3-level**: Music → Band → Year → audio_file.mp3/flac
  - **4-level**: Music → Band → Year → Show_Folder → song_files.mp3/flac
- **Drill-down navigation for show folders**: Users can now navigate into show folders to see and select individual songs
  - Double-click 📁 show folders to view individual tracks
  - ⬅️ "BACK TO SHOWS" navigation option when inside show folders
  - Visual distinction between folder view and song view
- **Enhanced random play navigation**: Random button now properly drills down to show specific songs in 4-level structures
- **Continuous playback**: Automatic advancement to the next song when current song ends
  - Seamless transitions with no gaps between songs
  - Smart navigation that skips non-playable items (back buttons, separators, info items)
  - Works in both 3-level and 4-level folder structures
- Visual distinction between single files (🎵) and show folders (📁) in the interface
- Enhanced navigation system that handles both folder structures seamlessly
- Smart file detection prioritizes direct audio files over show folders when both exist
- Improved user interface with special styling for navigation elements

## [1.0.2] - 2025-01-18
### Added
- Real-time waveform display visualization during playback

## [1.0.1] - 2025-01-15

### Added
- FLAC audio format support in addition to MP3
- Auto-detection for relative Music folder location
- Application launch now centers on main monitor
- High DPI support and scaling improvements

### Changed
- Optimized application startup time for better performance
- Updated error messages for better user experience
- Improved positioning and layout of UI components
- Enhanced volume control responsiveness

### Fixed
- Fixed DPI scaling issues on high-resolution displays
- Resolved Music folder detection in various directory structures

## [1.0.0] - 2024-12-20

### Added
- Complete retro-style cassette tape interface
- MP3 audio file playback with NAudio engine
- Dual circular dial navigation system (Band selection and Year browsing)
- Digital time display with current/total duration
- Spinning cassette animation during playback
- Volume control slider with mute/unmute functionality
- Progress bar with click-to-seek functionality
- Random show selection feature
- Music library folder selector (Star button)
- Help dialog with version information and support links
- Keyboard shortcuts (Spacebar: Play/Pause, Escape: Stop, F1: Help)
- Custom ActionButton, CircularDial, and DigitalTimeLabel components
- Embedded resource management for UI images
- Error handling for missing folders and unsupported file formats

### Technical
- Built on .NET 8.0 Windows Forms
- NAudio 2.2.1 integration for audio playback
- Custom double-buffered controls for smooth UI rendering
- Application icon and branding assets
- Self-contained executable with embedded resources

## [0.9.0] - 2024-12-01

### Added
- Initial project structure and basic Windows Forms application
- Core audio playback functionality using NAudio
- Basic file system navigation for music library
- Retro-themed UI foundation with custom styling
- Volume control implementation
- Basic play/pause/stop controls

### Technical
- Established .NET 8.0-windows target framework
- Integrated NAudio package for audio processing
- Created custom control base classes
- Implemented basic error handling and logging

