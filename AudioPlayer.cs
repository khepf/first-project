using NAudio.Wave;

namespace MyMusicPlayer
{
    public class AudioPlayer : IDisposable
    {
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;
        private bool isPaused = false;
        private float currentVolume = 0.7f;
        private float volumeBeforeMute = 0.7f;
        private bool isMuted = false;
        private string currentFilePath = "";
        private WaveformSampleProvider? waveformSampleProvider;

        // Events for UI updates
        public event EventHandler<PlaybackStateEventArgs>? PlaybackStateChanged;
        public event EventHandler<ProgressEventArgs>? ProgressChanged;
        public event EventHandler<VolumeEventArgs>? VolumeChanged;
        public event EventHandler? PlaybackStopped;

        // Properties
        public bool IsPaused => isPaused;
        public bool IsPlaying => outputDevice?.PlaybackState == PlaybackState.Playing;
        public bool IsStopped => outputDevice?.PlaybackState == PlaybackState.Stopped || outputDevice == null;
        public PlaybackState PlaybackState => outputDevice?.PlaybackState ?? PlaybackState.Stopped;
        public float Volume 
        { 
            get => currentVolume; 
            set 
            { 
                currentVolume = Math.Clamp(value, 0.0f, 1.0f);
                ApplyVolume();
                VolumeChanged?.Invoke(this, new VolumeEventArgs(currentVolume, isMuted));
            } 
        }
        public bool IsMuted 
        { 
            get => isMuted; 
            set 
            { 
                if (value != isMuted)
                {
                    if (value)
                    {
                        volumeBeforeMute = currentVolume;
                        isMuted = true;
                    }
                    else
                    {
                        isMuted = false;
                        currentVolume = volumeBeforeMute;
                    }
                    ApplyVolume();
                    VolumeChanged?.Invoke(this, new VolumeEventArgs(currentVolume, isMuted));
                }
            } 
        }
        public string CurrentFilePath => currentFilePath;
        public TimeSpan CurrentTime => audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => audioFile?.TotalTime ?? TimeSpan.Zero;

        public void Play(string filePath, TimeSpan? startPosition = null, WaveformDisplay? waveformDisplay = null)
        {
            Stop(); // Stop any current playback
            
            currentFilePath = filePath;
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            // Set the starting position if provided
            if (startPosition.HasValue && startPosition.Value < audioFile.TotalTime)
            {
                audioFile.CurrentTime = startPosition.Value;
            }

            // Create waveform sample provider if waveform display is provided
            if (waveformDisplay != null)
            {
                waveformSampleProvider = new WaveformSampleProvider(audioFile, waveformDisplay);
                outputDevice.Init(waveformSampleProvider);
            }
            else
            {
                outputDevice.Init(audioFile);
            }

            ApplyVolume();
            outputDevice.Play();
            isPaused = false;

            PlaybackStateChanged?.Invoke(this, new PlaybackStateEventArgs(PlaybackState.Playing, filePath));
        }

        public void Pause()
        {
            if (outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
                isPaused = true;
                waveformSampleProvider?.ClearWaveform();
                PlaybackStateChanged?.Invoke(this, new PlaybackStateEventArgs(PlaybackState.Paused, currentFilePath));
            }
        }

        public void Resume()
        {
            if (isPaused && outputDevice?.PlaybackState == PlaybackState.Paused)
            {
                outputDevice.Play();
                isPaused = false;
                PlaybackStateChanged?.Invoke(this, new PlaybackStateEventArgs(PlaybackState.Playing, currentFilePath));
            }
        }

        public void Stop()
        {
            var wasPlaying = IsPlaying || IsPaused;
            
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            waveformSampleProvider?.ClearWaveform();
            
            outputDevice = null;
            audioFile = null;
            waveformSampleProvider = null;
            isPaused = false;
            currentFilePath = "";

            if (wasPlaying)
            {
                PlaybackStopped?.Invoke(this, EventArgs.Empty);
                PlaybackStateChanged?.Invoke(this, new PlaybackStateEventArgs(PlaybackState.Stopped, ""));
            }
        }

        public void SetPosition(TimeSpan position)
        {
            if (audioFile != null && position <= audioFile.TotalTime)
            {
                audioFile.CurrentTime = position;
            }
        }

        public void ToggleMute()
        {
            IsMuted = !IsMuted;
        }

        private void ApplyVolume()
        {
            if (outputDevice != null)
            {
                outputDevice.Volume = isMuted ? 0.0f : currentVolume;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    // Event argument classes
    public class PlaybackStateEventArgs : EventArgs
    {
        public PlaybackState State { get; }
        public string FilePath { get; }

        public PlaybackStateEventArgs(PlaybackState state, string filePath)
        {
            State = state;
            FilePath = filePath;
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public TimeSpan CurrentTime { get; }
        public TimeSpan TotalTime { get; }

        public ProgressEventArgs(TimeSpan currentTime, TimeSpan totalTime)
        {
            CurrentTime = currentTime;
            TotalTime = totalTime;
        }
    }

    public class VolumeEventArgs : EventArgs
    {
        public float Volume { get; }
        public bool IsMuted { get; }

        public VolumeEventArgs(float volume, bool isMuted)
        {
            Volume = volume;
            IsMuted = isMuted;
        }
    }
}