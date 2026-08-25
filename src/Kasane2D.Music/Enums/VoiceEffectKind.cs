using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Enums;

/// <summary>
/// Type of voice effect to add to a synthesizer track.
/// </summary>
public enum VoiceEffectKind
{
    /// <summary>
    /// No effect.
    /// </summary>
    None,
    /// <summary>
    /// Custom effect.
    /// </summary>
    Custom,
    /// <summary>
    /// <see cref="KasaneUtility"/>
    /// </summary>
    Utility,
    /// <summary>
    /// <see cref="KasaneFilter"/>
    /// </summary>
    Filter,
    /// <summary>
    /// <see cref="KasaneEq8"/>
    /// </summary>
    Eq8,
    /// <summary>
    /// <see cref="KasaneCompressor"/>
    /// </summary>
    Compressor,
    /// <summary>
    /// <see cref="KasaneLimiter"/>
    /// </summary>
    Limiter,
    /// <summary>
    /// <see cref="KasaneOverdrive"/>
    /// </summary>
    Overdrive,
    /// <summary>
    /// <see cref="KasaneDelay"/>
    /// </summary>
    Delay,
    /// <summary>
    /// <see cref="KasanePingPongDelay"/>
    /// </summary>
    PingPongDelay,
}