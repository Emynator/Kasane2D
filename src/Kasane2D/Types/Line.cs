namespace Kasane2D.Types;

public readonly record struct Line(Vec2F Start, Vec2F End)
{
    public bool Intersects(Line other)
    {
        var direction = Start - End;
        var otherDirection = other.Start - other.End;
        var div = direction.Cross(otherDirection);
        var diff = Start - other.Start;
        if (MathF.Abs(div) < 1e-6)
        {
            return MathF.Abs(direction.Cross(diff)) < 1e-6;
        }
        
        var s = direction.Cross(diff);
        var t = otherDirection.Cross(diff);

        return div > 0.0f
            ? s >= 0.0f && s <= div && t >= 0.0f && t <= div
            : s <= 0.0f && s >= div && t <= 0.0f && t >= div;
    }
}