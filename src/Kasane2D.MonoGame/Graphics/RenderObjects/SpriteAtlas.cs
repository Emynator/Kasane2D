using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Primitives;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class SpriteAtlas : ISpriteAtlas
{
    public SpriteAtlas(Vec2I dimensions, Vec2I spriteSize, MonoGameTexture texture)
    {
        Dimensions = dimensions;
        SpriteSize = spriteSize;
        MonoGameTexture = texture;
    }

    public Vec2I Dimensions { get; }
    
    public Vec2I SpriteSize { get; }

    public ITexture Texture => MonoGameTexture;
    
    public MonoGameTexture MonoGameTexture { get; }

    public Rectangle GetSrcRect(Vec2I index)
    {
        if (index.X < 0 || index.Y < 0 || index.X >= Dimensions.X || index.Y >= Dimensions.Y)
        {
            throw new IndexOutOfRangeException();
        }
        
        return new(index.CompWiseMul(SpriteSize).ToPoint(), SpriteSize.ToPoint());
    }
}