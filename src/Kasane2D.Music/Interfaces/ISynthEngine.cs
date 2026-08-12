using Kasane2D.Music.Types;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Interfaces;

/// <summary>
/// Represents a synthesizer engine.
/// </summary>
public interface ISynthEngine : ISoundSubSystem
{
    /// <summary>
    /// Gets the conductor assigned to this synth engine.
    /// </summary>
    public IConductor? Conductor { get; }
    
    /// <summary>
    /// Plays a given song pattern immediately.
    /// </summary>
    /// <param name="pattern">The pattern to play.</param>
    public void Play(SongPattern pattern);
    
    /// <summary>
    /// Queues a song pattern to play after the current one is finished.
    /// </summary>
    /// <param name="pattern">The song pattern to play next.</param>
    public void Queue(SongPattern pattern);

    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause();

    /// <summary>
    /// Resumes playback.
    /// </summary>
    public void Resume();

    /// <summary>
    /// Stops playback and drops all patterns.
    /// </summary>
    public void Stop();
}