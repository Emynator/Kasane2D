namespace Kasane2D.Config;

/// <summary>
/// Configuration of the sound system.
/// </summary>
public class AudioConfiguration
{
    /// <summary>
    /// Buffer size in ms the sound system processes. Default is 15ms.
    /// </summary>
    /// <remarks>The sound system thread processes one buffer at a time and puts that buffer into a queue. The
    /// backend takes an available buffer from that queue when its sound device requests another buffer. The buffer
    /// size and the maximum number of buffers in that queue influences the audio latency. At the same time, if the
    /// numbers are too low, there are cracks and pops because the sound driver runs out of samples to play.</remarks>
    public int BufferSizeInMs { get; set; } = 15;
    
    /// <summary>
    /// Maximum number of sound buffers to be put in the buffer queue. Default is 4.
    /// </summary>
    /// <remarks>The sound system thread processes one buffer at a time and puts that buffer into a queue. The
    /// backend takes an available buffer from that queue when its sound device requests another buffer. The buffer
    /// size and the maximum number of buffers in that queue influences the audio latency. At the same time, if the
    /// numbers are too low, there are cracks and pops because the sound driver runs out of samples to play.</remarks>
    public int BuffersInQueue { get; set; } = 4;
    
    /// <summary>
    /// Sample rate in Hz. Default is 44.1 kHz (CD audio quality).
    /// </summary>
    public int SampleRate { get; set; } = 44100;

    /// <summary>
    /// Number of sound effect channels. This is the maximum number of sound effects that can play at the same time.
    /// Default is 32.
    /// </summary>
    public int SfxChannelCount { get; set; } = 32;
}