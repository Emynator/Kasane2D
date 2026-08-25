using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Effects;

namespace Kasane2D.Music.Configs;

/// <summary>
/// Configuration of a track effect.
/// </summary>
public class EffectConfig
{
    /// <summary>
    /// Name of the effect.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Type of effect.
    /// </summary>
    public required VoiceEffectKind Kind { get; set; }
    
    /// <summary>
    /// Optional: Factory function to create a custom effect.
    /// </summary>
    /// <remarks>This is only used in case Kind is <see cref="VoiceEffectKind.Custom"/>.</remarks>
    public Func<int, VoiceEffect>? CustomEffectFactory { get; set; }
}