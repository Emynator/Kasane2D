using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

public struct RenderTile
{
    public RenderTile(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }
    
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;

    public bool HFlip { get; set; } = false;

    public bool VFlip { get; set; } = false;
}