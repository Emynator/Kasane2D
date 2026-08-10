using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// A rendering layer with a scrollable viewport that renders something to the screen.
/// </summary>
public interface ISurface
{
    /// <summary>
    /// Total width and height of the surface in pixels. 
    /// </summary>
    public Vec2I SurfaceSize { get; }

    /// <summary>
    /// The Viewport attached to this surface.
    /// </summary>
    public Viewport Viewport { get; }

    /// <summary>
    /// Scrolls the viewport from its current position by the given vector.
    /// </summary>
    /// <param name="value">The vector to scroll by.</param>
    /// <remarks>Scrolling wraps around on both axis. E.g. if the X-position exceeds the surface width it wraps around
    /// to 0 and continues scrolling from there.</remarks>
    public void ScrollBy(Vec2I value);

    /// <summary>
    /// Scrolls the viewport from its current position by the given vector.
    /// </summary>
    /// <param name="value">The vector to scroll by.</param>
    /// <remarks>Scrolling wraps around on both axis. E.g. if the X-position exceeds the surface width it wraps around
    /// to 0 and continues scrolling from there.
    /// Scrolling is only done in whole pixels. The float values are always rounded down.</remarks>
    public void ScrollBy(Vec2F value);

    /// <summary>
    /// Scrolls the viewport to the absolute position of the given vector.
    /// </summary>
    /// <param name="value">The position to scroll to.</param>
    /// <remarks>If the X and Y coordinates are outside the surface, they wrap around. So the actual position scrolled
    /// to is Abs(value.X) % SurfaceSize.X and Abs(value.Y) % SurfaceSize.Y.</remarks>
    public void ScrollTo(Vec2I value);

    /// <summary>
    /// Scrolls the viewport to the absolute position of the given vector.
    /// </summary>
    /// <param name="value">The position to scroll to.</param>
    /// <remarks>If the X and Y coordinates are outside the surface, they wrap around. So the actual position scrolled
    /// to is Abs(value.X) % SurfaceSize.X and Abs(value.Y) % SurfaceSize.Y.
    /// Scrolling is only done in whole pixels. The float values are always rounded down.</remarks>
    public void ScrollTo(Vec2F value);
}