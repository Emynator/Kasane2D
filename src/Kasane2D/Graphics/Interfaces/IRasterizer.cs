using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// Backend rasterizer implementation that is used by the renderer.
/// </summary>
/// <remarks>This interface represents the rasterizer implementation for the backend. The rasterizer is used internally
/// by the engine's renderer and not meant to be called from user code. For rendering from user code refer to
/// <see cref="IRenderer"/>.</remarks>
public interface IRasterizer
{
    /// <summary>
    /// Gets the texture manager implementation.
    /// </summary>
    public ITextureManager TextureManager { get; }
    
    /// <summary>
    /// Gets the screen surface with the native screen resolution.
    /// </summary>
    /// <remarks>The screen surface is the final surface that is drawn on top of the screen after every layer has been
    /// rendered and upscaled. This can be used to for example render UIs at the native screen resolution.</remarks>
    public ITextureSurface ScreenSurface { get; }
    
    /// <summary>
    /// The color used to clear the screen buffer before drawing.
    /// </summary>
    public Color ClearColor { get; set; }

    /// <summary>
    /// Creates a new tilemap surface for the provided tilesize and dimensions.
    /// </summary>
    /// <param name="name">The name of the layer.</param>
    /// <param name="tileSize">With and height of a single tile in pixels.</param>
    /// <param name="dimensions">Number of tile rows and columns the surface should contain.</param>
    /// <returns>The created tilemap surface.</returns>
    public ITilemapSurface CreateTilemapSurface(string name, Vec2I tileSize, Vec2I dimensions);

    /// <summary>
    /// Creates a new texture surface with the provided size.
    /// </summary>
    /// <param name="name">The name of the layer.</param>
    /// <param name="dimensions">Width and height of the underlying texture in pixels.</param>
    /// <returns>The created texture surface.</returns>
    public ITextureSurface CreateTextureSurface(string name, Vec2I dimensions);
    
    /// <summary>
    /// Frees a texture surface.
    /// </summary>
    /// <param name="surface">The surface to free.</param>
    public void FreeTextureSurface(ITextureSurface surface);

    /// <summary>
    /// Creates a new sprite layer with the provided spriteSize and sprite count.
    /// </summary>
    /// <param name="name">The name of the layer.</param>
    /// <param name="spriteSize">Width and height of the sprites in pixels.</param>
    /// <param name="spriteCount">Number of sprites available in the layer.</param>
    /// <returns>The created sprite layer.</returns>
    public ISpriteLayer CreateSpriteLayer(string name, Vec2I spriteSize, int spriteCount);

    /// <summary>
    /// Creates a new overlay layer with the provided child surface.
    /// </summary>
    /// <param name="name">The name of the layer.</param>
    /// <param name="childSurface">The child surface of the overlay.</param>
    /// <returns>The created overlay.</returns>
    public IOverlay CreateOverlay(string name, ISurface childSurface);

    /// <summary>
    /// Pushes a surface to the layer stack.
    /// </summary>
    /// <param name="layer">The layer to push.</param>
    public void PushLayer(ISurface layer);

    /// <summary>
    /// Pushes a sprite layer to the layer stack.
    /// </summary>
    /// <param name="layer">The layer to push.</param>
    public void PushLayer(ISpriteLayer layer);

    /// <summary>
    /// Rasterize all surfaces to their viewports and draw them bottom to top to the final screen buffer.
    /// </summary>
    /// <remarks>Surfaces are drawn from bottom most to top most. Higher surfaces draw on top of lower ones.</remarks>
    public void Rasterize();
    
    /// <summary>
    /// Backend implementation of <see cref="IRenderer.BeginDraw"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void BeginDraw(ITextureSurface target);

    /// <summary>
    /// Backend implementation of <see cref="IRenderer.EndDraw"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void EndDraw();

    /// <summary>
    /// Backend implementation of <see cref="IRenderer.Draw(ITexture, Rect?, Rect?)"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null);
    
    /// <summary>
    /// Backend implementation of <see cref="IRenderer.Draw(ISurface, Rect?, Rect?)"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null);

    /// <summary>
    /// Backend implementation of <see cref="IRenderer.Draw(Rect, Color)"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void Draw(Rect rect, Color color);
    
    /// <summary>
    /// Backend implementation of <see cref="IRenderer.Draw(Line, int, Color)"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void Draw(Line line, int thickness, Color color);

    /// <summary>
    /// Backend implementation of <see cref="IRenderer.Draw(Bezier, int, Color, int)"/>
    /// </summary>
    /// <remarks>Free-form rendering functions are assumed to be called before <see cref="Rasterize"/> composes
    /// the final image.</remarks>
    public void Draw(Bezier bezier, int thickness, Color color, int precision);
}