namespace Kasane2D.Config;

public class AudioConfiguration
{
    public int DefaultBufferSizeInMs { get; set; } = 50;
    
    public int SampleRate { get; set; } = 44100;

    public int SfxChannelCount { get; set; } = 32;
}