using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ISurface
{
    public Vec2I SurfaceSize { get; }
    
    public Viewport Viewport { get; }

    public void ScrollBy(Vec2I value);

    public void ScrollBy(Vec2F value);
    
    public void ScrollTo(Vec2I value);

    public void ScrollTo(Vec2F value);
}