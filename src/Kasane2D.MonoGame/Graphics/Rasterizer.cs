using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.MonoGame.Graphics.RenderObjects;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MgColor = Microsoft.Xna.Framework.Color;
using KasaneColor = Kasane2D.Graphics.Types.Color;

namespace Kasane2D.MonoGame.Graphics;

internal class Rasterizer : IRasterizer, IDisposable
{
    private readonly GraphicsConfiguration config;
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly TextureManager textureManager;
    private readonly List<MonoGameSurface> surfaces = [];
    private readonly RenderTarget2D backBuffer;
    private readonly RenderTarget2D upscaleBuffer;
    private readonly Rectangle backBufferRect;
    private readonly Rectangle upscaleRect;
    private readonly Rectangle deviceRect;
    private readonly Texture2D pixel;

    public Rasterizer(GraphicsConfiguration config, GraphicsDevice device)
    {
        this.config = config;
        this.device = device;

        spriteBatch = new(device);
        textureManager = new(config, device);
        backBuffer = new(device, config.ViewportSize.X, config.ViewportSize.Y);

        var facX = config.ScreenSize.X / config.ViewportSize.X;
        var facY = config.ScreenSize.Y / config.ViewportSize.Y;
        var fac = Math.Min(facX, facY);
        var width = config.ViewportSize.X * fac;
        var height = config.ViewportSize.Y * fac;

        upscaleBuffer = new(device, width, height);
        backBufferRect = new(0, 0, config.ViewportSize.X, config.ViewportSize.Y);
        upscaleRect = new(0, 0, width, height);
        deviceRect = new(0, 0, config.ScreenSize.X, config.ScreenSize.Y);
        
        pixel = new Texture2D(device, 1, 1);
        pixel.SetData([MgColor.White]);
    }

    public ITextureManager TextureManager => textureManager;

    public KasaneColor ClearColor { get; set; } = KasaneColor.Black;

    public void Dispose()
    {
    }

    public ITilemapSurface CreateTilemapSurface(Vec2I tileSize, Vec2I dimensions)
    {
        var result = new TilemapSurface
        (
            device,
            spriteBatch,
            dimensions,
            tileSize,
            config.ViewportSize
        );

        surfaces.Add(result);

        return result;
    }

    public ITextureSurface CreateTextureSurface(Vec2I dimensions)
    {
        var result = new TextureSurface(device, spriteBatch, dimensions, config.ViewportSize);
        surfaces.Add(result);
        
        return result;
    }

    public ISpriteLayer CreateSpriteLayer(Vec2I spriteSize, int spriteCount)
    {
        var result = new SpriteSurface
        (
            device,
            spriteBatch,
            config.ViewportSize + spriteSize * 2,
            config.ViewportSize,
            spriteSize,
            spriteCount
        );

        surfaces.Add(result);

        return result;
    }

    public void Rasterize()
    {
        foreach (var surface in surfaces)
        {
            surface.Rasterize();
        }

        device.SetRenderTarget(backBuffer);
        device.Clear(ClearColor.ToMgColor());

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var surface in surfaces)
        {
            var tex = surface.GetSurface();
            spriteBatch.Draw(tex, Vector2.Zero, MgColor.White);
        }
        spriteBatch.End();

        device.SetRenderTarget(upscaleBuffer);
        device.Clear(MgColor.Transparent);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(backBuffer, upscaleRect, backBufferRect, MgColor.White);
        spriteBatch.End();

        device.SetRenderTarget(null);
        device.Clear(MgColor.Black);

        spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        spriteBatch.Draw(upscaleBuffer, deviceRect, upscaleRect, MgColor.White);
        spriteBatch.End();
    }

    public void BeginDraw(ITextureSurface target)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
    }

    public void EndDraw()
    {
        spriteBatch.End();
    }

    public void Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null)
    {
        var tex = src.AsTexture();
        var destRect = dstRect?.ToRectangle() ?? new(Point.Zero, tex.Size.ToPoint());
        var sourceRect = srcRect?.ToRectangle();
        
        spriteBatch.Draw(tex.Texture, destRect, sourceRect, MgColor.White);
    }

    public void Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null)
    {
        var surface = src.AsSurface();
        var destRect = dstRect?.ToRectangle() ?? new(Point.Zero, surface.SurfaceSize.ToPoint());
        var sourceRect = srcRect?.ToRectangle();
        
        spriteBatch.Draw(surface.GetSurface(), destRect, sourceRect, MgColor.White);
    }

    public void Draw(Rect rect, KasaneColor color)
    {
        spriteBatch.Draw(pixel, rect.ToRectangle(), null, color.ToMgColor());
    }

    public void Draw(Line line, int thickness, KasaneColor color)
    {
        RenderLine(line.Start.ToVector2(), line.End.ToVector2(), thickness, color.ToMgColor());
    }

    public void Draw(Bezier bezier, int thickness, KasaneColor color, int precision)
    {
        var prev = bezier.Start;
        for (var i = 0; i < precision; i++)
        {
            var t = (i + 1.0f) / precision;
            var next = bezier.Interpolate(t);
            
            RenderLine(prev.ToVector2(), next.ToVector2(), thickness, color.ToMgColor());
            prev = next;
        }
    }
    
    private void RenderLine(Vector2 start, Vector2 end, int thickness, MgColor color)
    {
        var length = (end - start).Length();
        var rectSize = new Vector2(length, thickness);
        var rotation = MathF.Atan2(end.Y - start.Y, end.X - start.X);
        var rect = new Rectangle(start.ToPoint(), rectSize.ToPoint());

        spriteBatch.Draw
        (
            pixel,
            rect,
            null,
            color,
            rotation,
            start,
            SpriteEffects.None,
            1.0f
        );
    }
}