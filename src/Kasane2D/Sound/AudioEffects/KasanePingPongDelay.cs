using Kasane2D.Sound.Extensions;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// A ping-pong delay effect.
/// </summary>
public class KasanePingPongDelay : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly ISoundSystem soundSystem;
    private IAudioBuffer delayBufferL = null!;
    private IAudioBuffer delayBufferR = null!;
    private int delaySamples = 0;
    private float decay;

    internal KasanePingPongDelay
        (
        ISoundSystem soundSystem,
        float delay,
        float decayGain,
        float feedback,
        float wet,
        string? name
        )
    {
        this.soundSystem = soundSystem;

        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasanePingPongDelay_{actual}";
        Delay = delay;
        DecayGain = decayGain;
        Feedback = feedback;
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
    } = false;

    /// <summary>
    /// Delay time in seconds. Minimum is 0.0f.
    /// </summary>
    public float Delay
    {
        get;
        set
        {
            tlock.Wait();

            field = MathF.Max(0.0f, value);
            var newDelaySamples = (int)(field * soundSystem.SampleRate);
            if (newDelaySamples < 1)
            {
                newDelaySamples = 1;
            }

            var newBufferL = soundSystem.CreateBuffer(newDelaySamples);
            var newBufferR = soundSystem.CreateBuffer(newDelaySamples);

            if (delaySamples == 1 || newDelaySamples == 1)
            {
                delaySamples = newDelaySamples;
                delayBufferL = newBufferL;
                delayBufferR = newBufferR;

                var tmp = new float[delaySamples];
                delayBufferL.Write(tmp);
                delayBufferR.Write(tmp);

                tlock.Release();

                return;
            }

            var l = delayBufferL.Read(delaySamples);
            var r = delayBufferR.Read(delaySamples);
            var stream = new StereoAudioStream(delaySamples, l, r)
                .Resample(delaySamples, newDelaySamples)
                .AsStereoStream();

            newBufferL.Write(stream.GetLeft());
            newBufferR.Write(stream.GetRight());

            delaySamples = newDelaySamples;
            delayBufferL = newBufferL;
            delayBufferR = newBufferR;

            tlock.Release();
        }
    }

    /// <summary>
    /// Decay gain on delay. Ranges from -60.0dB to -1.0dB.
    /// </summary>
    public float DecayGain
    {
        get => 20.0f * MathF.Log10(decay);
        set
        {
            tlock.Wait();

            var actualValue = MathF.Max(-60.0f, MathF.Min(-1.0f, value));
            decay = MathF.Pow(10.0f, actualValue / 20.0f);

            tlock.Release();
        }
    }

    /// <summary>
    /// Feedback amount from 0.0f to 1.0f.
    /// </summary>
    public float Feedback
    {
        get;
        set
        {
            tlock.Wait();
            field = MathF.Max(0.0f, MathF.Min(1.0f, value));
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
            var left = delayBufferL.Read();
            var right = delayBufferR.Read();
            outLeft[i] = (inLeft[i] + left) * Wet + inLeft[i] * dry;
            outRight[i] = (inRight[i] + right) * Wet + inRight[i] * dry;

            var delay = (inLeft[i] + inRight[i]) / 2.0f * decay + right * Feedback;
            delayBufferL.Write(delay);
            delayBufferR.Write(left * decay);
        }

        tlock.Release();
    }
    
    /// <summary>
    /// Resets the delay and clears all pending audio from it.
    /// </summary>
    public void Reset()
    {
        tlock.Wait();
        
        delayBufferL.Clear();
        delayBufferR.Clear();
        var tmp = new float[delaySamples];
        delayBufferL.Write(tmp);
        delayBufferR.Write(tmp);
        
        tlock.Release();
    }
}