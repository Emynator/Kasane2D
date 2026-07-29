using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

public struct RenderSprite
{
    public RenderSprite(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }

    public Vec2I Position { get; set; } = Vec2I.Zero;

    public Rect Rect => new(Position, Size);
    
    public ISpriteAtlas? SpriteAtlas { get; set; } = null;
    
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;
    
    public bool IsActive { get; set; } = false;

    public bool HFlip { get; set; } = false;

    public bool VFlip { get; set; } = false;
}