using Kasane2D.Input.Enums;

namespace Kasane2D.Input.Interfaces;

public interface IInputSystem
{
    public InputButtonState Check(KeyKind key);
}