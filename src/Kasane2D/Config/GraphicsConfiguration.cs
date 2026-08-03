using Kasane2D.Types;

namespace Kasane2D.Config;

public class GraphicsConfiguration
{
    public Vec2I DefaultTileSize { get; set; } = Vec2I.Zero;

    public Vec2I DefaultTilemapDimensions { get; set; } = Vec2I.Zero;
    
    public Vec2I DefaultSpriteSize { get; set; } = Vec2I.Zero;

    public Vec2I DefaultSurfaceSize { get; set; } = Vec2I.Zero;
    
    public int DefaultSpriteCount { get; set; } = 64;

    public Vec2I ViewportSize { get; set; } = Vec2I.Zero;
    
    public Vec2I ScreenSize { get; set; } = Vec2I.Zero;
}