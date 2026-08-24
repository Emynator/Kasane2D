using Kasane2D.Sound.AudioEffects.Dsp;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// An audio limiter.
/// </summary>
public class KasaneLimiter : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private EnvelopeFollower envelopeFollowerL;
    private EnvelopeFollower envelopeFollowerR;
    private float drive = 1.0f;
    private float ceiling = 1.0f;
    private float gain = 1.0f;

    internal KasaneLimiter
        (
        int sampleRate,
        float drive,
        float attack,
        float release,
        float ceiling,
        float gain,
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
        Ceiling = ceiling;
        Gain = gain;
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
    /// Upper ceiling of the limiter in dBFS.
    /// </summary>
    public float Ceiling
    {
        get => 20.0f * MathF.Log10(ceiling);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-80.0f, MathF.Min(0.0f, value));
            ceiling = MathF.Pow(10.0f, actual / 20.0f);
            tlock.Release();
        }
    }

    /// <summary>
    /// Gain in db that is applied after processing.
    /// </summary>
    public float Gain
    {
        get => 20.0f * MathF.Log10(gain);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-60.0f, MathF.Min(20.0f, value));
            gain = MathF.Pow(10.0f, actual / 20.0f);
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
            var left = inLeft[i] * drive;
            var envelopeLeft = envelopeFollowerL.Next(left);
            if (envelopeLeft <= ceiling)
            {
                outLeft[i] = left * gain;
            }
            else
            {
                outLeft[i] = ceiling * gain;
            }

            var right = inRight[i] * drive;
            var envelopeRight = envelopeFollowerR.Next(right);
            if (envelopeRight <= ceiling)
            {
                outRight[i] = right * gain;
            }
            else
            {
                outRight[i] = ceiling * gain;
            }
        }

        tlock.Release();
    }
}