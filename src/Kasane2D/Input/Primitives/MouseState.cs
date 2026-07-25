using Kasane2D.Input.Enums;
using Kasane2D.Primitives;

namespace Kasane2D.Input.Primitives;

public struct MouseState
{
    public MouseState()
    {
    }
    
    public Vec2I Position { get; set; } = Vec2I.Zero;
    
    public Vec2I MouseWheel { get; set; } = Vec2I.Zero;

    public InputButtonState LeftClick { get; set; } = InputButtonState.Released;
    
    public InputButtonState MiddleClick { get; set; } = InputButtonState.Released;
    
    public InputButtonState RightClick { get; set; } = InputButtonState.Released;
    
    public InputButtonState Button4 { get; set; } = InputButtonState.Released;
    
    public InputButtonState Button5 { get; set; } = InputButtonState.Released;
}