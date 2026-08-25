using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Configs;

/// <summary>
/// Configuration of a single track of a synthesizer engine.
/// </summary>
public class TrackConfig
{
    /// <summary>
    /// The name of the track.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// The type of generator used for this track.
    /// </summary>
    public required GeneratorKind Kind { get; set; }
    
    /// <summary>
    /// Optional: Factory function to create a custom generator.
    /// </summary>
    /// <remarks>This is only used in case Kind is <see cref="GeneratorKind.Custom"/>.</remarks>
    public Func<int, Generator>? CustomGeneratorFactory { get; set; }
    
    /// <summary>
    /// Optional: List of effects available to this track for direct control from the sequencer.
    /// </summary>
    public List<EffectConfig> Effects { get; set; } = [];
}