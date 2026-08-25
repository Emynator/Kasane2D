using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Sound.Enums;

/// <summary>
/// Distortion types for the <see cref="KasaneOverdrive"/> effect.
/// </summary>
public enum DistortionType
{
    /// <summary>
    /// Hard, abrupt clipping.
    /// </summary>
    DigitalClip,
    /// <summary>
    /// Smooth, rounded saturation.
    /// </summary>
    SoftSaturation,
    /// <summary>
    /// Softer, more gradual compression than hard clipping.
    /// </summary>
    WarmSaturation,
    /// <summary>
    /// Stronger central gain, rounded saturation.
    /// </summary>
    Overdrive,
    /// <summary>
    /// Smooth nonlinear shaping, eventually folds.
    /// </summary>
    SineShaper,
    /// <summary>
    /// Reaches +/- 1.0f cleanly, then folds when overdriven.
    /// </summary>
    SineFold,
    /// <summary>
    /// Soft at first, then folds with enough drive.
    /// </summary>
    CubicFold,
}