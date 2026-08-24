using Kasane2D.Sound.Enums;

namespace Kasane2D.Sound.AudioEffects.Dsp;

internal struct DspFilter
{
    private const float q2Pole = 0.7071068f;
    private const float q3Pole = 1.0f;
    private const float q4Pole0 = 0.5411961f;
    private const float q4Pole1 = 1.306563f;
    private const float maxQ = 20.0f;

    private readonly int sampleRate;
    private BiQuad stage0;
    private BiQuad stage1;

    public DspFilter(int sampleRate)
    {
        this.sampleRate = sampleRate;
        stage0 = new BiQuad(sampleRate);
        stage1 = new BiQuad(sampleRate);
    }

    public DspFilterType Type
    {
        get;
        set
        {
            field = value;
            UpdateBiQuads();
        }
    } = DspFilterType.None;

    public int Slope
    {
        get;
        set
        {
            field = Math.Max(1, Math.Min(4, value));
            UpdateBiQuads();
        }
    } = 1;

    public float Frequency
    {
        get;
        set => field = MathF.Max(0.0f, MathF.Min(sampleRate / 2.0f, value));
    }

    public float Resonance
    {
        get;
        set
        {
            field = MathF.Max(0.0f, MathF.Min(value, 1.0f));
            UpdateBiQuads();
        }
    } = 0.0f;

    public float Q
    {
        get;
        set
        {
            field = value;
            UpdateBiQuads();
        }
    } = q2Pole;

    public float Gain
    {
        get;
        set
        {
            field = value;
            UpdateBiQuads();
        }
    } = 0.0f;

    public float Apply(float input)
    {
        if (Type == DspFilterType.None)
        {
            return input;
        }

        if (Type is not (DspFilterType.LowPass or DspFilterType.HighPass) || Slope <= 2)
        {
            return stage0.Next(input);
        }

        return stage1.Next(stage0.Next(input));
    }

    private void UpdateBiQuads()
    {
        if (Type == DspFilterType.None)
        {
            return;
        }

        switch (Type)
        {
            case DspFilterType.LowPass:
                switch (Slope)
                {
                    case 1:
                        stage0.ConfigureAsOnePoleLpf(Frequency);
                        break;
                    case 2:
                        stage0.ConfigureAsLpf(Frequency, ApplyResonance(q2Pole));
                        break;
                    case 3:
                        stage0.ConfigureAsOnePoleLpf(Frequency);
                        stage1.ConfigureAsLpf(Frequency, ApplyResonance(q3Pole));
                        break;
                    case 4:
                        stage0.ConfigureAsLpf(Frequency, q4Pole0);
                        stage1.ConfigureAsLpf(Frequency, ApplyResonance(q4Pole1));
                        break;
                }
                return;

            case DspFilterType.HighPass:
                switch (Slope)
                {
                    case 1:
                        stage0.ConfigureAsOnePoleHpf(Frequency);
                        break;
                    case 2:
                        stage0.ConfigureAsHpf(Frequency, Resonance);
                        break;
                    case 3:
                        stage0.ConfigureAsOnePoleHpf(Frequency);
                        stage1.ConfigureAsHpf(Frequency, ApplyResonance(q3Pole));
                        break;
                    case 4:
                        stage0.ConfigureAsHpf(Frequency, q4Pole0);
                        stage1.ConfigureAsHpf(Frequency, ApplyResonance(q4Pole1));
                        break;
                }
                return;

            case DspFilterType.BandPass:
                stage0.ConfigureAsBpf(Frequency, ApplyResonance(q2Pole));
                return;

            case DspFilterType.Notch:
                stage0.ConfigureAsNotch(Frequency, Q);
                return;

            case DspFilterType.Peak:
                stage0.ConfigureAsPeak(Frequency, Gain, Q);
                return;

            case DspFilterType.LowShelf:
                stage0.ConfigureAsLowShelf(Frequency, Gain, Q);
                return;

            case DspFilterType.HighShelf:
                stage0.ConfigureAsHighShelf(Frequency, Gain, Q);
                return;
        }
    }

    private float ApplyResonance(float baseQ)
    {
        return baseQ * MathF.Pow(maxQ / baseQ, Resonance);
    }
}