using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for delay parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Delay">Optional: changes the delay length of the delay.</param>
/// <param name="DecayGain">Optional: changes the decay gain value of the delay.</param>
/// <param name="Feedback">Optional: changes the feedback amount of the delay.</param>
/// <param name="Wet">Optional: changes the wet value of the delay.</param>
public sealed record class DelayUpdate
    (
    string EffectName,
    bool? Bypass = null,
    DelayLength? Delay = null,
    float? DecayGain = null,
    float? Feedback = null,
    float? Wet = null
    ) : EffectUpdate(EffectName, Bypass);