using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music;

public class SynthConfig
{
    public required string Name { get; set; }
    
    public List<TrackConfig> TrackConfigs { get; set; } = [];
}

public class TrackConfig
{
    public required string Name { get; set; }
    
    public required GeneratorKind Kind { get; set; }
    
    public Func<int, Generator>? CustomGeneratorFactory { get; set; }
}