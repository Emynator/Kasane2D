namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Primary interface of the sound system.
/// </summary>
public interface ISoundSystem
{
    /// <summary>
    /// Gets the sound system's sample rate in Hz.
    /// </summary>
    public int SampleRate { get; }
    
    /// <summary>
    /// Gets the sound system's buffer size in samples.
    /// </summary>
    public int BufferSize { get; }
    
    /// <summary>
    /// Gets the sound system's audio mixer.
    /// </summary>
    public IAudioMixer AudioMixer { get; }
    
    /// <summary>
    /// Gets the sound system's sound effect manager.
    /// </summary>
    public ISfxManager SfxManager { get; }
    
    /// <summary>
    /// Gets the sound system's music player.
    /// </summary>
    public IMusicPlayer MusicPlayer { get; }
    
    /// <summary>
    /// Process one buffer worth of samples.
    /// </summary>
    /// <remarks>This method is only intended to be called by backend implementations and should not be called from
    /// user code.</remarks>
    public void Process();

    /// <summary>
    /// Add a custom sound subsystem.
    /// </summary>
    /// <param name="system">The subsystem to add.</param>
    public void AddSubSystem(ISoundSubSystem system);
    
    /// <summary>
    /// Removes a custom sound subsystem.
    /// </summary>
    /// <param name="id">GUID of the subsystem to remove.</param>
    public void RemoveSubSystem(Guid id);
    
    /// <summary>
    /// Create an audio ring buffer of the determined size.
    /// </summary>
    /// <param name="bufferSize">Size of the audio buffer in samples.</param>
    /// <returns>The audio buffer.</returns>
    public IAudioBuffer CreateBuffer(int bufferSize);
}