using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

internal class DmgNoise : Generator
{
    private bool longMode = true;
    private int stepCount = 1;
    private int step = 0;
    private ushort lfsr = 0xBA55;
    
    public DmgNoise(int sampleRate) : base(sampleRate)
    {
    }

    public override void ControlUpdate(GeneratorUpdate ev)
    {
        if (ev is not DmgNoiseUpdate actual)
        {
            return;
        }

        if (actual.LongMode is not null)
        {
            longMode = actual.LongMode.Value;
        }

        if (actual.StepCount is not null)
        {
            stepCount = actual.StepCount.Value;
        }
    }

    protected override float Generate(double frequency)
    {
        var result = (lfsr & 0x1) == 0x1 ? 1.0f : -1.0f;
        Step(frequency);
        
        return result;
    }

    protected override void PhaseCallback()
    {
        step++;
        if (step < stepCount)
        {
            return;
        }
        
        step = 0;
        
        var value = (((lfsr & 0x2) >> 1) ^ (lfsr & 0x1)) == 0x1;
        var orVal = longMode ? 0x8000 : 0x80;
        lfsr |= value ? (ushort)orVal : (ushort)0x0; 
        lfsr >>= 1;
    }
}