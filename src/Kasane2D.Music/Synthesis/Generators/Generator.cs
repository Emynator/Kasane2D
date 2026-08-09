using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

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

    public abstract void ControlUpdate(GeneratorUpdate ev);

    public void Reset()
    {
        Phase = 0.0d;
    }

    protected abstract float Generate(double frequency);

    protected virtual void PhaseCallback()
    {
    }

    protected void Step(double frequency)
    {
        Phase += frequency / sampleRate;
        if (Phase >= 2.0d)
        {
            var cycles = Math.Floor(Phase);
            Phase -= cycles;
            
            for (var i = 0; i < (int)cycles; i++)
            {
                PhaseCallback();
            }
        }
        if (Phase >= 1.0d)
        {
            Phase -= 1.0d;
            PhaseCallback();
        }
    }
}