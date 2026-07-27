namespace Kasane2D.Types;

public readonly record struct Bezier(Vec2F Start, Vec2F End, Vec2F Control)
{
    public Vec2F Interpolate(float t)
    {
        var a = Start.Lerp(Control, t);
        var b = Control.Lerp(End, t);

        return a.Lerp(b, t);
    }
}