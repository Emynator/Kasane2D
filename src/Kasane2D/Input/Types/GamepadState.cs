using Kasane2D.Input.Enums;
using Kasane2D.Types;

namespace Kasane2D.Input.Types;

/// <summary>
/// Current state of a gamepad.
/// </summary>
public struct GamepadState
{
    /// <summary>
    /// Creates an empty gamepad state.
    /// </summary>
    public GamepadState()
    {
    }

    /// <summary>
    /// Index of the gamepad.
    /// </summary>
    public int Index { get; set; } = 0;
    
    /// <summary>
    /// True if the gamepad is currently connected.
    /// </summary>
    public bool IsConnected { get; set; } = false;

    /// <summary>
    /// Vector representing the current position of the left thumb stick.
    /// </summary>
    /// <remarks>Position values are in the range of 0.0f to 1.0f.</remarks>
    public Vec2F LeftStick { get; set; } = Vec2F.Zero;
    
    /// <summary>
    /// Vector representing the current position of the right thumb stick.
    /// </summary>
    /// <remarks>Position values are in the range of 0.0f to 1.0f.</remarks>
    public Vec2F RightStick { get; set; }  = Vec2F.Zero;

    /// <summary>
    /// Current position of the left trigger.
    /// </summary>
    /// <remarks>Value is in the range of 0.0f to 1.0f.</remarks>
    public float LeftTrigger { get; set; } = 0.0f;
    
    /// <summary>
    /// Current position of the right trigger.
    /// </summary>
    /// <remarks>Value is in the range of 0.0f to 1.0f.</remarks>
    public float RightTrigger { get; set; } = 0.0f;

    /// <summary>
    /// Button state of DPad Up in this tick.
    /// </summary>
    public InputButtonState DPadUp { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of DPad Down in this tick.
    /// </summary>
    public InputButtonState DPadDown { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of DPad Left in this tick.
    /// </summary>
    public InputButtonState DPadLeft { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of DPad Right in this tick.
    /// </summary>
    public InputButtonState DPadRight { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the A button in this tick.
    /// </summary>
    public InputButtonState A { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the B button in this tick.
    /// </summary>
    public InputButtonState B { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the X button in this tick.
    /// </summary>
    public InputButtonState X { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the Y button in this tick.
    /// </summary>
    public InputButtonState Y { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the left shoulder button in this tick.
    /// </summary>
    public InputButtonState LB { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the right shoulder button in this tick.
    /// </summary>
    public InputButtonState RB { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the left stick button in this tick.
    /// </summary>
    public InputButtonState LS { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the right stick button in this tick.
    /// </summary>
    public InputButtonState RS { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the start button in this tick.
    /// </summary>
    public InputButtonState Start { get; set; } = InputButtonState.Released;
    
    /// <summary>
    /// Button state of the back button in this tick.
    /// </summary>
    public InputButtonState Back { get; set; } = InputButtonState.Released;
}