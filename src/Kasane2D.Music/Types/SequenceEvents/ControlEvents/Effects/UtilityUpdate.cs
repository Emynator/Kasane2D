namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for utility parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Gain">Optional: changes the gain value of the utility.</param>
/// <param name="Pan">Optional: changes the pan value of the utility.</param>
public sealed record class UtilityUpdate
    (
    string EffectName,
    bool? Bypass = null,
    float? Gain = null,
    int? Pan = null
    ) : EffectUpdate(EffectName, Bypass);