using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Primitives;

namespace Kasane2D.Graphics.RenderObjects;

public abstract class Surface : ISurface
{
    private Viewport viewport;
    
    protected Surface(Vec2I surfaceSize, Vec2I viewportSize)
    {
        SurfaceSize = surfaceSize;
        
        viewport = new(viewportSize);
    }
    
    public Vec2I SurfaceSize { get; }
    
    public Viewport Viewport => viewport;

    public void Scroll(Vec2I value)
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

    public void Scroll(Vec2F value)
    {
        Scroll(value.ToVec2I());
    }
}