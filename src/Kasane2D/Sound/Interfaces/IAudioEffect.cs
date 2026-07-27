namespace Kasane2D.Sound.Interfaces;

public interface IAudioEffect
{
    public string Name { get; }
    
    public void Apply(Span<float> left, Span<float> right);
}