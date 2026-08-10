using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// Represents a single sprite sheet texture that can be used as a sprite atlas for sprites or a tile atlas for tiles.
/// </summary>
public interface ISpriteAtlas
{
    /// <summary>
    /// The number of rows and columns in the atlas.
    /// </summary>
    /// <remarks>Total width of the texture is Dimensions.X * SpriteSize.X.
    /// The total height of the texture is Dimensions.Y * SpriteSize.Y.</remarks>
    public Vec2I Dimensions { get; }
    
    /// <summary>
    /// The width and height of a single sprite in the atlas.
    /// </summary>
    /// <remarks>Total width of the texture is Dimensions.X * SpriteSize.X.
    /// The total height of the texture is Dimensions.Y * SpriteSize.Y.</remarks>
    public Vec2I SpriteSize { get; }

    /// <summary>
    /// The underlying texture of the atlas.
    /// </summary>
    public ITexture Texture { get; }
}