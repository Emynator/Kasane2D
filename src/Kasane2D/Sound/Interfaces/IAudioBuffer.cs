namespace Kasane2D.Sound.Interfaces;

public interface IAudioBuffer
{
    public int Length { get; }
    
    public float Read();

    public float[] Read(int sampleCount);
    
    public void Write(float sample);
    
    public void Write(float[] samples);
}