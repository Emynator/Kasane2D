namespace Kasane2D.Input.Enums;

/// <summary>
/// Possible states of an input system button or key.
/// </summary>
public enum InputButtonState
{
    /// <summary>
    /// The button was just pressed on this tick.
    /// </summary>
    JustPressed,
    /// <summary>
    /// The button continues to be pressed on this tick.
    /// </summary>
    Pressed,
    /// <summary>
    /// The button was just released on this tick.
    /// </summary>
    JustReleased,
    /// <summary>
    /// The button continues to be released on this tick.
    /// </summary>
    Released,
}