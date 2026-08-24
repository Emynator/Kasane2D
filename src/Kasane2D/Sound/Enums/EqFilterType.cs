using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Sound.Enums;

/// <summary>
/// The filtering type of a single parameter of a <see cref="KasaneEq8"/>.
/// </summary>
public enum EqFilterType
{
    /// <summary>
    /// High-pass filter with 24dB/octave slope.
    /// </summary>
    HighPass4X,
    /// <summary>
    /// High-pass filter with 12dB/octave slope.
    /// </summary>
    HighPass,
    /// <summary>
    /// Low-shelf filter.
    /// </summary>
    LowShelf,
    /// <summary>
    /// Peak filter.
    /// </summary>
    Peak,
    /// <summary>
    /// High-shelf filter.
    /// </summary>
    HighShelf,
    /// <summary>
    /// Low-pass filter with 12dB/octave slope.
    /// </summary>
    LowPass,
    /// <summary>
    /// Low-pass filter with 24dB/octave slope.
    /// </summary>
    LowPass4X,
}