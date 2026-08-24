using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// A simple utility to change gain and pan inside an effect-chain.
/// </summary>
public class KasaneUtility : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private float gain = 1.0f;
    private float leftGain;
    private float rightGain;

    internal KasaneUtility(float gain, int pan, string? name)
    {
        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasaneUtility_{actual}";
        Gain = gain;
        Pan = pan;
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
    } = false;

    /// <summary>
    /// Gain in db that will be applied. Ranges from -60dB to +20dB.
    /// </summary>
    /// <remarks><seealso href="https://en.wikipedia.org/wiki/Decibel#Acoustics"/></remarks>
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

    /// <summary>
    /// Pan that will be applied.
    /// </summary>
    /// <remarks>Ranges from -100 to 100 where -100 is fully left and 100 is fully right.</remarks>
    public int Pan
    {
        get;
        set
        {
            tlock.Wait();

            var actual = value;
            if (value > 100)
            {
                actual = 100;
            }
            if (value < -100)
            {
                actual = -100;
            }
            field = actual;

            var normalized = (1.0f / 100 * actual + 1.0f) * 0.5f;
            var angle = normalized * MathF.PI * 0.5f;
            leftGain = MathF.Cos(angle);
            rightGain = MathF.Sin(angle);

            tlock.Release();
        }
    } = 0;

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
            outLeft[i] = inLeft[i] * gain * leftGain;
            outRight[i] = inRight[i] * gain * rightGain;
        }

        tlock.Release();
    }
}