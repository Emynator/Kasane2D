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
    /// <param name="name">Optional: the filter's name. Default is a GUID.</param>
    /// <returns>The created Eq8.</returns>
    public static KasaneEq8 CreateEq8(this ISoundSystem soundSystem, string? name = null)
    {
        return new(soundSystem.SampleRate, name);
    }
}