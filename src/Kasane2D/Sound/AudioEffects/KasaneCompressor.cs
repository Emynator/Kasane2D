using Kasane2D.Sound.AudioEffects.Dsp;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// Audio compressor.
/// </summary>
public class KasaneCompressor : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private EnvelopeFollower envelopeFollowerL;
    private EnvelopeFollower envelopeFollowerR;
    private float drive = 1.0f;
    private float threshold = 1.0f;
    private float ratio = 1.0f;
    private float makeup = 1.0f;

    internal KasaneCompressor
        (
        int sampleRate,
        float drive,
        float attack,
        float release,
        float threshold,
        int ratio,
        float makeupGain,
        float wet,
        string? name
        )
    {
        envelopeFollowerL = new(sampleRate);
        envelopeFollowerR = new(sampleRate);

        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasaneCompressor_{actual}";
        Drive = drive;
        Attack = attack;
        Release = release;
        Threshold = threshold;
        Ratio = ratio;
        MakeupGain = makeupGain;
        Wet = wet;
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
    /// Drive in db applied to the input before processing.
    /// </summary>
    public float Drive
    {
        get => 20.0f * MathF.Log10(drive);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-60.0f, MathF.Min(20.0f, value));
            drive = MathF.Pow(10.0f, actual / 20.0f);
            tlock.Release();
        }
    }

    /// <summary>
    /// Attack time in seconds.
    /// </summary>
    public float Attack
    {
        get => envelopeFollowerL.Attack;
        set
        {
            tlock.Wait();
            envelopeFollowerL.Attack = value;
            envelopeFollowerR.Attack = value;
            tlock.Release();
        }
    }

    /// <summary>
    /// Release time in seconds.
    /// </summary>
    public float Release
    {
        get => envelopeFollowerL.Release;
        set
        {
            tlock.Wait();
            envelopeFollowerL.Release = value;
            envelopeFollowerR.Release = value;
            tlock.Release();
        }
    }

    /// <summary>
    /// Threshold for compression start in dBFS.
    /// </summary>
    public float Threshold
    {
        get => 20.0f * MathF.Log10(threshold);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-80.0f, MathF.Min(0.0f, value));
            threshold = MathF.Pow(10.0f, actual / 20.0f);
            tlock.Release();
        }
    }

    /// <summary>
    /// Compression ratio.
    /// </summary>
    public int Ratio
    {
        get;
        set
        {
            tlock.Wait();
            field = value;
            ratio = MathF.Pow(10.0f, value / 20.0f);
            tlock.Release();
        }
    }

    /// <summary>
    /// Makup gain in db that is applied after processing.
    /// </summary>
    public float MakeupGain
    {
        get => 20.0f * MathF.Log10(makeup);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-60.0f, MathF.Min(20.0f, value));
            makeup = MathF.Pow(10.0f, actual / 20.0f);
            tlock.Release();
        }
    }
    
    /// <summary>
    /// Dry/Wet from 0.0f for full dry to 1.0f for full wet.
    /// </summary>
    public float Wet
    {
        get;
        set
        {
            tlock.Wait();
            field = MathF.Max(0.0f, MathF.Min(1.0f, value));
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

        var dry = 1.0f - Wet;
        for (var i = 0; i < inLeft.Length; i++)
        {
            var left = inLeft[i] * drive;
            var envelopeLeft = envelopeFollowerL.Next(left);
            if (envelopeLeft <= threshold)
            {
                outLeft[i] = left * makeup * Wet + inLeft[i] * dry;
            }
            else
            {
                outLeft[i] = (threshold + (left - threshold) / ratio) * makeup * Wet + inLeft[i] * dry;
            }

            var right = inRight[i] * drive;
            var envelopeRight = envelopeFollowerR.Next(right);
            if (envelopeRight <= threshold)
            {
                outRight[i] = right * makeup * Wet + inRight[i] * dry;
            }
            else
            {
                outRight[i] = (threshold + (right - threshold) / ratio) * makeup * Wet + inRight[i] * dry;
            }
        }

        tlock.Release();
    }
}