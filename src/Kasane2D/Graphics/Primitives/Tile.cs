using Kasane2D.Primitives;

namespace Kasane2D.Graphics.Primitives;

public struct Tile
{
    public Tile(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }
    
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;

    public bool HFlip { get; set; } = false;

    public bool VFlip { get; set; } = false;
}