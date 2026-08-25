using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

namespace Kasane2D.Music.Synthesis.Effects;

/// <summary>
/// Abstract base class for voice effects applied to a track.
/// </summary>
public abstract class VoiceEffect
{
    /// <summary>
    /// Determines if the effect will be applied or bypassed.
    /// </summary>
    public abstract bool Bypass { get; }
    
    /// <summary>
    /// Applies a control update event to update the effect parameters.
    /// </summary>
    /// <param name="ev">The update event.</param>
    public abstract void ControlUpdate(EffectUpdate ev);
    
    /// <summary>
    /// Called to apply the audio effect.
    /// </summary>
    /// <param name="inLeft">Input buffer containing the samples of the left channel.</param>
    /// <param name="inRight">Input buffer containing the samples of the right channel.</param>
    /// <param name="outLeft">Output buffer of the left channel where the resulting samples will be written to.</param>
    /// <param name="outRight">Output buffer of the right channel where the resulting samples will be written to.</param>
    public abstract void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        );
}