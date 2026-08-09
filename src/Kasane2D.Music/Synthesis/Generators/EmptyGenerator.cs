using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

internal class EmptyGenerator : Generator
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