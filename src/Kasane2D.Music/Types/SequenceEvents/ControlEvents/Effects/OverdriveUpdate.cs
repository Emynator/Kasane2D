using Kasane2D.Sound.Enums;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for overdrive parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Drive">Optional: changes the drive value of the overdrive.</param>
/// <param name="Type">Optional: changes the distortion type of the overdrive.</param>
/// <param name="Wet">Optional: changes the wet value of the overdrive.</param>
public sealed record class OverdriveUpdate
    (
    string EffectName,
    bool? Bypass = null,
    float? Drive = null,
    DistortionType? Type = null,
    float? Wet = null
    ) : EffectUpdate(EffectName, Bypass);