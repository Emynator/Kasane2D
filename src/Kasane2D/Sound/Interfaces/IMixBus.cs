namespace Kasane2D.Sound.Interfaces;

public interface IMixBus
{
    string Name { get; }
    
    public int Level { get; set; }
    
    public int Pan { get; set; }
    
    IMixBus? Parent { get; }
    
    IReadOnlyCollection<IMixBus> Children { get; }
    
    IReadOnlyCollection<IAudioEffect> Effects { get; }
    
    public void WriteLeft(ReadOnlySpan<float> samples);
    
    public void WriteRight(ReadOnlySpan<float> samples);
    
    public float[] ReadLeft(int sampleCount);
    
    public float[] ReadRight(int sampleCount);

    public void AddEffect(IAudioEffect effect);
    
    public void RemoveEffect(string name);
}