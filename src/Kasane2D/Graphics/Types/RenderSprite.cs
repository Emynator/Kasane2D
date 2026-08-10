using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

/// <summary>
/// An <see cref="ISpriteLayer"/>'s sprite.
/// </summary>
public struct RenderSprite
{
    /// <summary>
    /// Create a new sprite with the given width and height.
    /// </summary>
    /// <param name="size">Width and height of the sprite in pixels.</param>
    public RenderSprite(Vec2I size)
    {
        Size = size;
    }
    
    /// <summary>
    /// The sprite's width and height in pixels.
    /// </summary>
    public Vec2I Size { get; }

    /// <summary>
    /// The sprite's position on the screen.
    /// </summary>
    /// <remarks>The sprite layer extends one sprite width each to the left and right of the screen and one sprite
    /// height each to the top and bottom of the screen. This allows for sprites to be smoothly scrolled into the
    /// viewport.</remarks>
    public Vec2I Position { get; set; } = Vec2I.Zero;

    /// <summary>
    /// Gets the rectangle representing the bounding box of the sprite.
    /// </summary>
    public Rect Rect => new(Position, Size);
    
    /// <summary>
    /// Sprite sheet used to draw the sprite.
    /// </summary>
    public ISpriteAtlas? SpriteAtlas { get; set; } = null;
    
    /// <summary>
    /// Current index of the sprite graphics in the atlas.
    /// </summary>
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Determines if the sprite is drawn or not.
    /// </summary>
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// Determines if the sprite's graphics should be flipped horizontally when drawn.
    /// </summary>
    public bool HFlip { get; set; } = false;

    /// <summary>
    /// Determines if the sprite's graphics should be flipped vertically when drawn.
    /// </summary>
    public bool VFlip { get; set; } = false;
}