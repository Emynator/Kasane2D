namespace Kasane2D.Sound.Interfaces;

public interface IAudioBuffer
{
    public int Length { get; }
    
    public float Read();

    public float[] Read(int sampleCount);

    public void Read(Span<float> outBuffer);
    
    public void Write(float sample);
    
    public void Write(ReadOnlySpan<float> samples);
}