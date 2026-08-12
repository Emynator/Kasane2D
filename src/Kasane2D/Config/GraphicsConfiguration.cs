using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Config;

/// <summary>
/// Configuration of the graphics system.
/// </summary>
public class GraphicsConfiguration
{
    /// <summary>
    /// Configures if the mouse is visible or not. Default is false.
    /// </summary>
    public bool IsMouseVisibible { get; set; } = false;
    
    /// <summary>
    /// Configures the default tile width and height used for <see cref="ITilemapSurface"/>s if no layer specific
    /// override is provided.
    /// </summary>
    public Vec2I DefaultTileSize { get; set; } = Vec2I.Zero;

    /// <summary>
    /// Configures the default number of tile rows and columns used for <see cref="ITilemapSurface"/>s if no layer
    /// specific override is provided.
    /// </summary>
    public Vec2I DefaultTilemapDimensions { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Configures the default sprite width and height used for <see cref="ISpriteLayer"/>s if no layer specific
    /// override is provided.
    /// </summary>
    public Vec2I DefaultSpriteSize { get; set; } = Vec2I.Zero;

    /// <summary>
    /// Configures the default width and height of all other surface types if no layer specific override is provided.
    /// </summary>
    public Vec2I DefaultSurfaceSize { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Configures the default amount of sprites used for <see cref="ISpriteLayer"/>s if no layer specific override is
    /// provided.
    /// </summary>
    public int DefaultSpriteCount { get; set; } = 64;

    /// <summary>
    /// Configures the width and height of the viewport in pixels.
    /// </summary>
    /// <remarks>The viewport size is the native pixel size the engine works with. During rendering, the viewport gets
    /// upscaled to the actual screen size in two steps. First, it uses an integer upscaling to the largest integer
    /// value that is still equal or less than the screen resolution. Only then the viewport gets scaled to the
    /// actual screen resolution. This is done to retain the intended pixel art look of the games as best as
    /// possible.</remarks>
    public Vec2I ViewportSize { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Configures the actual width and height of the screen buffer in pixels.
    /// </summary>
    /// <remarks>The viewport size is the native pixel size the engine works with. During rendering, the viewport gets
    /// upscaled to the actual screen size in two steps. First, it uses an integer upscaling to the largest integer
    /// value that is still equal or less than the screen resolution. Only then the viewport gets scaled to the
    /// actual screen resolution. This is done to retain the intended pixel art look of the games as best as
    /// possible.</remarks>
    public Vec2I ScreenSize { get; set; } = Vec2I.Zero;
}