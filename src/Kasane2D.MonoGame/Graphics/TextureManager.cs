using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Primitives;
using Kasane2D.MonoGame.Graphics.RenderObjects;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics;

public class TextureManager : ITextureManager
{
    private readonly GraphicsConfiguration config;
    private readonly GraphicsDevice device;

    public TextureManager(GraphicsConfiguration config, GraphicsDevice device)
    {
        this.config = config;
        this.device = device;
    }

    public ITexture CreateTexture(Vec2I size)
    {
        return new MonoGameTexture(new(device, size.X, size.Y));
    }

    public ITexture CreateTexture(string filePath)
    {
        return !File.Exists(filePath)
            ? throw new FileNotFoundException($"File '{filePath}' does not exist")
            : new MonoGameTexture(Texture2D.FromFile(device, filePath));
    }

    public void FreeTexture(ITexture texture)
    {
        texture.AsTexture().Texture.Dispose();
    }

    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize)
    {
        return new SpriteAtlas
        (
            dimensions,
            spriteSize,
            new
            (
                new
                (
                    device,
                    dimensions.X * spriteSize.X,
                    dimensions.Y * spriteSize.Y
                )
            )
        );
    }

    public ISpriteAtlas CreateSpriteAtlas(Vec2I spriteSize, string filePath)
    {
        var texture = !File.Exists(filePath)
            ? throw new FileNotFoundException($"File '{filePath}' does not exist")
            : new MonoGameTexture(Texture2D.FromFile(device, filePath));
        
        return new SpriteAtlas(texture.Size, spriteSize, texture);
    }

    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize, string filePath)
    {
        var texture = !File.Exists(filePath)
            ? throw new FileNotFoundException($"File '{filePath}' does not exist")
            : new MonoGameTexture(Texture2D.FromFile(device, filePath));
        
        var minX = dimensions.X * spriteSize.X;
        var minY = dimensions.Y * spriteSize.Y;
        if (texture.Size.X < minX || texture.Size.Y < minY)
        {
            throw new InvalidOperationException();
        }

        return new SpriteAtlas(dimensions, spriteSize, texture);
    }

    public void FreeSpriteAtlas(ISpriteAtlas atlas)
    {
        atlas.AsAtlas().MonoGameTexture.Texture.Dispose();
    }
}