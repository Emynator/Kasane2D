using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ISurface
{
    public Vec2I SurfaceSize { get; }
    
    public Viewport Viewport { get; }

    public void Scroll(Vec2I value);

    public void Scroll(Vec2F value);
}