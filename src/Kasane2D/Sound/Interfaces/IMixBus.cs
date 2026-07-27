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
    
    public void Mix(int sampleCount);
}