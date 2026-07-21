using Kasane2D.Primitives;

namespace Kasane2D.Graphics.Primitives;

public record struct Viewport
{
    public Viewport(Vec2I size)
    {
        Size = size;
    }
    
    public Vec2I Size { get; }

    public Vec2I Position { get; set; } = Vec2I.Zero;
    
    public Rect ViewRect => new(Position, Size);
}