using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Synthesis.Generators;

internal class BasicOscillator : Generator
{
    public BasicOscillator(int sampleRate) : base(sampleRate)
    {
    }

    public BasicWave Shape { get; set; } = BasicWave.Sine;

    public double DutyCycle
    {
        get;
        set
        {
            switch (value)
            {
                case < 0.0d:
                    field = 0.0d;
                    return;
                case > 1.0d:
                    field = 1.0d;
                    return;
                default:
                    field = value;
                    break;
            }
        }
    } = 0.5d;

    protected override float Generate(double frequency)
    {
        var result = Shape switch
        {
            BasicWave.Sine => (float)Math.Sin(Math.Tau * Phase),
            BasicWave.Triangle => (float)(1.0d - 4.0d * Math.Abs(Phase - 0.5d)),
            BasicWave.Saw => (float)(Phase * 2.0d - 1.0d),
            BasicWave.Square => Square(frequency),
            _ => throw new InvalidOperationException(),
        };

        Step(frequency);
        
        return result;
    }

    private float Square(double frequency)
    {
        var phaseStep = frequency / sampleRate;
        var result = Phase < DutyCycle ? 1.0d : -1.0d;

        result += PolyBlep(Phase, phaseStep);

        var dutyPhase = Phase - DutyCycle;
        if (dutyPhase < 0.0d)
        {
            dutyPhase += 1.0d;
        }

        result -= PolyBlep(dutyPhase, phaseStep);

        return (float)result;
    }
    
    private double PolyBlep(double phase, double phaseStep)
    {
        if (phase < phaseStep)
        {
            var t = phase / phaseStep;
            return t + t - t * t - 1.0d;
        }

        if (phase > 1.0d - phaseStep)
        {
            var t = (phase - 1.0d) / phaseStep;
            return t * t + 2 * t + 1.0d;
        }

        return 0.0d;
    }
}