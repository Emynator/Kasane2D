using Kasane2D.Input.Enums;
using Kasane2D.Input.Interfaces;
using Kasane2D.Input.Types;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using KasaneMouseState = Kasane2D.Input.Types.MouseState;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

namespace Kasane2D.MonoGame.Input;

public class InputSystem : IInputSystem
{
    private static readonly KeyKind[] keyKinds =
    [
        KeyKind.A,
        KeyKind.B,
        KeyKind.C,
        KeyKind.D,
        KeyKind.E,
        KeyKind.F,
        KeyKind.G,
        KeyKind.H,
        KeyKind.I,
        KeyKind.J,
        KeyKind.K,
        KeyKind.L,
        KeyKind.M,
        KeyKind.N,
        KeyKind.O,
        KeyKind.P,
        KeyKind.Q,
        KeyKind.R,
        KeyKind.S,
        KeyKind.T,
        KeyKind.U,
        KeyKind.V,
        KeyKind.W,
        KeyKind.X,
        KeyKind.Y,
        KeyKind.Z,
        KeyKind.N1,
        KeyKind.N2,
        KeyKind.N3,
        KeyKind.N4,
        KeyKind.N5,
        KeyKind.N6,
        KeyKind.N7,
        KeyKind.N8,
        KeyKind.N9,
        KeyKind.N0,
        KeyKind.Numpad1,
        KeyKind.Numpad2,
        KeyKind.Numpad3,
        KeyKind.Numpad4,
        KeyKind.Numpad5,
        KeyKind.Numpad6,
        KeyKind.Numpad7,
        KeyKind.Numpad8,
        KeyKind.Numpad9,
        KeyKind.Numpad0,
        KeyKind.F1,
        KeyKind.F2,
        KeyKind.F3,
        KeyKind.F4,
        KeyKind.F5,
        KeyKind.F6,
        KeyKind.F7,
        KeyKind.F8,
        KeyKind.F9,
        KeyKind.F10,
        KeyKind.F11,
        KeyKind.F12,
        KeyKind.Comma,
        KeyKind.Period,
        KeyKind.QuestionMark,
        KeyKind.Semicolon,
        KeyKind.Quote,
        KeyKind.BracketOpen,
        KeyKind.BracketClose,
        KeyKind.Backslash,
        KeyKind.Minus,
        KeyKind.Plus,
        KeyKind.Tab,
        KeyKind.Escape,
        KeyKind.Enter,
        KeyKind.Backspace,
        KeyKind.Delete,
        KeyKind.Space,
        KeyKind.Up,
        KeyKind.Down,
        KeyKind.Left,
        KeyKind.Right,
        KeyKind.LeftCtrl,
        KeyKind.LeftAlt,
        KeyKind.LeftShift,
        KeyKind.RightCtrl,
        KeyKind.RightAlt,
        KeyKind.RightShift,
    ];

    private readonly Dictionary<KeyKind, InputButtonState> keyboardState = new();
    private KeyboardState oldKbState = Keyboard.GetState();
    private MouseState oldMouseState = Mouse.GetState();

    private GamePadState[] oldGpStates =
    [
        GamePad.GetState(PlayerIndex.One),
        GamePad.GetState(PlayerIndex.Two),
        GamePad.GetState(PlayerIndex.Three),
        GamePad.GetState(PlayerIndex.Four),
        GamePad.GetState(PlayerIndex.Five),
        GamePad.GetState(PlayerIndex.Six),
        GamePad.GetState(PlayerIndex.Seven),
        GamePad.GetState(PlayerIndex.Eight),
    ];

    private KasaneMouseState mouseState = new();

    private readonly GamepadState[] gamepadStates =
    [
        new()
        {
            Index = 0,
        },
        new()
        {
            Index = 1,
        },
        new()
        {
            Index = 2,
        },
        new()
        {
            Index = 3,
        },
        new()
        {
            Index = 4,
        },
        new()
        {
            Index = 5,
        },
        new()
        {
            Index = 6,
        },
        new()
        {
            Index = 7,
        },
    ];

    public KasaneMouseState MouseState => mouseState;

    public InputButtonState Check(KeyKind key)
    {
        return keyboardState.GetValueOrDefault(key, InputButtonState.Released);
    }

    public bool IsKeyDown(KeyKind key)
    {
        var state = Check(key);
        
        return state is InputButtonState.Pressed or InputButtonState.JustPressed;
    }

    public GamepadState GetGamepadState(int index)
    {
        return gamepadStates[index];
    }

    public GamepadState[] GetConnectedGamepadStates()
    {
        return gamepadStates.Where(state => state.IsConnected).ToArray();
    }

    public void Update()
    {
        var newKbState = Keyboard.GetState();
        foreach (var key in keyKinds)
        {
            var k = key.ToKeys();
            if (oldKbState.IsKeyDown(k))
            {
                keyboardState[key] = newKbState.IsKeyDown(k)
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                keyboardState[key] = newKbState.IsKeyDown(k)
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
        }
        oldKbState = newKbState;

        var newMouseState = Mouse.GetState();
        mouseState.Position = newMouseState.Position.ToVec2I();
        var mouseWheel = new Vec2I(newMouseState.HorizontalScrollWheelValue, newMouseState.ScrollWheelValue);
        mouseState.MouseWheel = mouseWheel;

        if (oldMouseState.LeftButton == ButtonState.Pressed)
        {
            mouseState.LeftClick = newMouseState.LeftButton == ButtonState.Pressed
                ? InputButtonState.Pressed
                : InputButtonState.JustReleased;
        }
        else
        {
            mouseState.LeftClick = newMouseState.LeftButton == ButtonState.Pressed
                ? InputButtonState.JustPressed
                : InputButtonState.Released;
        }

        if (oldMouseState.MiddleButton == ButtonState.Pressed)
        {
            mouseState.MiddleClick = newMouseState.MiddleButton == ButtonState.Pressed
                ? InputButtonState.Pressed
                : InputButtonState.JustReleased;
        }
        else
        {
            mouseState.MiddleClick = newMouseState.MiddleButton == ButtonState.Pressed
                ? InputButtonState.JustPressed
                : InputButtonState.Released;
        }

        if (oldMouseState.RightButton == ButtonState.Pressed)
        {
            mouseState.RightClick = newMouseState.RightButton == ButtonState.Pressed
                ? InputButtonState.Pressed
                : InputButtonState.JustReleased;
        }
        else
        {
            mouseState.RightClick = newMouseState.RightButton == ButtonState.Pressed
                ? InputButtonState.JustPressed
                : InputButtonState.Released;
        }

        if (oldMouseState.XButton1 == ButtonState.Pressed)
        {
            mouseState.Button4 = newMouseState.XButton1 == ButtonState.Pressed
                ? InputButtonState.Pressed
                : InputButtonState.JustReleased;
        }
        else
        {
            mouseState.Button4 = newMouseState.XButton1 == ButtonState.Pressed
                ? InputButtonState.JustPressed
                : InputButtonState.Released;
        }

        if (oldMouseState.XButton2 == ButtonState.Pressed)
        {
            mouseState.Button5 = newMouseState.XButton2 == ButtonState.Pressed
                ? InputButtonState.Pressed
                : InputButtonState.JustReleased;
        }
        else
        {
            mouseState.Button5 = newMouseState.XButton2 == ButtonState.Pressed
                ? InputButtonState.JustPressed
                : InputButtonState.Released;
        }
        oldMouseState = newMouseState;

        for (var i = 0; i < gamepadStates.Length; i++)
        {
            var oldState = oldGpStates[i];
            var newState = GamePad.GetState((PlayerIndex)i);

            gamepadStates[i].IsConnected = newState.IsConnected;
            gamepadStates[i].LeftStick = newState.ThumbSticks.Left.ToVec2F();
            gamepadStates[i].RightStick = newState.ThumbSticks.Right.ToVec2F();
            gamepadStates[i].LeftTrigger = newState.Triggers.Left;
            gamepadStates[i].RightTrigger = newState.Triggers.Right;

            if (oldState.DPad.Up == ButtonState.Pressed)
            {
                gamepadStates[i].DPadUp = newState.DPad.Up == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].DPadUp = newState.DPad.Up == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.DPad.Down == ButtonState.Pressed)
            {
                gamepadStates[i].DPadDown = newState.DPad.Down == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].DPadDown = newState.DPad.Down == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.DPad.Left == ButtonState.Pressed)
            {
                gamepadStates[i].DPadLeft = newState.DPad.Left == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].DPadLeft = newState.DPad.Left == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.DPad.Right == ButtonState.Pressed)
            {
                gamepadStates[i].DPadRight = newState.DPad.Right == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].DPadRight = newState.DPad.Right == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }

            if (oldState.Buttons.A == ButtonState.Pressed)
            {
                gamepadStates[i].A = newState.Buttons.A == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].A = newState.Buttons.A == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.B == ButtonState.Pressed)
            {
                gamepadStates[i].B = newState.Buttons.B == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].B = newState.Buttons.B == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.X == ButtonState.Pressed)
            {
                gamepadStates[i].X = newState.Buttons.X == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].X = newState.Buttons.X == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.Y == ButtonState.Pressed)
            {
                gamepadStates[i].Y = newState.Buttons.Y == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].Y = newState.Buttons.Y == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                gamepadStates[i].LB = newState.Buttons.LeftShoulder == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].LB = newState.Buttons.LeftShoulder == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                gamepadStates[i].RB = newState.Buttons.RightShoulder == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].RB = newState.Buttons.RightShoulder == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.LeftStick == ButtonState.Pressed)
            {
                gamepadStates[i].LS = newState.Buttons.LeftStick == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].LS = newState.Buttons.LeftStick == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.RightStick == ButtonState.Pressed)
            {
                gamepadStates[i].RS = newState.Buttons.RightStick == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].RS = newState.Buttons.RightStick == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.Start == ButtonState.Pressed)
            {
                gamepadStates[i].Start = newState.Buttons.Start == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].Start = newState.Buttons.Start == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
            
            if (oldState.Buttons.Back == ButtonState.Pressed)
            {
                gamepadStates[i].Back = newState.Buttons.Back == ButtonState.Pressed
                    ? InputButtonState.Pressed
                    : InputButtonState.JustReleased;
            }
            else
            {
                gamepadStates[i].Back = newState.Buttons.Back == ButtonState.Pressed
                    ? InputButtonState.JustPressed
                    : InputButtonState.Released;
            }
        }
    }
}