using Kasane2D.Graphics.Primitives;
using Kasane2D.Graphics.RenderObjects;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class MonoGameSurface : Surface, IDisposable
{
    private readonly Texture2D? texture = null;
    
    protected MonoGameSurface(Vec2I surfaceSize, Vec2I viewportSize) : base(surfaceSize, viewportSize)
    {
    }

    public MonoGameSurface(GraphicsDevice device, Vec2I surfaceSize, Vec2I viewportSize) : base(surfaceSize, viewportSize)
    {
        texture = new Texture2D(device, SurfaceSize.X, SurfaceSize.Y);
    }
    
    public virtual void Dispose()
    {
        texture?.Dispose();
    }

    public virtual Texture2D GetSurface()
    {
        return texture ?? throw new InvalidOperationException("Derived class did not override GetFrameBuffer()");
    }
    
    public virtual void Rasterize()
    {
    }
}