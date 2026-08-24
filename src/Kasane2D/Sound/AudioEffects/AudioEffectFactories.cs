using Kasane2D.Sound.Enums;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// Factory functions for the build-in audio effects.
/// </summary>
public static class AudioEffectFactories
{
    /// <summary>
    /// Creates a KasaneFilter.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="type">The filter's type.</param>
    /// <param name="slope">The filter's slope.</param>
    /// <param name="cutoffFrequency">The filter's cutoff frequency.</param>
    /// <param name="resonance">The filter's resonance.</param>
    /// <param name="name">Optional: the filter's name. Default is a GUID.</param>
    /// <returns>The created filter.</returns>
    public static KasaneFilter CreateFilter
        (
        this ISoundSystem soundSystem,
        FilterType type,
        int slope,
        float cutoffFrequency,
        float resonance,
        string? name = null
        )
    {
        return new(soundSystem.SampleRate, type, slope, cutoffFrequency, resonance, name);
    }

    /// <summary>
    /// Creates a KasaneEq8.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="name">Optional: the eq's name. Default is a GUID.</param>
    /// <returns>The created eq8.</returns>
    public static KasaneEq8 CreateEq8(this ISoundSystem soundSystem, string? name = null)
    {
        return new(soundSystem.SampleRate, name);
    }

    /// <summary>
    /// Creates a KasaneDelay.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="delay">The delay time in seconds.</param>
    /// <param name="decayGain">The decay gain in dB.</param>
    /// <param name="feedback">The feedback amount in % from 0.0 to 1.0.</param>
    /// <param name="name">Optional: the delay's name. Default is a GUID.</param>
    /// <returns>The created delay.</returns>
    public static KasaneDelay CreateDelay
        (
        this ISoundSystem soundSystem,
        float delay,
        float decayGain,
        float feedback,
        string? name = null
        )
    {
        return new(soundSystem, delay, decayGain, feedback, name);
    }

    /// <summary>
    /// Creates a KasanePingPongDelay.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="delay">The delay time in seconds.</param>
    /// <param name="decayGain">The decay gain in dB.</param>
    /// <param name="feedback">The feedback amount in % from 0.0 to 1.0.</param>
    /// <param name="name">Optional: the delay's name. Default is a GUID.</param>
    /// <returns>The created delay.</returns>
    public static KasanePingPongDelay CreatePingPongDelay
        (
        this ISoundSystem soundSystem,
        float delay,
        float decayGain,
        float feedback,
        string? name = null
        )
    {
        return new(soundSystem, delay, decayGain, feedback, name);
    }

    /// <summary>
    /// Creates a KasaneUtility.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="gain">The gain in dB.</param>
    /// <param name="pan">The pan from -100 to 100.</param>
    /// <param name="name">Optional: the utility's name. Default is a GUID.</param>
    /// <returns>The created utility.</returns>
    public static KasaneUtility CreateUtility(this ISoundSystem soundSystem, float gain, int pan, string? name = null)
    {
        return new(gain, pan, name);
    }

    /// <summary>
    /// Creates a KasaneCompressor.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="drive">The drive in dB.</param>
    /// <param name="attack">The attack time in seconds.</param>
    /// <param name="release">The release time in seconds.</param>
    /// <param name="threshold">The threshold in dBFS.</param>
    /// <param name="ratio">The compression ratio.</param>
    /// <param name="makeupGain">The makeup gain in dB.</param>
    /// <param name="name">Optional: the utility's name. Default is a GUID.</param>
    /// <returns>The created compressor.</returns>
    public static KasaneCompressor CreateCompressor
        (
        this ISoundSystem soundSystem,
        float drive,
        float attack,
        float release,
        float threshold,
        int ratio,
        float makeupGain,
        string? name = null
        )
    {
        return new(soundSystem.SampleRate, drive, attack, release, threshold, ratio, makeupGain, name);
    }
    
    /// <summary>
    /// Creates a KasaneCompressor.
    /// </summary>
    /// <param name="soundSystem">The sound system.</param>
    /// <param name="drive">The drive in dB.</param>
    /// <param name="attack">The attack time in seconds.</param>
    /// <param name="release">The release time in seconds.</param>
    /// <param name="ceiling">The ceiling in dBFS.</param>
    /// <param name="gain">The gain in dB.</param>
    /// <param name="name">Optional: the utility's name. Default is a GUID.</param>
    /// <returns>The created compressor.</returns>
    public static KasaneLimiter CreateLimiter
        (
        this ISoundSystem soundSystem,
        float drive,
        float attack,
        float release,
        float ceiling,
        float gain,
        string? name = null
        )
    {
        return new(soundSystem.SampleRate, drive, attack, release, ceiling, gain, name);
    }
}