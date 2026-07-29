using MgColor = Microsoft.Xna.Framework.Color;
using KasaneColor = Kasane2D.Graphics.Types.Color;

namespace Kasane2D.MonoGame.Graphics.Extensions;

internal static class TypesExtensions
{
    public static MgColor ToMgColor(this KasaneColor color)
    {
        return new(color.R, color.G, color.B, color.A);
    }

    public static KasaneColor ToKasaneColor(this MgColor color)
    {
        return new() { R = color.R, G = color.G, B = color.B, A = color.A };
    }
}