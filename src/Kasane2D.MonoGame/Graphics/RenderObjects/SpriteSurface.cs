using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class SpriteSurface : MonoGameSurface, ISpriteLayer
{
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly RenderTarget2D surface;
    private readonly RenderTarget2D viewportSurface;
    private readonly Rectangle clipRect;

    public SpriteSurface
        (
        GraphicsDevice device,
        SpriteBatch spriteBatch,
        Vec2I surfaceSize,
        Vec2I viewportSize,
        Vec2I spriteSize,
        int count
        ) : base(surfaceSize, viewportSize)
    {
        this.device = device;
        this.spriteBatch = spriteBatch;
        SpriteSize = spriteSize;

        surface = new(device, surfaceSize.X, surfaceSize.Y);
        viewportSurface = new(device, viewportSize.X, viewportSize.Y);
        Sprites = new Sprite[count];
        clipRect = new Rectangle(spriteSize.X, spriteSize.Y, viewportSize.X, viewportSize.Y);
        
        for (var i = 0; i < count; i++)
        {
            Sprites[i] = new(spriteSize);
        }
    }

    public Vec2I SpriteSize { get; }

    public Sprite[] Sprites { get; }

    public override void Dispose()
    {
        viewportSurface.Dispose();
        
        base.Dispose();
    }

    public override Texture2D GetSurface()
    {
        return viewportSurface;
    }

    public override void Rasterize()
    {
        device.SetRenderTarget(surface);
        device.Clear(Color.Transparent);
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var sprite in Sprites)
        {
            var atlas = sprite.SpriteAtlas as SpriteAtlas;
            if (atlas is null)
            {
                continue;
            }

            var src = atlas.GetSrcRect(sprite.AtlasIndex);
            var dst = sprite.Rect.ToRectangle();
            
            spriteBatch.Draw(atlas.MonoGameTexture.Texture, dst, src, Color.White);
        }
        spriteBatch.End();
        
        device.SetRenderTarget(viewportSurface);
        device.Clear(Color.Transparent);
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(surface, Viewport.ViewRect.ToRectangle(), clipRect, Color.White);
        spriteBatch.End();
    }
}