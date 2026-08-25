namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for limiter parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Drive">Optional: changes the drive value of the limiter.</param>
/// <param name="Attack">Optional: changes the attack value of the limiter.</param>
/// <param name="Release">Optional: changes the release value of the limiter.</param>
/// <param name="Ceiling">Optional: changes the ceiling value of the limiter.</param>
/// <param name="Gain">Optional: changes the gain value of the limiter.</param>
public sealed record class LimiterUpdate
    (
    string EffectName,
    bool? Bypass = null,
    float? Drive = null,
    float? Attack = null,
    float? Release = null,
    float? Ceiling = null,
    float? Gain = null
    ) : EffectUpdate(EffectName, Bypass);