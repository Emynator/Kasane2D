namespace Kasane2D.Sound.Interfaces;

public interface ISoundSystem
{
    public int SampleRate { get; }
    
    public IAudioMixer AudioMixer { get; }
    
    public ISfxManager SfxManager { get; }
    
    public void Process(int sampleCount);
}