namespace Kasane2D.Types;

/// <summary>
/// Represents a rectangle.
/// </summary>
public readonly record struct Rect
{
    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    /// <param name="position">The top-left coordinates of the rectangle.</param>
    /// <param name="size">The width and height of the rectangle.</param>
    public Rect(Vec2I position, Vec2I size)
    {
        Position = position;
        Size = size;
    }

    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    /// <param name="x">The top-left X-coordinate of the rectangle.</param>
    /// <param name="y">The top-left Y-coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    public Rect(int x, int y, int width, int height)
    {
        Position = new Vec2I(x, y);
        Size = new Vec2I(width, height);
    }
    
    /// <summary>
    /// Gets the position of the rectangle.
    /// </summary>
    public Vec2I Position { get; }
    
    /// <summary>
    /// Gets the width and height of the rectangle.
    /// </summary>
    public Vec2I Size { get; }
    
    /// <summary>
    /// Gets the X-coordinate of the rectangle.
    /// </summary>
    public int X => Position.X;
    
    /// <summary>
    /// Gets the Y-coordinate of the rectangle.
    /// </summary>
    public int Y => Position.Y;
    
    /// <summary>
    /// Gets the width of the rectangle.
    /// </summary>
    public int Width => Size.X;
    
    /// <summary>
    /// Gets the height of the rectangle.
    /// </summary>
    public int Height => Size.Y;

    /// <summary>
    /// Gets the top-left corner of the rectangle.
    /// </summary>
    public Vec2I TopLeft => Position;
    
    /// <summary>
    /// Gets the bottom-right corner of the rectangle.
    /// </summary>
    public Vec2I BottomRight => Position + Size;

    /// <summary>
    /// Checks if the rectangle intersects with the other rectangle.
    /// </summary>
    /// <param name="other">The other rectangle to check.</param>
    /// <returns>True if the rectangles intersect, false if not.</returns>
    public bool Intersects(Rect other)
    {
        var tl = TopLeft;
        var br = BottomRight;
        var otherTl = other.TopLeft;
        var otherBr = other.BottomRight;

        return (otherTl.X <= br.X || otherTl.Y <= br.Y) && (otherBr.X >= tl.X || otherBr.Y >= tl.Y);
    }
}