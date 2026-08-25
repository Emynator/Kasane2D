using Kasane2D.Sound.Enums;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for filter parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Type">Optional: changes the type of the filter.</param>
/// <param name="Slope">Optional: changes the slope of the filter.</param>
/// <param name="CutoffFrequency">Optional: changes the cutoff frequency of the filter.</param>
/// <param name="Resonance">Optional: changes the resonance of the filter.</param>
public sealed record class FilterUpdate
    (
    string EffectName,
    bool? Bypass = null,
    FilterType? Type = null,
    int? Slope = null,
    float? CutoffFrequency = null,
    float? Resonance = null
    ) : EffectUpdate(EffectName, Bypass);