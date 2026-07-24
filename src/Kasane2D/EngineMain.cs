using Kasane2D.Graphics;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Interfaces;

namespace Kasane2D;

public abstract class EngineMain
{
    internal IRasterizer? Rasterizer { get; set; }

    internal Renderer? InternalRenderer { get; set; }

    internal IEngineRunner? EngineRunner { get; set; }

    protected IRenderer Renderer =>
        InternalRenderer ?? throw new InvalidOperationException("Renderer not initialized.");

    public virtual void Init()
    {
    }

    public abstract void Tick(double dt);

    public virtual void Draw()
    {
        Rasterizer?.Rasterize();
    }

    protected void Quit()
    {
        EngineRunner?.Stop();
    }
}