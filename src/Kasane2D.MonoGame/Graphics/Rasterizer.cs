using Kasane2D.Config;
using Kasane2D.Graphics;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Primitives;
using Kasane2D.MonoGame.Graphics.RenderObjects;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics;

public class Rasterizer : IRasterizer, IDisposable
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

    public Rasterizer(GraphicsConfiguration config, GraphicsDevice device)
    {
        this.config = config;
        this.device = device;

        spriteBatch = new(device);
        textureManager = new(device);
        backBuffer = new(device, config.SurfaceSize.X, config.SurfaceSize.Y);
        
        var facX = config.ScreenSize.X / config.SurfaceSize.X;
        var facY = config.ScreenSize.Y / config.SurfaceSize.Y;
        var fac = Math.Min(facX, facY);
        var width = config.SurfaceSize.X * fac;
        var height = config.SurfaceSize.Y * fac;
        
        upscaleBuffer = new(device, width, height);
        backBufferRect = new(0, 0, config.SurfaceSize.X, config.SurfaceSize.Y);
        upscaleRect = new(0, 0, width, height);
        deviceRect = new(0, 0, config.ScreenSize.X, config.ScreenSize.Y);
    }

    public ITextureManager TextureManager => textureManager;

    public void Dispose()
    {
    }

    public ISurface CreateSurface()
    {
        var result = new MonoGameSurface(device, config.SurfaceSize, config.ViewportSize);
        surfaces.Add(result);

        return result;
    }

    public ISurface CreateSurface(Vec2I dimensions)
    {
        var result = new MonoGameSurface(device, dimensions, config.ViewportSize);
        surfaces.Add(result);

        return result;
    }

    public ITilemapSurface CreateTilemapSurface()
    {
        var result = new TilemapSurface
        (
            device,
            spriteBatch,
            config.TilemapDimensions,
            config.TileSize,
            config.ViewportSize
        );

        surfaces.Add(result);

        return result;
    }

    public ITilemapSurface CreateTilemapSurface(Vec2I tileSize)
    {
        var result = new TilemapSurface
        (
            device,
            spriteBatch,
            config.TilemapDimensions,
            tileSize,
            config.ViewportSize
        );

        surfaces.Add(result);

        return result;
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

    public ISpriteLayer CreateSpriteLayer(int spriteCount)
    {
        var result = new SpriteSurface
        (
            device,
            spriteBatch,
            config.ViewportSize + config.DefaultSpriteSize * 2,
            config.ViewportSize,
            config.DefaultSpriteSize,
            spriteCount
        );
        
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
        device.Clear(Color.Transparent);
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var surface in surfaces)
        {
            spriteBatch.Draw(surface.GetSurface(), Vector2.Zero, Color.White);
        }
        spriteBatch.End();
        
        device.SetRenderTarget(upscaleBuffer);
        device.Clear(Color.Transparent);
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(backBuffer, upscaleRect, backBufferRect, Color.White);
        spriteBatch.End();
        
        device.SetRenderTarget(null);
        device.Clear(Color.Black);
        
        spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        spriteBatch.Draw(upscaleBuffer, deviceRect, upscaleRect, Color.White);
        spriteBatch.End();
    }
}