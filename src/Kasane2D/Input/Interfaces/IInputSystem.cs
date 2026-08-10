using Kasane2D.Input.Enums;
using Kasane2D.Input.Types;

namespace Kasane2D.Input.Interfaces;

/// <summary>
/// Primary interface to the input system.
/// </summary>
public interface IInputSystem
{
    /// <summary>
    /// Gets the current state of the mouse.
    /// </summary>
    public MouseState MouseState { get; }
    
    /// <summary>
    /// Gets the current state of a keyboard key.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>The state of the key.</returns>
    public InputButtonState Check(KeyKind key);
    
    /// <summary>
    /// Checks if a keyboard key is currently pressed.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key is pressed or just pressed, false otherwise.</returns>
    public bool IsKeyDown(KeyKind key);

    /// <summary>
    /// Gets the current state of a gamepad.
    /// </summary>
    /// <param name="index">The gamepad index whose state to get.</param>
    /// <returns>The state of the gamepad.</returns>
    public GamepadState GetGamepadState(int index);

    /// <summary>
    /// Gets the states of all currently connected gamepads.
    /// </summary>
    /// <returns>The states of all connected gamepads.</returns>
    public GamepadState[] GetConnectedGamepadStates();
}