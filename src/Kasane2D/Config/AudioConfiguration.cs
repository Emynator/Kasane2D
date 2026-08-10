namespace Kasane2D.Config;

public class AudioConfiguration
{
    public int BufferSizeInMs { get; set; } = 15;
    
    public int BuffersInQueue { get; set; } = 4;
    
    public int SampleRate { get; set; } = 44100;

    public int SfxChannelCount { get; set; } = 32;
}