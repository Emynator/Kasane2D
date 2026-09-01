using Kasane2D.Music.Enums;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

internal struct Opl2Operator
{
    private readonly int sampleRate;
    private Envelope envelope;
    private OplWaveType waveType = OplWaveType.Sine;
    private float feedbackAmount = 0.0f;
    private double frequencyFactor = 0.0d;
    private bool isFixed = false;
    private double phase = 0.0d;
    private float feedBack = 0.0f;

    public Opl2Operator(int sampleRate)
    {
        this.sampleRate = sampleRate;
        envelope = new(sampleRate);
    }

    public float Next(double frequency, float modulation = 0.0f)
    {
        var sine = Math.Sin(Math.Tau * phase + modulation + feedbackAmount * feedBack);
        feedBack = waveType switch
        {
            OplWaveType.Sine => (float)sine,
            OplWaveType.HalfSine => phase <= 0.5d ? (float)sine : 0.0f,
            OplWaveType.AbsSine => MathF.Abs((float)sine),
            OplWaveType.SineSaw => Math.Abs(phase - 0.5d) > 0.25d ? (float)sine : 0.0f,
            _ => 0.0f,
        };
        feedBack = envelope.Apply(feedBack);

        phase += isFixed
            ? frequencyFactor / sampleRate
            : frequencyFactor * frequency / sampleRate;
        
        if (phase >= 2.0d)
        {
            var cycles = Math.Floor(phase);
            phase -= cycles;
        }

        if (phase >= 1.0d)
        {
            phase -= 1.0d;
        }

        return feedBack;
    }

    public void Reset()
    {
        phase = 0.0d;
        envelope.Reset();
    }

    public void Update(Opl2OperatorUpdate update)
    {
        if (update.EnvelopeUpdate.DoUpdate)
        {
            envelope.Attack = update.EnvelopeUpdate.Attack;
            envelope.Decay = update.EnvelopeUpdate.Decay;
            envelope.Sustain = update.EnvelopeUpdate.Sustain;
            envelope.Release = update.EnvelopeUpdate.Release;
        }

        if (update.WaveType is not null)
        {
            waveType = update.WaveType.Value;
        }
        
        if (update.FeedbackAmount is not null)
        {
            var x = Math.Clamp(update.FeedbackAmount.Value, 0.0f, 1.0f);
            feedbackAmount = 4.0f * MathF.PI * x * x;
        }
        
        if (update.Frequency is not null)
        {
            frequencyFactor = update.Frequency.Value;
        }
        
        if (update.IsFixed is not null)
        {
            isFixed = update.IsFixed.Value;
        }
    }
}