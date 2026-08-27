using Kasane2D.Config;
using Kasane2D.Enums;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Exceptions;
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
    private const string systemKey = "Backend::GraphicsSystem::Rasterizer::Rasterize";

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

    private bool isDrawing = false;

    public Rasterizer(GraphicsConfiguration config, GraphicsDevice device)
    {
        this.config = config;
        this.device = device;

        spriteBatch = new(device);
        textureManager = new(config, device);
        backBuffer = new(device, config.ViewportSize.X, config.ViewportSize.Y);
        backBufferRect = new(0, 0, config.ViewportSize.X, config.ViewportSize.Y);
        pixel = new Texture2D(device, 1, 1);
        pixel.SetData([MgColor.White]);

        int width;
        int height;
        if (config.ScreenSize < config.ViewportSize)
        {
            width = config.ViewportSize.X;
            height = config.ViewportSize.Y;
        }
        else
        {
            var facX = config.ScreenSize.X / config.ViewportSize.X;
            var facY = config.ScreenSize.Y / config.ViewportSize.Y;
            var fac = Math.Min(facX, facY);

            width = config.ViewportSize.X * fac;
            height = config.ViewportSize.Y * fac;
        }

        upscaleBuffer = new(device, width, height);
        upscaleRect = new(0, 0, width, height);

        var gcd = Gcd(config.ViewportSize.X, config.ViewportSize.Y);
        var viewportRatio = config.ViewportSize / gcd;
        gcd = Gcd(config.ScreenSize.X, config.ScreenSize.Y);
        var screenRatio = config.ScreenSize / gcd;

        if (
            screenRatio == viewportRatio
            || config.AspectRatioScalingMode == AspectRatioScalingMode.Stretch
            )
        {
            deviceRect = new(0, 0, config.ScreenSize.X, config.ScreenSize.Y);
            return;
        }

        if (config.ScreenSize.X < config.ViewportSize.X || config.ScreenSize.Y < config.ViewportSize.Y)
        {
            if (screenRatio.X < screenRatio.Y)
            {
                var fac = (float)config.ViewportSize.X / config.ScreenSize.X;
                var actualWidth = (int)(width / fac);
                var screenHeight = (int)(height / fac);
                deviceRect = new(0, (config.ScreenSize.Y - screenHeight) / 2, actualWidth, screenHeight);

                return;
            }

            deviceRect = new(0, 0, config.ScreenSize.X, config.ScreenSize.Y);
            return;
        }

        if (screenRatio.X < screenRatio.Y)
        {
            var fac = (float)config.ViewportSize.X / config.ScreenSize.X;
            var screenHeight = (int)MathF.Round(height * fac);
            deviceRect = new(0, (config.ScreenSize.Y - screenHeight) / 2, config.ScreenSize.X, screenHeight);

            return;
        }

        var scaleFacX = (float)config.ScreenSize.Y / height;
        var scaleFacY = (float)config.ScreenSize.X / width;
        var neededWidth = (int)MathF.Round(width * scaleFacX);
        var neededHeight = (int)MathF.Round(height * scaleFacY);
        if (neededWidth > config.ScreenSize.X)
        {
            deviceRect = new(0, (config.ScreenSize.Y - neededHeight) / 2, config.ScreenSize.X, neededHeight);
            return;
        }
        
        deviceRect = new((config.ScreenSize.X - neededWidth) / 2, 0, neededWidth, config.ScreenSize.Y);
    }

    public ITextureManager TextureManager => textureManager;

    public KasaneColor ClearColor { get; set; } = KasaneColor.Black;

    public void Dispose()
    {
    }

    public ITilemapSurface CreateTilemapSurface(string name, Vec2I tileSize, Vec2I dimensions)
    {
        var result = new TilemapSurface
        (
            name,
            device,
            spriteBatch,
            dimensions,
            tileSize,
            config.ViewportSize
        );

        surfaces.Add(result);

        return result;
    }

    public ITextureSurface CreateTextureSurface(string name, Vec2I dimensions)
    {
        var result = new TextureSurface(name, device, spriteBatch, dimensions, config.ViewportSize);
        surfaces.Add(result);

        return result;
    }

    public ISpriteLayer CreateSpriteLayer(string name, Vec2I spriteSize, int spriteCount)
    {
        var result = new SpriteSurface
        (
            name,
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
        if (isDrawing)
        {
            throw new DrawStillInProgressException();
        }

        Engine.Monitor.StartMeasurement(systemKey);

        foreach (var surface in surfaces)
        {
            surface.Rasterize();
        }

        device.SetRenderTarget(backBuffer);
        device.Clear(MgColor.Transparent);

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
        device.Clear(ClearColor.ToMgColor());

        spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        spriteBatch.Draw(upscaleBuffer, deviceRect, upscaleRect, MgColor.White);
        spriteBatch.End();

        Engine.Monitor.FinishMeasurement(systemKey);
    }

    public void BeginDraw(ITextureSurface target)
    {
        var actual = target.AsTextureSurface();
        device.SetRenderTarget(actual.RenderTarget);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        isDrawing = true;
    }

    public void EndDraw()
    {
        spriteBatch.End();
        isDrawing = false;
    }

    public void Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

        var tex = src.AsTexture();
        var destRect = dstRect?.ToRectangle() ?? new(Point.Zero, tex.Size.ToPoint());
        var sourceRect = srcRect?.ToRectangle();

        spriteBatch.Draw(tex.Texture, destRect, sourceRect, MgColor.White);
    }

    public void Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

        var surface = src.AsSurface();
        var destRect = dstRect?.ToRectangle() ?? new(Point.Zero, surface.SurfaceSize.ToPoint());
        var sourceRect = srcRect?.ToRectangle();

        spriteBatch.Draw(surface.GetSurface(), destRect, sourceRect, MgColor.White);
    }

    public void Draw(Rect rect, KasaneColor color)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

        spriteBatch.Draw(pixel, rect.ToRectangle(), null, color.ToMgColor());
    }

    public void Draw(Line line, int thickness, KasaneColor color)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

        RenderLine(line.Start.ToVector2(), line.End.ToVector2(), thickness, color.ToMgColor());
    }

    public void Draw(Bezier bezier, int thickness, KasaneColor color, int precision)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

        var prev = bezier.Start;
        for (var i = 0; i < precision; i++)
        {
            var t = (i + 1.0f) / precision;
            var next = bezier.Interpolate(t);

            RenderLine(prev.ToVector2(), next.ToVector2(), thickness, color.ToMgColor());
            prev = next;
        }
    }

    private static int Gcd(int a, int b)
    {
        while (true)
        {
            if (b == 0)
            {
                return a;
            }

            var a1 = a;
            a = b;
            b = a1 % b;
        }
    }

    private void RenderLine(Vector2 start, Vector2 end, int thickness, MgColor color)
    {
        if (!isDrawing)
        {
            throw new DrawingNotStartedException();
        }

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