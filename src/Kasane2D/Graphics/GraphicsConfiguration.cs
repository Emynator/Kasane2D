using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;

namespace Kasane2D.Graphics;

public class GraphicsConfiguration
{
    public Vec2I TileSize { get; set; } = Vec2I.Zero;

    public Vec2I DefaultSpriteSize { get; set; } = Vec2I.Zero;

    public Vec2I SurfaceSize { get; set; } = Vec2I.Zero;
    
    public Vec2I TilemapDimensions { get; set; } = Vec2I.Zero;

    public Vec2I ViewportSize { get; set; } = Vec2I.Zero;
    
    public Vec2I ScreenSize { get; set; } = Vec2I.Zero;
}