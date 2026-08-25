namespace Kasane2D.Music.Configs;

/// <summary>
/// Configuration of a synthesizer engine.
/// </summary>
public class SynthConfig
{
    /// <summary>
    /// Name of the synthesizer engine.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Configurations of all tracks in this synthesizer engine.
    /// </summary>
    public List<TrackConfig> TrackConfigs { get; set; } = [];
}