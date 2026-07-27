namespace Kasane2D.Sound.Interfaces;

public interface IMixBus
{
    string Name { get; }
    
    public int Level { get; set; }
    
    public int Pan { get; set; }
    
    IAudioBuffer OutLeft { get; }
    
    IAudioBuffer OutRight { get; }
    
    IAudioBuffer InLeft { get; }
    
    IAudioBuffer InRight { get; }
    
    IMixBus? Parent { get; }
    
    IReadOnlyCollection<IMixBus> Children { get; }
    
    IReadOnlyCollection<IAudioEffect> Effects { get; }

    public void AddEffect(IAudioEffect effect);
    
    public void RemoveEffect(string name);
    
    public void Mix(int sampleCount);
}