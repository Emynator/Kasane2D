namespace Kasane2D.Sound.Interfaces;

public interface IAudioMixer
{
    public IMixBus Master { get; }
    
    public IMixBus CreateMixBus(string name, IMixBus? parent = null);
    
    public void ReleaseMixBus(IMixBus bus);
}