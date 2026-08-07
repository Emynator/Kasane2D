namespace Kasane2D.Music.Synthesis;

internal class Envelope
{
    private readonly int samplesPerMs;
    private EnvelopeState state = EnvelopeState.End;
    private int attackEnd;
    private int decayEnd;
    private int releaseEnd;
    private int position = 0;
    private float currentGain = 0.0f;

    public Envelope(int sampleRate)
    {
        samplesPerMs = sampleRate / 1000;
        attackEnd = (int)(0.5f * samplesPerMs);
        decayEnd = (int)(1.0f * samplesPerMs);
        releaseEnd = (int)(1.0f * samplesPerMs);
    }

    public float Attack
    {
        get;
        set
        {
            attackEnd = (int)(value * samplesPerMs);
            field = value;
        }
    } = 0.5f;

    public float Decay
    {
        get;
        set
        {
            decayEnd = (int)(value * samplesPerMs);
            field = value;
        }
    } = 1.0f;

    public float Sustain { get; set; } = 1.0f;

    public float Release
    {
        get;
        set
        {
            releaseEnd = (int)(value * samplesPerMs);
            field = value;
        }
    } = 1.0f;

    public void Apply(Span<float> input, Span<float> output)
    {
        if (input.Length != output.Length)
        {
            throw new ArgumentException($"{nameof(input)} and {nameof(output)} must be the same length.");
        }

        for (var i = 0; i < input.Length; i++)
        {
            var gain = 0.0f;
            switch (state)
            {
                case EnvelopeState.Attack:
                    gain = (float)double.Lerp(0.0d, 1.0d, (double)position / attackEnd);
                    if (currentGain < gain)
                    {
                        currentGain = gain;
                    }
                    
                    output[i] = input[i] * currentGain;
                    
                    position++;
                    if (position >= attackEnd)
                    {
                        position = 0;
                        state = EnvelopeState.Decay;
                    }
                    
                    break;
                
                case EnvelopeState.Decay:
                    gain = (float)double.Lerp(1.0d, Sustain, (double)(position) / decayEnd);
                    if (currentGain > gain)
                    {
                        currentGain = gain;
                    }
                    
                    output[i] = input[i] * currentGain;
                    position++;
                    if (position >= decayEnd)
                    {
                        position = 0;
                        state = EnvelopeState.Sustain;
                        currentGain = Sustain;
                    }
                    
                    break;
                
                case EnvelopeState.Sustain:
                    output[i] = input[i] * currentGain;
                    break;
                
                case EnvelopeState.Release:
                    gain = (float)double.Lerp(Sustain, 0.0d, (double)position / releaseEnd);
                    if (currentGain > gain)
                    {
                        currentGain = gain;
                    }
                    
                    output[i] = input[i] * currentGain;
                    position++;
                    if (position >= releaseEnd)
                    {
                        position = 0;
                        state = EnvelopeState.End;
                        currentGain = 0.0f;
                    }

                    break;
                
                default:
                    output[i] = 0.0f;
                    break;
            }
        }
    }

    public void EnterRelease()
    {
        state = EnvelopeState.Release;
        position = 0;
    }

    public void Reset()
    {
        state = EnvelopeState.Attack;
        position = 0;
    }

    private enum EnvelopeState
    {
        Attack,
        Decay,
        Sustain,
        Release,
        End,
    }
}