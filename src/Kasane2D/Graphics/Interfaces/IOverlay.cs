using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// A rendering layer that is a movable surface that can also be deactivated.
/// </summary>
public interface IOverlay : ISurface
{
    /// <summary>
    /// The child surface of the overlay.
    /// </summary>
    public ISurface Surface { get; }
    
    /// <summary>
    /// If the surface is active or not.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Position of the surface on the screen.
    /// </summary>
    public Vec2I Position { get; set; }
    
    /// <summary>
    /// Size the surface is clipped to. Clamped between (1, 1) and the screen size.
    /// </summary>
    public Vec2I Size { get; set; }
}