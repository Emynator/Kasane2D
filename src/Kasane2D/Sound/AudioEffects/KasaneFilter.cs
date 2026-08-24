using Kasane2D.Sound.AudioEffects.Dsp;
using Kasane2D.Sound.Enums;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// A simple, configurable filter.
/// </summary>
public class KasaneFilter : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private DspFilter dspFilterL;
    private DspFilter dspFilterR;

    internal KasaneFilter
        (
        int sampleRate,
        FilterType type,
        int slope,
        float cutoffFrequency,
        float resonance,
        string? name
        )
    {
        dspFilterL = new DspFilter(sampleRate);
        dspFilterR = new DspFilter(sampleRate);

        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasaneFilter_{actual}";
        Type = type;
        Slope = slope;
        CutoffFrequency = cutoffFrequency;
        Resonance = resonance;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public bool Bypass
    {
        get;
        set
        {
            tlock.Wait();
            field = value;
            tlock.Release();
        }
    }

    /// <summary>
    /// The type of the filter.
    /// </summary>
    public FilterType Type
    {
        get;
        set
        {
            tlock.Wait();

            field = value;
            var t = Type switch
            {
                FilterType.LowPass => DspFilterType.LowPass,
                FilterType.HighPass => DspFilterType.HighPass,
                FilterType.BandPass => DspFilterType.BandPass,
                _ => DspFilterType.None,
            };

            dspFilterL.Type = t;
            dspFilterR.Type = t;

            tlock.Release();
        }
    }

    /// <summary>
    /// The slope of the filter from 1 to 4, representing a 6dB to 24dB per octave.
    /// </summary>
    public int Slope
    {
        get => dspFilterL.Slope;
        set
        {
            tlock.Wait();

            dspFilterL.Slope = value;
            dspFilterR.Slope = value;

            tlock.Release();
        }
    }

    /// <summary>
    /// The cutoff frequency of the filter.
    /// </summary>
    public float CutoffFrequency
    {
        get => dspFilterL.Frequency;
        set
        {
            tlock.Wait();

            dspFilterL.Frequency = value;
            dspFilterR.Frequency = value;

            tlock.Release();
        }
    }

    /// <summary>
    /// The filter's resonance in percent from 0.0f to 1.0f.
    /// </summary>
    public float Resonance
    {
        get => dspFilterL.Resonance;
        set
        {
            tlock.Wait();

            dspFilterL.Resonance = value;
            dspFilterR.Resonance = value;

            tlock.Release();
        }
    }

    /// <inheritdoc/>
    public void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        )
    {
        tlock.Wait();
        
        if (Bypass)
        {
            inLeft.CopyTo(outLeft);
            inRight.CopyTo(outRight);
            tlock.Release();
            
            return;
        }

        for (var i = 0; i < inLeft.Length; i++)
        {
            outLeft[i] = dspFilterL.Apply(inLeft[i]);
            outRight[i] = dspFilterR.Apply(inRight[i]);
        }

        tlock.Release();
    }
}