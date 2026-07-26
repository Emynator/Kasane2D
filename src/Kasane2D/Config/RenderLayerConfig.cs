using Kasane2D.Types;

namespace Kasane2D.Config;

public enum LayerType
{
    Tilemap,
    Sprite,
}

public class RenderLayerConfig
{
    public required string Name { get; init; }
    
    public required LayerType Type { get; init; }

    public int Layer { get; init; } = -1;
    
    public Vec2I? TileSize { get; init; }
    
    public Vec2I? SpriteSize { get; init; }
    
    public Vec2I? Dimensions { get; init; }
    
    public int? SpriteCount { get; init; }

    internal bool Verify()
    {
        return Type switch
        {
            LayerType.Tilemap => true,
            LayerType.Sprite => SpriteCount is not null,
            _ => false,
        };
    }
}