using Kasane2D.Types;
using Microsoft.Xna.Framework;

namespace Kasane2D.MonoGame.Extensions;

internal static class PrimitivesExtensions
{
    public static Point ToPoint(this Vec2I vec)
    {
        return new(vec.X, vec.Y);
    }

    public static Vec2I ToVec2I(this Point point)
    {
        return new(point.X, point.Y);
    }
    
    public static Vec2F ToVec2F(this Vector2 vec)
    {
        return new(vec.X, vec.Y);
    }

    public static Rectangle ToRectangle(this Rect rect)
    {
        return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
}