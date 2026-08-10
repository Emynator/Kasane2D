using Kasane2D.Input.Enums;
using Kasane2D.Types;

namespace Kasane2D.Input.Types;

/// <summary>
/// Current state of the mouse.
/// </summary>
public struct MouseState
{
    public MouseState()
    {
    }
    
    /// <summary>
    /// Current position of the mouse pointer on the screen in this tick.
    /// </summary>
    public Vec2I Position { get; set; } = Vec2I.Zero;
    
    /// <summary>
    /// Current position of the scroll wheel in this tick.
    /// </summary>
    public Vec2I MouseWheel { get; set; } = Vec2I.Zero;

    /// <summary>
    /// Button state of the left mouse button in this tick.
    /// </summary>
    public InputButtonState LeftClick { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the middle mouse button in this tick.
    /// </summary>
    public InputButtonState MiddleClick { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the right mouse button in this tick.
    /// </summary>
    public InputButtonState RightClick { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the first side mouse button in this tick.
    /// </summary>
    public InputButtonState Button4 { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the second side mouse button in this tick.
    /// </summary>
    public InputButtonState Button5 { get; set; } = InputButtonState.Released;
}