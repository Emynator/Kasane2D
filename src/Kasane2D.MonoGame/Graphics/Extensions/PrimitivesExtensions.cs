using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework;

namespace Kasane2D.MonoGame.Graphics.Extensions;

internal static class PrimitivesExtensions
{
    public static Point ToPoint(this Vec2I vec)
    {
        return new(vec.X, vec.Y);
    }

    public static Rectangle ToRectangle(this Rect rect)
    {
        return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
}