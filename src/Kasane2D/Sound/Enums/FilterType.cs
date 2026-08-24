using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Sound.Enums;

/// <summary>
/// The filter type of a <see cref="KasaneFilter"/>.
/// </summary>
public enum FilterType
{
    /// <summary>
    /// A low-pass filter.
    /// </summary>
    LowPass,
    /// <summary>
    /// A high-pass filter.
    /// </summary>
    HighPass,
    /// <summary>
    /// A bandpass filter.
    /// </summary>
    BandPass,
}