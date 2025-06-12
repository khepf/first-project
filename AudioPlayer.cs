using NAudio.Wave;

class AudioPlayer
{
    private WaveOutEvent? outputDevice;
    private AudioFileReader? audioFile;
    private bool isPaused = false;

    public bool IsPaused => isPaused;
    public bool IsPlaying => outputDevice?.PlaybackState == PlaybackState.Playing;

    public void Play(string filePath)
    {
        Stop(); // Stop any current playback
        outputDevice = new WaveOutEvent();
        audioFile = new AudioFileReader(filePath);
        outputDevice.Init(audioFile);
        outputDevice.Play();
        isPaused = false;
    }

    public void Pause()
    {
        if (outputDevice?.PlaybackState == PlaybackState.Playing)
        {
            outputDevice.Pause();
            isPaused = true;
        }
    }

    public void Resume()
    {
        if (isPaused && outputDevice?.PlaybackState == PlaybackState.Paused)
        {
            outputDevice.Play();
            isPaused = false;
        }
    }

    public void Stop()
    {
        outputDevice?.Stop();
        outputDevice?.Dispose();
        audioFile?.Dispose();
        outputDevice = null;
        audioFile = null;
        isPaused = false;
    }
}