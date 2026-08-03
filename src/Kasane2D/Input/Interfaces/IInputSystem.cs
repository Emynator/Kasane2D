using Kasane2D.Input.Enums;
using Kasane2D.Input.Types;

namespace Kasane2D.Input.Interfaces;

public interface IInputSystem
{
    public MouseState MouseState { get; }
    
    public InputButtonState Check(KeyKind key);
    
    public bool IsKeyDown(KeyKind key);

    public GamepadState GetGamepadState(int index);

    public GamepadState[] GetConnectedGamepadStates();
}