using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ISpriteAtlas
{
    public Vec2I Dimensions { get; }
    
    public Vec2I SpriteSize { get; }

    public ITexture Texture { get; }
}