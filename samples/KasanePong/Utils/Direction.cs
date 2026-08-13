using Kasane2D.Types;

namespace KasanePong.Utils;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public static class DirectionExtensions
{
    public static Vec2F ToVec2F(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vec2F.Up,
            Direction.Down => Vec2F.Down,
            Direction.Left => Vec2F.Left,
            Direction.Right => Vec2F.Right,
            _ => Vec2F.Zero,
        };
    }
}