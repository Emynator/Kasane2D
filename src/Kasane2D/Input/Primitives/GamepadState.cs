using Kasane2D.Input.Enums;
using Kasane2D.Primitives;

namespace Kasane2D.Input.Primitives;

public struct GamepadState
{
    public GamepadState()
    {
    }

    public int Index { get; set; } = 0;
    
    public bool IsConnected { get; set; } = false;

    public Vec2F LeftStick { get; set; } = Vec2F.Zero;
    
    public Vec2F RightStick { get; set; }  = Vec2F.Zero;

    public float LeftTrigger { get; set; } = 0.0f;
    
    public float RightTrigger { get; set; } = 0.0f;

    public InputButtonState DPadUp { get; set; } = InputButtonState.Released;
    
    public InputButtonState DPadDown { get; set; } = InputButtonState.Released;
    
    public InputButtonState DPadLeft { get; set; } = InputButtonState.Released;
    
    public InputButtonState DPadRight { get; set; } = InputButtonState.Released;
    
    public InputButtonState A { get; set; } = InputButtonState.Released;
    
    public InputButtonState B { get; set; } = InputButtonState.Released;
    
    public InputButtonState X { get; set; } = InputButtonState.Released;
    
    public InputButtonState Y { get; set; } = InputButtonState.Released;
    
    public InputButtonState LB { get; set; } = InputButtonState.Released;
    
    public InputButtonState RB { get; set; } = InputButtonState.Released;
    
    public InputButtonState LS { get; set; } = InputButtonState.Released;
    
    public InputButtonState RS { get; set; } = InputButtonState.Released;
    
    public InputButtonState Start { get; set; } = InputButtonState.Released;
    
    public InputButtonState Back { get; set; } = InputButtonState.Released;
}