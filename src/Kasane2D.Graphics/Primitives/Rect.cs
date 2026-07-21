namespace Kasane2D.Graphics.Primitives;

public readonly record struct Rect
{
    public Rect(Vec2I position, Vec2I size)
    {
        Position = position;
        Size = size;
    }

    public Rect(int x, int y, int width, int height)
    {
        Position = new Vec2I(x, y);
        Size = new Vec2I(width, height);
    }
    
    public Vec2I Position { get; }
    
    public Vec2I Size { get; }
    
    public int X => Position.X;
    
    public int Y => Position.Y;
    
    public int Width => Size.X;
    
    public int Height => Size.Y;

    public Vec2I TopLeft => Position;
    
    public Vec2I BottomRight => Position + Size;

    public bool Contains(Rect other)
    {
        var tl = TopLeft;
        var br = BottomRight;
        var otherTl = other.TopLeft;
        var otherBr = other.BottomRight;

        return (otherTl.X <= br.X || otherTl.Y <= br.Y) && (otherBr.X >= tl.X || otherBr.Y >= tl.Y);
    }
}