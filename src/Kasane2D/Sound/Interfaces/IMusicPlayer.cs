using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// A simple player for playing music files.
/// </summary>
public interface IMusicPlayer
{
    /// <summary>
    /// Gets if a file is currently playing.
    /// </summary>
    public bool IsPlaying { get; }
    
    /// <summary>
    /// Gets if the currently playing file will loop forever.
    /// </summary>
    public bool IsLooping { get; }
    
    /// <summary>
    /// Gets the number of songs in the queue.
    /// </summary>
    public int QueueLength { get; }
    
    /// <summary>
    /// Plays a given audio file immediately.
    /// </summary>
    /// <param name="song">The audio file to play.</param>
    /// <param name="loop">Optional: if the file should be looped forever. Default is false.</param>
    public void Play(AudioFileStream song, bool loop = false);

    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause();

    /// <summary>
    /// Resumes playback.
    /// </summary>
    public void Resume();

    /// <summary>
    /// Pauses playback and resets playback position.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Ends a loop and returns to feeding the next song from the queue.
    /// </summary>
    public void EndLoop();
    
    /// <summary>
    /// Put a song into the queue to be played after the song before it has finished.
    /// </summary>
    /// <param name="song">The song to put into the queue.</param>
    /// <remarks>The queue is ignored when in loop mode. Call <see cref="EndLoop"/> to return to queue based playback.</remarks>
    public void Queue(AudioFileStream song);

    /// <summary>
    /// Drops all songs currently waiting in the playback queue.
    /// </summary>
    public void ClearQueue();
}