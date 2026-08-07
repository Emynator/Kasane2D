namespace Kasane2D.Music.Synthesis.Generators;

public class EmptyGenerator : Generator
{
    public EmptyGenerator(int sampleRate) : base(sampleRate)
    {
    }

    protected override float Generate(double frequency)
    {
        return 0.0f;
    }
}