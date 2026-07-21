using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.Graphics.Primitives;

public struct Sprite
{
    public Sprite(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }

    public Vec2I Position { get; set; } = Vec2I.Zero;

    public Rect Rect => new(Position, Size);
    
    public ISpriteAtlas? SpriteAtlas { get; set; } = null;
    
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;

    public bool HFlip { get; set; } = false;

    public bool VFlip { get; set; } = false;
}