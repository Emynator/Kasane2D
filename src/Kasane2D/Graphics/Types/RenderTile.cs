using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

/// <summary>
/// An <see cref="ITilemapSurface"/>'s tile.
/// </summary>
public struct RenderTile
{
    /// <summary>
    /// Create a new tile with the given width and height.
    /// </summary>
    /// <param name="size">Width and height of the tile in pixels.</param>
    public RenderTile(Vec2I size)
    {
        Size = size;
    }
    
    /// <summary>
    /// The tile's width and height in pixels.
    /// </summary>
    public Vec2I Size { get; }
    
    /// <summary>
    /// Current index of the tile graphics in the atlas.
    /// </summary>
    public Vec2I AtlasIndex { get; set; } = Vec2I.Zero;

    /// <summary>
    /// Determines if the tile's graphics should be flipped horizontally when drawn.
    /// </summary>
    public bool HFlip { get; set; } = false;

    /// <summary>
    /// Determines if the tile's graphics should be flipped vertically when drawn.
    /// </summary>
    public bool VFlip { get; set; } = false;
}