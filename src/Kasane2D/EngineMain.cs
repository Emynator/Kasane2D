using Kasane2D.Graphics;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.Sound;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D;

public abstract class EngineMain
{
    public ISoundSystem? SoundSystem => InternalSoundSystem;

    internal IRasterizer? Rasterizer { get; set; }

    internal Renderer? InternalRenderer { get; set; }

    internal IEngineRunner? EngineRunner { get; set; }

    internal IInputSystem? InternalInputSystem { get; set; }

    internal SoundSystem? InternalSoundSystem { get; set; }

    protected IRenderer Renderer =>
        InternalRenderer ?? throw new InvalidOperationException("Renderer not initialized.");

    protected IInputSystem InputSystem =>
        InternalInputSystem ?? throw new InvalidOperationException("Input system not initialized.");

    public virtual void Init()
    {
    }

    public void MainTick(float dt)
    {
        Tick(dt);
    }

    public void MainDraw()
    {
        Draw();
        Rasterizer?.Rasterize();
    }

    protected virtual void Tick(float dt)
    {
    }

    protected virtual void Draw()
    {
    }

    protected void Quit()
    {
        EngineRunner?.Stop();
    }
}