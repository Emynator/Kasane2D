namespace Kasane2D.Music.Enums;

/// <summary>
/// Generator kind to use in a synthesizer track.
/// </summary>
public enum GeneratorKind
{
    /// <summary>
    /// Empty generator that does nothing.
    /// </summary>
    None,
    /// <summary>
    /// Custom generator implementation.
    /// </summary>
    Custom,
    /// <summary>
    /// An oscillator generating basic wave forms.
    /// </summary>
    BasicOscillator,
    /// <summary>
    /// A DMG inspired LFSR noise generator.
    /// </summary>
    DmgNoise,
    /// <summary>
    /// A SID inspired LFSR noise generator.
    /// </summary>
    SidNoise,
    /// <summary>
    /// A retro wave table generator with 8 or 4 bit wave tables.
    /// </summary>
    RetroWaveTable,
    /// <summary>
    /// A simple, tracker-inspired sampler.
    /// </summary>
    Sampler,
}