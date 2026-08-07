namespace Kasane2D.Music.Synthesis.Generators;

public abstract class Generator
{
    protected readonly int sampleRate;
    
    protected Generator(int sampleRate)
    {
        this.sampleRate = sampleRate;
    }
    
    protected double Phase { get; private set; } = 0.0d;

    public void Generate(Span<float> output, double frequency)
    {
        for (var i = 0; i < output.Length; i++)
        {
            output[i] = Generate(frequency);
        }
    }

    protected abstract float Generate(double frequency);

    protected void Step(double frequency)
    {
        Phase += frequency / sampleRate;
        if (Phase >= 2.0d)
        {
            Phase -= Math.Floor(Phase);
        }
        if (Phase >= 1.0d)
        {
            Phase -= 1.0d;
        }
    }
}