using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.RenderObjects;

/// <summary>
/// Abstract base class for ISurface implementations that implements common surface functionality.
/// </summary>
/// <remarks>This class is intended for backends to derive from for their various surface implementations. It should
/// not be used in user code.</remarks>
public abstract class Surface : ISurface
{
    private Viewport viewport;
    
    /// <summary>
    /// Implementation called ctor.
    /// </summary>
    /// <param name="surfaceSize">The width and height of the surface.</param>
    /// <param name="viewportSize">The width and height of the viewport.</param>
    protected Surface(Vec2I surfaceSize, Vec2I viewportSize)
    {
        SurfaceSize = surfaceSize;
        
        viewport = new(viewportSize);
    }
    
    /// <inheritdoc/>
    public Vec2I SurfaceSize { get; }
    
    /// <inheritdoc/>
    public Viewport Viewport => viewport;

    /// <inheritdoc/>
    public void ScrollBy(Vec2I value)
    {
        var newPos = viewport.Position + value;
        if (newPos.X < 0)
        {
            var x = newPos.X * -1;
            x %= SurfaceSize.X;
            newPos.X = SurfaceSize.X - x;
        }

        if (newPos.Y < 0)
        {
            var y = newPos.Y * -1;
            y %= SurfaceSize.Y;
            newPos.Y = SurfaceSize.Y - y;
        }
        
        newPos.X %= SurfaceSize.X;
        newPos.Y %= SurfaceSize.Y;
        
        viewport.Position = newPos;
    }

    /// <inheritdoc/>
    public void ScrollBy(Vec2F value)
    {
        ScrollBy(value.ToVec2I());
    }

    /// <inheritdoc/>
    public void ScrollTo(Vec2I value)
    {
        var dest = new Vec2I(value.X > 0 ? value.X : -value.X, value.Y > 0 ? value.Y : -value.Y);
        dest.X %= SurfaceSize.X;
        dest.Y %= SurfaceSize.Y;
        
        viewport.Position = dest;
    }

    /// <inheritdoc/>
    public void ScrollTo(Vec2F value)
    {
        ScrollTo(value.ToVec2I());
    }
}