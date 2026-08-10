using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// A rendering layer that renders sprites to the screen.
/// </summary>
public interface ISpriteLayer
{
    /// <summary>
    /// Width and Height of a sprite in pixels.
    /// </summary>
    public Vec2I SpriteSize { get; }
    
    /// <summary>
    /// The array of sprites in this layer.
    /// </summary>
    /// <remarks>If you do not require the direct low-level control of the sprites, it is recommended to use an <see cref="ISlotManager"/> and work with sprite slots instead of directly manipulating the sprites of a layer.</remarks>
    public RenderSprite[] Sprites { get; }
}