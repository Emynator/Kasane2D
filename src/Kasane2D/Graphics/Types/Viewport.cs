using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

/// <summary>
/// Represents the viewport of a surface.
/// </summary>
public struct Viewport
{
    /// <summary>
    /// Create a new viewport with the given width and height.
    /// </summary>
    /// <param name="size">Width and height of the viewport in pixels.</param>
    public Viewport(Vec2I size)
    {
        Size = size;
    }
    
    /// <summary>
    /// Width and height of the viewport in pixels.
    /// </summary>
    public Vec2I Size { get; }

    /// <summary>
    /// Current position of the viewport on the surface.
    /// </summary>
    /// <remarks>Position means the top-left pixel of the viewport.</remarks>
    public Vec2I Position { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Gets the rectangle representing the bounding box of the viewport.
    /// </summary>
    /// <remarks>Does not accurately represent the wrap-around behavior of surfaces. The rect can extend outside the
    /// actual surface size even though the viewport actually wraps around during rendering.</remarks>
    public Rect ViewRect => new(Position, Size);
}