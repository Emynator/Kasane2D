# 3 - Input System
Kasane2D's input system is a state based system. Before each call of `EngineMain.Tick()`, the engine polls the current state of all input devices and makes them accessible in the `IInputSystem` interface. The input system can be accessed simply through `EngineMain.InputSystem`.

The states of all kinds of keys and buttons is represented through the `InputButtonState` enum:
- A state of `InputButtonState.JustPressed` means that the button was not pressed in the previous update but it is pressed now.
- A state of `InputButtonState.Pressed` means that the button was pressed in the previous update and continues to be pressed.
- A state of `InputButtonState.JustReleased` means that the button was pressed in the previous update, but it is no longer pressed now.
- A state of `InputButtonState.Released` means that the button has not been pressed in the previous update and continues to not be pressed.

**Important:** Since it is a state based system, that necessarily means that the update rate of the input system is directly tied to the tick rate the engine is running with!

# Handling Keyboard Input
Keyboard input can be easily checked with two different methods. You can either use `IInputSystem.Check()` to get the current state of the button or use `IInputSystem.IsKeyDown()` if you just want to check if the button is currently pressed or not.
All keyboard keys are represented with the `KeyKind` enum.

# Handling Mouse Input
The current state of the mouse is available in `IInputSystem.MouseState`. The states of the various mouse buttons are available through the various properties. The current cursor position and mouse wheel position are represented with `Vec2I`s.

# Handling Controller Input
Controller input is handled in a similar fashion to mouse input with `GamepadState`. Unlike with keyboard and mouse, there can be multiple controllers connected at the same time (up to 8 total). You can check the states of each controller respectively by calling `IInputSystem.GetGamepadState()` with the player index ranging from 0 to 7.

Unlike keyboard and mouse, controllers might not be currently connected, at all. They might also disconnect while the game is currently running (for example because the battery in a wireless controller runs out). `IInputSystem.GetConnectedGamepadStates()` returns only the states of all the currently connected inputs.

**Important:** The properties of `GamepadState` are named in respect to the layout of an Xbox controller!