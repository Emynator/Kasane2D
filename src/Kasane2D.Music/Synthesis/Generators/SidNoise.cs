using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

internal class SidNoise : Generator
{
    private int lfsr;
    
    public SidNoise(int sampleRate) : base(sampleRate)
    {
    }

    public override void ControlUpdate(GeneratorUpdate ev)
    {
    }

    protected override float Generate(double frequency)
    {
        Step(frequency);
        
        var output = ((lfsr & 0x400000) >> 15)
            | ((lfsr & 0x100000) >> 14)
            | ((lfsr & 0x10000) >> 11)
            | ((lfsr & 0x2000) >> 9)
            | ((lfsr & 0x800) >> 8)
            | ((lfsr & 0x80) >> 5)
            | ((lfsr & 0x10) >> 3)
            | ((lfsr & 0x2) >> 1);

        return output / 127.5f - 1.0f;
    }

    protected override void PhaseCallback()
    {
        var value = ((lfsr & 0x400000) ^ ((lfsr & 0x20000) << 5)) << 1;
        lfsr |= value;
        lfsr = (lfsr >> 1) | value;
    }
}