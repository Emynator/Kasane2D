using Kasane2D.Config;
using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// Primary interface to the graphics system.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Gets the texture manager used by the renderer.
    /// </summary>
    public ITextureManager TextureManager { get; }
    
    /// <summary>
    /// The color used to clear the screen buffer before drawing.
    /// </summary>
    public Color ClearColor { get; set; }
    
    /// <summary>
    /// Initializes the graphics system.
    /// </summary>
    /// <param name="renderLayerConfigs">User provided configuration of rendering layers.</param>
    /// <remarks>Intended for use by backend implementations only.</remarks>
    public void Init(ICollection<RenderLayerConfig> renderLayerConfigs);

    /// <summary>
    /// Gets the named render surface.
    /// </summary>
    /// <param name="name">Name of the render surface to get.</param>
    /// <typeparam name="T">Type of the surface to get.</typeparam>
    /// <returns>The render surface with the specified name.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the surface of the specified name does not exist.</exception>
    public T GetSurface<T>(string name) where T : ISurface;
    
    /// <summary>
    /// Gets the named sprite layer.
    /// </summary>
    /// <param name="name">Name of the sprite layer to get.</param>
    /// <returns>The sprite layer with the specified name.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the sprite layer of the specified name does not exist.</exception>
    public ISpriteLayer GetSpriteLayer(string name);
    
    /// <summary>
    /// Gets a slot manager to manage <see cref="SpriteSlot"/>s of an <see cref="ISpriteLayer"/>.
    /// </summary>
    /// <param name="layerName">Name of the sprite layer whose slot manager to get.</param>
    /// <returns>Slot manager of the specified layer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the sprite layer of the specified name does not exist.</exception>
    public ISlotManager GetSlotManager(string layerName);

    /// <summary>
    /// Begin free-form rendering on a render target.
    /// </summary>
    /// <param name="target">The render target to draw to.</param>
    /// <remarks>All subsequent draw calls target the given surface until <see cref="EndDraw"/> is called.</remarks>
    public void BeginDraw(ITextureSurface target);

    /// <summary>
    /// Ends free-form rendering and submits the pending draw calls.
    /// </summary>
    public void EndDraw();

    /// <summary>
    /// Draws a texture to the render target.
    /// </summary>
    /// <param name="src">Texture to draw to the render target.</param>
    /// <param name="dstRect">Optional: destination rectangle to mark the surface area to draw to. Whole surface if null.</param>
    /// <param name="srcRect">Optional: source rectangle to mark the texture area to draw from. Whole texture if null.</param>
    public void Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null);
    
    /// <summary>
    /// Draws the contents of a surface to the render target.
    /// </summary>
    /// <param name="src">Surface to draw to the render target.</param>
    /// <param name="dstRect">Optional: destination rectangle to mark the surface area to draw to. Whole surface if null.</param>
    /// <param name="srcRect">Optional: source rectangle to mark the surface area to draw from. Whole surface if null.</param>
    public void Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null);

    /// <summary>
    /// Draws a filled rectangle to the render target.
    /// </summary>
    /// <param name="rect">Rectangle to draw.</param>
    /// <param name="color">Fill color of the rectangle.</param>
    public void Draw(Rect rect, Color color);
    
    /// <summary>
    /// Draws a line to the render target.
    /// </summary>
    /// <param name="line">Line to draw.</param>
    /// <param name="thickness">Line thickness in pixels.</param>
    /// <param name="color">Color of the line.</param>
    public void Draw(Line line, int thickness, Color color);

    /// <summary>
    /// Draws a beziér to the render target.
    /// </summary>
    /// <param name="bezier">Beziér to draw.</param>
    /// <param name="thickness">Line thickness in pixels.</param>
    /// <param name="color">Color of the beziér.</param>
    /// <param name="precision">Optional: curve precision. Number of lines to approach the beziér curve with. Default: 5.</param>
    public void Draw(Bezier bezier, int thickness, Color color, int precision = 5);
}