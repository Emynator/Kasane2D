using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

public struct Viewport
{
    public Viewport(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }

    public Vec2I Position { get; set; } = Vec2I.Zero;
    
    public Rect ViewRect => new(Position, Size);
}