using Kasane2D.Music.Types.SequenceEvents;

namespace Kasane2D.Music.Synthesis.Generators;

public class EmptyGenerator : Generator
{
    public EmptyGenerator(int sampleRate) : base(sampleRate)
    {
    }

    public override void ControlUpdate(GeneratorUpdate ev)
    {
    }

    protected override float Generate(double frequency)
    {
        return 0.0f;
    }
}