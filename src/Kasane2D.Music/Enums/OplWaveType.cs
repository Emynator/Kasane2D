using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Enums;

/// <summary>
/// Wave type for <see cref="Opl2Voice"/>'s operators.
/// </summary>
public enum OplWaveType
{
    /// <summary>
    /// Sine wave.
    /// </summary>
    Sine,
    /// <summary>
    /// Positive half of a sine wave.
    /// </summary>
    HalfSine,
    /// <summary>
    /// Sine wave with negative half folded to positive.
    /// </summary>
    AbsSine,
    /// <summary>
    /// Sine wave with negative half folded to positive and only the first and third quarter of the wave.
    /// </summary>
    SineSaw,
}