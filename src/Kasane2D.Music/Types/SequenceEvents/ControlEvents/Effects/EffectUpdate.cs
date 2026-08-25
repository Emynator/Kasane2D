namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// Abstract base class representing the parameter update for a track's effect.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
public abstract record class EffectUpdate(string EffectName, bool? Bypass);