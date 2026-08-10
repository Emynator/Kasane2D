using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Manages sound effect playback.
/// </summary>
public interface ISfxManager
{
    /// <summary>
    /// Gets the number of sound effect channels that can play at the same time.
    /// </summary>
    public int ChannelCount { get; }
    
    /// <summary>
    /// Gets the number of sound effect channels that are currently playing.
    /// </summary>
    public int BusyChannels { get; }
    
    /// <summary>
    /// Gets if all channels are currently busy.
    /// </summary>
    public bool AllChannelsBusy { get; }
    
    /// <summary>
    /// Gets the number of sound effects currently waiting for a free channel to be played.
    /// </summary>
    public int QueueLength { get; }
    
    /// <summary>
    /// Plays the given sound effect if a channel is available or puts it into the playback queue if not.
    /// </summary>
    /// <param name="sound">The sound effect to play.</param>
    public void Play(AudioFileStream sound);

    /// <summary>
    /// Stop playback of all currently playing sound effects and removes them from the channel.
    /// </summary>
    public void StopAll();

    /// <summary>
    /// Drop all sound effects currently waiting for a free channel from the playback queue.
    /// </summary>
    public void DropQueue();
}