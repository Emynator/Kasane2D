using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Types;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class SpriteSurface : MonoGameSurface, ISpriteLayer
{
    private readonly string systemKey;
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly RenderTarget2D surface;
    private readonly RenderTarget2D viewportSurface;
    private readonly Rectangle clipRect;

    public SpriteSurface
        (
        string name,
        GraphicsDevice device,
        SpriteBatch spriteBatch,
        Vec2I surfaceSize,
        Vec2I viewportSize,
        Vec2I spriteSize,
        int count
        ) : base(surfaceSize, viewportSize)
    {
        systemKey = $"Backend::GraphicsSystem::SpriteLayer::{name}::Rasterize";
        this.device = device;
        this.spriteBatch = spriteBatch;
        SpriteSize = spriteSize;

        surface = new(device, surfaceSize.X, surfaceSize.Y);
        viewportSurface = new(device, viewportSize.X, viewportSize.Y);
        Sprites = new RenderSprite[count];
        clipRect = new Rectangle(spriteSize.X, spriteSize.Y, viewportSize.X, viewportSize.Y);

        for (var i = 0; i < count; i++)
        {
            Sprites[i] = new(spriteSize);
        }
    }

    public Vec2I SpriteSize { get; }

    public RenderSprite[] Sprites { get; }

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
        Engine.Monitor.StartMeasurement(systemKey);
        
        device.SetRenderTarget(surface);
        device.Clear(Color.Transparent);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var sprite in Sprites)
        {
            if (!sprite.IsActive)
            {
                continue;
            }

            if (
                sprite.Position.X < -SpriteSize.X
                || sprite.Position.X > Viewport.Size.X + SpriteSize.X
                || sprite.Position.Y < -SpriteSize.Y
                || sprite.Position.Y > Viewport.Size.Y + SpriteSize.Y
                )
            {
                continue;
            }

            if (sprite.SpriteAtlas is not SpriteAtlas atlas)
            {
                continue;
            }

            var src = atlas.GetSrcRect(sprite.AtlasIndex);
            var spritePos = sprite.Position + SpriteSize;
            var dst = new Rectangle(spritePos.ToPoint(), SpriteSize.ToPoint());

            var effects = SpriteEffects.None;
            if (sprite.HFlip)
            {
                effects |= SpriteEffects.FlipHorizontally;
            }
            if (sprite.VFlip)
            {
                effects |= SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw
            (
                atlas.MonoGameTexture.Texture,
                dst,
                src,
                Color.White,
                0.0f,
                Vector2.Zero,
                effects,
                1.0f
            );
        }
        spriteBatch.End();

        device.SetRenderTarget(viewportSurface);
        device.Clear(Color.Transparent);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(surface, Viewport.ViewRect.ToRectangle(), clipRect, Color.White);
        spriteBatch.End();
        
        Engine.Monitor.FinishMeasurement(systemKey);
    }
}