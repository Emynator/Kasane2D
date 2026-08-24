using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class TextureSurface : MonoGameSurface, ITextureSurface
{
    private readonly string systemKey;
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly RenderTarget2D viewportSurface;

    public TextureSurface
        (
        string name,
        GraphicsDevice device,
        SpriteBatch spriteBatch,
        Vec2I surfaceSize,
        Vec2I viewportSize
        )
        : base(surfaceSize, viewportSize)
    {
        systemKey = $"Backend::GraphicsSystem::Surface::{name}::Rasterize";
        this.device = device;
        this.spriteBatch = spriteBatch;

        RenderTarget = new RenderTarget2D(device, SurfaceSize.X, SurfaceSize.Y);
        viewportSurface = new RenderTarget2D(device, viewportSize.X, viewportSize.Y);
    }

    public RenderTarget2D RenderTarget { get; }

    public override Texture2D GetSurface()
    {
        return viewportSurface;
    }

    public override void Rasterize()
    {
        Engine.Monitor.StartMeasurement(systemKey);
        
        device.SetRenderTarget(viewportSurface);

        spriteBatch.Begin(samplerState: SamplerState.PointWrap);
        spriteBatch.Draw(RenderTarget, Viewport.ViewRect.ToRectangle(), Color.White);
        spriteBatch.End();
        
        Engine.Monitor.FinishMeasurement(systemKey);
    }
}